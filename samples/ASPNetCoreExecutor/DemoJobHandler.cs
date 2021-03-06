using System.Threading.Tasks;
using DotXxlJob.Core;
using com.xxl.job.core.biz.model;
using DotXxlJob.Core.Model;
using System.Threading;

namespace ASPNetCoreExecutor
{
    /// <summary>
    /// 示例Job，只是写个日志
    /// </summary>
    [JobHandler("demoJobHandler")]
    public class DemoJobHandler:AbstractJobHandler
    {
        
        public override Task<ReturnT> Execute(JobExecuteContext context)
        {
            Thread.Sleep(25000);
            context.JobLogger.Log("receive demo job handler,parameter:{0}",context.JobParameter);

            return Task.FromResult(ReturnT.SUCCESS);
        }
    }
}