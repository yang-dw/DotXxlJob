using System;
using System.IO;
using com.xxl.job.core.rpc.codec;
using DotXxlJob.Core.Model;
using hessiancsharp.io;

namespace DotXxlJob.Core
{
    public static class HessianSerializer
    {
     
        public static RpcRequest DeserializeRequest(Stream stream)
        {
            RpcRequest request = null;

            try
            { 
                CHessianInput input = new CHessianInput(stream);
                request = (RpcRequest)input.ReadObject();
                return request;
            }
            catch (EndOfStreamException)
            {
                //没有数据可读了
            }
            return request;

        }
     
        
        public static void SerializeRequest(Stream stream,RpcRequest req)
        {
            CHessianOutput output = new CHessianOutput(stream);
            output.WriteObject(req);
        }
        
        public static void SerializeResponse(Stream stream,RpcResponse res)
        {
            CHessianOutput output = new CHessianOutput(stream);
            output.WriteObject(res);
        }
        
  
        public static RpcResponse DeserializeResponse(Stream resStream)
        {
            RpcResponse rsp = null;

            try
            {
                CHessianInput input = new CHessianInput(resStream);
                rsp = (RpcResponse)input.ReadObject();

                return rsp;

            }
            catch (EndOfStreamException)
            {
                //没有数据可读了
            }
            catch
            {
                //TODO: do something?
            }

            return rsp;
        }
        
      
    }

  
}