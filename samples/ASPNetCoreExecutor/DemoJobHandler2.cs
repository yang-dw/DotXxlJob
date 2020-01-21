using System.Threading.Tasks;
using DotXxlJob.Core;
using com.xxl.job.core.biz.model;
using DotXxlJob.Core.Model;
using System.Threading;
using System;

namespace ASPNetCoreExecutor
{
    /// <summary>
    /// 示例Job，只是写个日志
    /// </summary>
    [JobHandler("demoJobHandler2")]
    public class DemoJobHandler2:AbstractJobHandler
    {
        IServiceProvider serviceProvider;
        public DemoJobHandler2(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }
        public override Task<ReturnT> Execute(JobExecuteContext context)
        {
            for (int i = 0; i < 8; i++)
            {
                Thread.Sleep(3000);
                context.JobLogger.Log("demoJobHandler2,parameter:{0}", context.JobParameter);
            }
            return Task.FromResult(ReturnT.SUCCESS);
        }
    }
}