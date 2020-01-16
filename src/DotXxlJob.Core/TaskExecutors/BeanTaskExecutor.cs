using System.Threading.Tasks;
using com.xxl.job.core.biz.model;
using DotXxlJob.Core.Model;

namespace DotXxlJob.Core.TaskExecutors
{
    /// <summary>
    /// 实现 IJobHandler的执行器
    /// </summary>
    public class BeanTaskExecutor:ITaskExecutor
    {
        private readonly IJobHandlerFactory _handlerFactory;
        private readonly IJobLogger _jobLogger;

        public BeanTaskExecutor(IJobHandlerFactory handlerFactory,IJobLogger jobLogger)
        {
            this._handlerFactory = handlerFactory;
            this._jobLogger = jobLogger;
        }
        
        public string GlueType { get; } = Constants.GlueType.BEAN;
        
        public Task<ReturnT> Execute(TriggerParam triggerParam)
        {
            var handler = _handlerFactory.GetJobHandler(triggerParam.executorHandler);

            if (handler == null)
            {
               return Task.FromResult(ReturnT.Failed($"job handler [{triggerParam.executorHandler} not found."));
            }
            var context = new JobExecuteContext(this._jobLogger, triggerParam.executorParams);
            return handler.Execute(context);
        }
    }
}