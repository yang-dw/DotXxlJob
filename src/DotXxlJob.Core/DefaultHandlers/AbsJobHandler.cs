using System.Threading.Tasks;
using com.xxl.job.core.biz.model;
using DotXxlJob.Core.Model;

namespace DotXxlJob.Core.DefaultHandlers
{
    public abstract class AbsJobHandler:IJobHandler
    {
        public virtual void Dispose()
        {
           
        }

        public abstract Task<ReturnT> Execute(JobExecuteContext context);

    }
}