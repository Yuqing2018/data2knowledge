using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Service.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Threading.Tasks;
using System;

namespace MusicKG.Scheduler.Engine.Action
{
    public abstract class JobAction<TJobContext, TActionOptions, TActionInput> : IJobAction 
        where TJobContext : JobTaskContext
        where TActionInput : class 
        where TActionOptions : JobActionOptions
    {
        protected string defaultExecutorName;
        protected ILogger logger;
        protected TJobContext jobContext;
        protected Func<string, IActionExecutor> executorFactory;

        public JobAction(Func<string, IActionExecutor> executorFactory, ILogger logger)
        {
            this.executorFactory = executorFactory;
            this.logger = logger;
            ActionType = GetType().Name;
        }

        public string ActionType { get; protected set; }

        public async Task<JobActionResult> ExecuteAsync(JobActionDataModel jobAction, object actionData, int actionRetryTimes)
        {
            var data = CastData(jobAction.ActionId, actionData);

            var options = CastOptions(jobAction.ActionId, jobAction.Options);

            int actionRetyTime = 0;

            while (true)
            {
                try
                {
                    return await DoActionAsync(jobAction, options, data);
                }
                catch (Exception ex)
                {
                    await RevertAsync(jobAction);

                    if (actionRetyTime < actionRetryTimes)
                    {
                        logger.LogActionError(jobAction.ActionId, $"Execute action error {actionRetyTime} times. Details: {ex}");

                        logger.LogActionInfo(jobAction.ActionId, "Reverting action.");

                        logger.LogActionInfo(jobAction.ActionId, "Retring action.");

                        actionRetyTime += 1;
                    }
                    else
                        throw new JobExecuteException($"Execute action failed {actionRetryTimes}.", jobAction.ActionId, ex);
                }
            }
        }

        public virtual JobTaskContext CreateContext(DateTime from, DateTime to, JobTaskServiceModel taskDefine)
        {
            var result = Activator.CreateInstance<TJobContext>();

            result.From = from;
            result.To = to;
            result.Task = taskDefine;
            
            result.Initialize();

            return result;
        }

        public virtual void SetContext(JobTaskContext context)
        {
            jobContext = context as TJobContext;
        }

        public virtual async Task RevertAsync(JobActionDataModel jobAction)
        {
            var options = CastOptions(jobAction.ActionId, jobAction.Options);

            var executor = GetExecutor(jobAction.ActionId, options);

            await executor.RevertAsync(jobAction.ActionId, jobContext);
        }

        protected abstract JobActionResult GetActionResult(JobActionDataModel action, TActionOptions options, string message, object nextActionData);

        protected async virtual Task<JobActionResult> DoActionAsync(JobActionDataModel action, TActionOptions options, TActionInput actionData)
        {
            if (options.Ignore && action.IsDefault)
            {
                var message = "The default action is configured as Ignore, the task is finished.";
                logger.LogActionInfo(action.ActionId, message);
                return JobActionResult.Finish(message);
            }

            var executor = GetExecutor(action.ActionId, options);

            var nextActionData = await executor.ExecuteAsync(action.ActionId, jobContext, options, actionData);

            return GetActionResult(action, options, 
                $"The action {action.ActionId} executed successfully with executor {executor.GetType().Name}.", 
                nextActionData);
        }

        private TActionOptions CastOptions(string actionId, BsonDocument options)
        {
            TActionOptions result = null;

            try
            {
                result = BsonSerializer.Deserialize<TActionOptions>(options);
            }
            catch (Exception ex)
            {
                throw new JobExecuteException($"Cast action options failed.", actionId, ex);
            }

            if (result == null)
                throw new JobExecuteException($"Cast action options failed.", actionId);

            return result;
        }

        private TActionInput CastData(string actionId, object data)
        {
            var result = data as TActionInput;

            if (result == null)
            {
                throw new JobExecuteException($"Cast action data failed. Input data for action '{actionId}' must be type of {typeof(TActionInput).Name}", actionId);
            }

            return result;
        }

        private IActionExecutor GetExecutor(string actionId, TActionOptions options)
        {
            var executor = executorFactory?.Invoke(options.ExecutorName);

            if (executor == null)
            {
                logger.LogActionWarning(actionId, $"Executor named '{options.ExecutorName}' is not defined. Use default instead.");

                var defaultExecutor = executorFactory?.Invoke(defaultExecutorName);

                if (defaultExecutor == null)
                    throw new JobExecuteException($"Executor named '{options.ExecutorName}' and its default executor {defaultExecutorName} are all not defined.", actionId);

                executor = defaultExecutor;
            }

            return executor;
        }
    }
}
