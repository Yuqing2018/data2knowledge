using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Engine.Action
{
    public abstract class ActionExecutor<TContext, TOptions, TInputData, TOutputData> : IActionExecutor
        where TContext : JobTaskContext
        where TOptions : JobActionOptions
        where TInputData : class
        where TOutputData : class
    {
        protected ILogger<ActionExecutor<TContext, TOptions, TInputData, TOutputData>> logger;

        public ActionExecutor(ILogger<ActionExecutor<TContext, TOptions, TInputData, TOutputData>> logger)
        {
            this.logger = logger;
            this.ExecutorType = GetType().Name;
        }

        public string ExecutorType { get; protected set; }

        public virtual async Task<object> ExecuteAsync(string actionId, JobTaskContext context, JobActionOptions options, object data)
        {
            if (data == null)
            {
                logger.LogActionInfo(actionId, "The input data is empty, ignore executing.");
                return new object();
            }

            return await ExecuteInternalAsync(actionId, 
                CastData<TContext>(actionId, context), 
                CastData<TOptions>(actionId, options), 
                CastData<TInputData>(actionId, data));
        }

        public virtual async Task RevertAsync(string actionId, JobTaskContext context)
        {
            await RevertInternalAsync(actionId,
                CastData<TContext>(actionId, context));
        }

        protected abstract Task<TOutputData> ExecuteInternalAsync(string actionId, TContext context, TOptions options, TInputData data);

        protected abstract Task RevertInternalAsync(string actionId, TContext context);

        protected T CastData<T>(string actionId, object data) where T : class
        {
            if (data == null)
                return null;

            var result = data as T;

            if (result == null)
                throw new JobExecuteException($"Cast {data.GetType().Name} to {typeof(T)} failed.", actionId);

            return result;
        }
    }
}
