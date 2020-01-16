using System.Threading.Tasks;
using com.xxl.job.core.biz.model;
using DotXxlJob.Core.Model;

namespace DotXxlJob.Core
{
    public interface ITaskExecutor
    {
        string GlueType { get; }

        Task<ReturnT> Execute(TriggerParam triggerParam);
    }
}