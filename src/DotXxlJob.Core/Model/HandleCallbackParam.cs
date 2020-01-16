using System.Runtime.Serialization;
using com.xxl.job.core.biz.model;

namespace com.xxl.job.core.biz.model
{
    
    public class HandleCallbackParam
    {
        public HandleCallbackParam()
        {
            
        }
        public HandleCallbackParam(TriggerParam triggerParam, ReturnT result)
        {
            this.logId = triggerParam.logId;
            this.logDateTim = triggerParam.logDateTim;
            this.executeResult = result;
        }
        
       
        public int CallbackRetryTimes { get; set; }
         
        public int logId; 
        public long logDateTim; 
        public ReturnT executeResult;
    }
}