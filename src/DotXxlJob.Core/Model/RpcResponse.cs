using System.Runtime.Serialization;

namespace com.xxl.job.core.rpc.codec
{
    public class RpcResponse
    {
        public string requestId;

        public string error;

        public object result;

        public bool IsError { get { return error != null; } }
    }
}