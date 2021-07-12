using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Engine.Settings;
using MusicKG.Scheduler.DataAccess.Enums;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.Service.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Engine
{
    public class JobTaskExecutor : IJobTaskExecutor
    {
        private readonly ILogger<JobTaskExecutor> logger;
        private readonly IJobTaskService jobTaskService;
        private readonly ILocker locker;
        private readonly JobTaskSettings settings;
        private readonly Func<string, IJobAction> actionFactory;

        public JobTaskExecutor(Func<string, IJobAction> actionFactory, 
            JobTaskSettings settings, IJobTaskService jobTaskService,
            ILocker locker, ILogger<JobTaskExecutor> logger)
        {
            this.locker = locker;
            this.jobTaskService = jobTaskService;
            this.settings = settings;
            this.actionFactory = actionFactory;
            this.logger = logger;
        }

        public async Task<JobTaskExecuteResultServiceModel> ExecuteAsync(JobServiceModel job, JobTaskServiceModel taskDefine)
        {
            var currentRunAt = DateTime.UtcNow;

            if (!await locker.TryLockAsync(taskDefine.Id))
            {
                logger.LogInformation($"[{job.Name}] - [{taskDefine.Name}]: Task is locked by other executor...");

                return new JobTaskExecuteResultServiceModel
                {
                    IsSucceed = false,
                    RunAt = currentRunAt,
                    DeadAction = "N/A",
                    Message = "Task is locked by other executor"
                };
            }

            JobTaskContext context = null;

            try
            {
                var (startTime, endTime) = GetStartTime(taskDefine, currentRunAt);

                logger.LogInformation($"-------------------------------------------------------------------");
                logger.LogInformation($"[{job.Name}] - [{taskDefine.Name}]: Task started. From: {startTime}, To: {endTime}");
                logger.LogInformation($"-------------------------------------------------------------------");

                await jobTaskService.UpdateStatusAsync(taskDefine.Id, TaskExecutionResult.执行中);

                var actionDefine = job.Actions?.FirstOrDefault(a => a.IsDefault);

                if (actionDefine == null)
                {
                    throw new NullReferenceException("Job has no default action!");
                }

                object actionData = new DefaultActionData();

                while (true)
                {
                    var actionInstance = actionFactory?.Invoke(actionDefine.ActionId);

                    if (actionInstance == null)
                        throw new TypeLoadException($"Cannot create action instance.");

                    if (context == null)
                    {
                        context = actionInstance.CreateContext(startTime, endTime, taskDefine);
                        context.TaskRunTime = currentRunAt;
                    }

                    actionInstance.SetContext(context);

                    logger.LogActionInfo(actionDefine.ActionId, "Action start executing.");

                    var result = await actionInstance.ExecuteAsync(actionDefine, actionData, settings.ActionRetryTimes);

                    logger.LogActionInfo(actionDefine.ActionId, $"Action finish executing with message '{result.Message}'.");

                    if (result.IsFinished)
                    {
                        var executeResult = new JobTaskExecuteResultServiceModel
                        {
                            IsSucceed = true,
                            DeadAction = actionDefine.ActionId,
                            RunAt = currentRunAt,
                            Message = result.Message
                        };

                        await jobTaskService.AppendHistoryAsync(taskDefine.Id, executeResult);

                        logger.LogInformation($"[{job.Name}] - [{taskDefine.Name}]: Task finished...");

                        return executeResult;
                    }

                    actionDefine = job.Actions?.FirstOrDefault(a => a.ActionId == result.NextActionId);
                    actionData = result.NextActionData;

                    if (actionDefine == null)
                    {
                        throw new NullReferenceException($"Job action define does not be found.");
                    }
                }
            }
            catch (JobExecuteException ex)
            {
                logger.LogError($"[{job.Name}] - [{ex.ActionId}] - [{ex.Message}]. Details: {ex}");

                var executeResult = new JobTaskExecuteResultServiceModel
                {
                    IsSucceed = false,
                    DeadAction = ex.ActionId,
                    RunAt = currentRunAt,
                    Message = ex.ToString()
                };

                await jobTaskService.AppendHistoryAsync(taskDefine.Id, executeResult);

                if (context != null)
                    await RevertAsync(job, context);

                return executeResult;
            }
            catch (Exception ex)
            {
                logger.LogError($"[{job.Name}]: Encounter an unhandled exception when executing job. Details: {ex}");

                var exectueResult = new JobTaskExecuteResultServiceModel
                {
                    IsSucceed = false,
                    DeadAction = JobTaskContext.Unknown,
                    RunAt = currentRunAt,
                    Message = ex.ToString()
                };

                await jobTaskService.AppendHistoryAsync(taskDefine.Id, exectueResult);

                if (context != null)
                    await RevertAsync(job, context);

                return exectueResult;
            }
            finally
            {
                context?.Dispose();
                await locker.ReleaseAsync(taskDefine.Id);
            }
        }

        private async Task RevertAsync(JobServiceModel job, JobTaskContext context)
        {
            foreach (var action in job.Actions)
            {
                var actionInstance = actionFactory?.Invoke(action.ActionId);

                if (actionInstance == null)
                    throw new TypeLoadException($"Cannot create action instance.");

                actionInstance.SetContext(context);

                await actionInstance.RevertAsync(action);
            }
        }

        private (DateTime startTime, DateTime endTime) GetStartTime(JobTaskServiceModel jobTask, DateTime currentRunAt)
        {
            DateTime start, end;

            if (jobTask.IsRunOnce)
            {
                start = jobTask.StartTime.HasValue ? jobTask.StartTime.Value : jobTask.MerchantCreationTime;
                end = jobTask.EndTime.HasValue ? jobTask.EndTime.Value : currentRunAt;
            }
            else
            {
                if (jobTask.LastSucceedAt == null || jobTask.LastSucceedAt == DateTime.MinValue || jobTask.LastSucceedAt == DateTime.MaxValue)
                    start = jobTask.MerchantCreationTime;
                else
                    start = jobTask.LastSucceedAt.Value;

                end = currentRunAt;
            }

            return (start, end);
        }
    }
}
