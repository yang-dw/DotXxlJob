using System.Runtime.Serialization;

namespace com.xxl.job.core.biz.model
{ 
    public class ReturnT
    {
        public const int SUCCESS_CODE = 200;
        public const int FAIL_CODE = 500;

        public static readonly ReturnT SUCCESS = new ReturnT(SUCCESS_CODE, null);
        public static readonly ReturnT FAIL = new ReturnT(FAIL_CODE, null);
        public static readonly ReturnT FAIL_TIMEOUT = new ReturnT(502, null);
        
        public ReturnT() { }

        public ReturnT(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }


        public int code;
        public string msg;

        public object content;
        
      

        public static ReturnT Failed(string msg)
        {
             return new ReturnT(FAIL_CODE, msg);
        }
        public static ReturnT Success(string msg)
        {
            return new ReturnT(SUCCESS_CODE, msg);
        }
        
    }
    
   
   
}