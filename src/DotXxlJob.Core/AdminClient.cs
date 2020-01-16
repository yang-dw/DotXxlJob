using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using com.xxl.job.core.biz.model;
using com.xxl.job.core.rpc.codec;
using DotXxlJob.Core.Config;
using DotXxlJob.Core.Model;
using hessiancsharp.io;
using java.lang;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotXxlJob.Core
{
    public class AdminClient
    {
        private static readonly string MAPPING = "/api";
        private readonly XxlJobExecutorOptions _options;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<AdminClient> _logger;

        private List<AddressEntry> _addresses;

        private int _currentIndex;

        public AdminClient(IOptions<XxlJobExecutorOptions> optionsAccessor
            , IHttpClientFactory clientFactory
            , ILogger<AdminClient> logger)
        {
            Preconditions.CheckNotNull(optionsAccessor?.Value, "XxlJobExecutorOptions");

            this._options = optionsAccessor?.Value;
            this._clientFactory = clientFactory;
            this._logger = logger;
            InitAddress();
        }

        private void InitAddress()
        {
            this._addresses = new List<AddressEntry>();
            foreach (var item in this._options.AdminAddresses.Split(';'))
            {
                try
                {
                    var uri = new Uri(item + MAPPING);
                    var entry = new AddressEntry { RequestUri = uri };
                    this._addresses.Add(entry);
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "init admin address error.");
                }
            }
        }

        public Task<ReturnT> Callback(List<HandleCallbackParam> callbackParamList)
        {
            return InvokeRpcService("callback", new ArrayList { new Class { name = Constants.JavaListFulName } }, callbackParamList);
        }

        public Task<ReturnT> Registry(RegistryParam registryParam)
        {
            return InvokeRpcService("registry", new ArrayList { new Class { name = "com.xxl.job.core.biz.model.RegistryParam" } }, registryParam, true);
        }

        public Task<ReturnT> RegistryRemove(RegistryParam registryParam)
        {
            return InvokeRpcService("registryRemove", new ArrayList { new Class { name = "com.xxl.job.core.biz.model.RegistryParam" } }, registryParam, true);
        }

        private async Task<ReturnT> InvokeRpcService(string methodName, ArrayList parameterTypes,
            object parameters, bool polling = false)
        {
            var request = new RpcRequest {
                requestId = Guid.NewGuid().ToString("N"),
                createMillisTime = DateTime.Now.GetTotalMilliseconds(),
                accessToken = _options.AccessToken,
                className = "com.xxl.job.core.biz.AdminBiz",
                methodName = methodName,
                parameterTypes = parameterTypes,
                parameters = new ArrayList { parameters }
            };
            byte[] postBuf=null;
            using (var stream = new MemoryStream())
            {
                HessianSerializer.SerializeRequest(stream, request);

                try
                {
                    postBuf = stream.ToArray();
                }
                catch 
                { 
                
                }
            }

            var triedTimes = 0;
            var retList = new List<ReturnT>();

            using (var client = this._clientFactory.CreateClient(Constants.DefaultHttpClientName))
            {
                while (triedTimes++ < this._addresses.Count)
                {
                    var address = this._addresses[this._currentIndex];
                    this._currentIndex = (this._currentIndex + 1) % this._addresses.Count;
                    if (!address.CheckAccessible())
                        continue;

                    Stream resStream;
                    try
                    {
                        resStream = await DoPost(client, address, postBuf);
                        address.Reset();
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex, "request admin error.{0}", ex.Message);
                        address.SetFail();
                        continue;
                    }

                    RpcResponse res = null;
                    try
                    {
                        /*
                       using (StreamReader reader = new StreamReader(resStream))
                       {
                           string content  = await reader.ReadToEndAsync();

                           this._logger.LogWarning(content);
                       }
                       */
                        res = HessianSerializer.DeserializeResponse(resStream);
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex, "DeserializeResponse error:{errorMessage}", ex.Message);
                    }

                    if (res == null)
                    {
                        retList.Add(ReturnT.Failed("response is null"));
                    }
                    else if (res.IsError)
                    {
                        retList.Add(ReturnT.Failed(res.error));
                    }
                    else if (res.result is ReturnT ret)
                    {
                        retList.Add(ret);
                    }
                    else
                    {
                        retList.Add(ReturnT.Failed("response is null"));
                    }

                    if (!polling)
                    {
                        return retList[0];
                    }
                }

                if (retList.Count > 0)
                {
                    return retList.Last();
                }
            }
            throw new Exception("xxl-rpc server address not accessible.");
        }
        private CHessianInput GetHessianInput(string content)
        {
            var buffer = new byte[content.Length / 2];
            for (int i = 0; i < content.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(content.Substring(i, 2), 16);
            }
             
            var stream = new MemoryStream(buffer);
            CHessianInput input = new CHessianInput(stream);
            return input;
        }
        private async Task<Stream> DoPost(HttpClient client, AddressEntry address, byte[] postBuf)
        {
            var content = new ByteArrayContent(postBuf);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var responseMessage = await client.PostAsync(address.RequestUri, content);

            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsStreamAsync();
        }
    }
}