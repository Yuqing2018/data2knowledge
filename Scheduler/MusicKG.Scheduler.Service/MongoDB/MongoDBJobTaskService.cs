using MusicKG.Scheduler.DataAccess;
using MusicKG.Scheduler.DataAccess.Enums;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Service.Helpers;
using MusicKG.Scheduler.Service.Models;
using MusicKG.Scheduler.Service.Resources;
using MusicKG.Scheduler.Service.Settings;
using MusicKG.Scheduler.Service.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Service.MongoDB
{
    public class MongoDBJobTaskService : IJobTaskService
    {
        private readonly ISchedulerDbContext context;
        private readonly TaskHistorySettings settings;
        private readonly ILogger<MongoDBJobTaskService> logger;

        public MongoDBJobTaskService(ISchedulerDbContext context, TaskHistorySettings settings, ILogger<MongoDBJobTaskService> logger)
        {
            this.settings = settings;
            this.context = context;
            this.logger = logger;
        }

        public async Task CreateAsync(string jobId, JobTaskManagementServiceModel jobTask)
        {
            var dataModel = new JobTaskDataModel
            {
                Id = Guid.NewGuid().ToString(),
                JobId = jobId,
                Name = jobTask.Name,
                Description = jobTask.Description,
                IsDeleted = false,
                IsRunOnce = jobTask.IsRunOnce,
                StartTime = jobTask.StartTime,
                EndTime = jobTask.EndTime,
                Executor = new ExecutorDataModel
                {
                    Domain = jobTask.ExecutorDomain,
                    UserName = jobTask.ExecutorName,
                    Password = jobTask.ExecutorPassword
                },
                Status = new JobTaskStatus
                {
                    TryTimes = 0,
                    LastStatus = TaskExecutionResult.准备中,
                    LastRunAt = DateTime.MinValue,
                    LastSucceedAt = DateTime.MinValue,
                    History = new List<JobTaskHistory>()
                },
                MerchantCreationTime = jobTask.MerchantCreationTime,
                ManagedAt = DateTime.UtcNow,
                ManagedBy = jobTask.ManagedBy,
                TaskDefine = jobTask.TaskDefine.ToBsonDocument()
            };

            try
            {
                await context.JobTasks.InsertOneAsync(dataModel);
            }
            catch (MongoWriteException e)
            {
                var message = SchedulerMessages.JobTaskManageFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = string.Format(SchedulerMessages.JobTaskNameExistMessage, jobTask.Name);

                logger?.LogError(e, message);

                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task UpdateAsync(string jobId, string id, JobTaskManagementServiceModel jobTask)
        {
            var builder = Builders<JobTaskDataModel>.Update;

            var update = builder.Set(t => t.Name, jobTask.Name)
                .Set(t => t.Description, jobTask.Description)
                .Set(t => t.IsRunOnce, jobTask.IsRunOnce)
                .Set(t => t.StartTime, jobTask.StartTime)
                .Set(t => t.EndTime, jobTask.EndTime)
                .Set(t => t.Executor, new ExecutorDataModel
                {
                    Domain = jobTask.ExecutorDomain,
                    UserName = jobTask.ExecutorName,
                    Password = jobTask.ExecutorPassword
                })
                .Set(t => t.TaskDefine, jobTask.TaskDefine.ToBsonDocument())
                .Set(t => t.ManagedAt, DateTime.UtcNow)
                .Set(t => t.MerchantCreationTime, jobTask.MerchantCreationTime)
                .Set(t => t.ManagedBy, jobTask.ManagedBy);

            try
            {
                var result = await context.JobTasks.UpdateOneAsync(t => t.JobId == jobId && t.Id == id, update);
            }
            catch (MongoWriteException e)
            {
                var message = SchedulerMessages.JobTaskManageFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = string.Format(SchedulerMessages.JobTaskNameExistMessage, jobTask.Name);

                logger?.LogError(e, message);

                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task<bool> ExistAsync(string id)
        {
            return await context.JobTasks.AsQueryable().AnyAsync(t => t.Id == id);
        }

        public async Task<bool> IsTimeSpanOverlap(string jobId, DateTime from, DateTime to)
        {
            //!(t2 < t3 || t4 <t1)    =>  t3<=t2 &&t4>=t1
            return await context.JobTasks.AsQueryable().AnyAsync(x => x.JobId == jobId
            && x.IsDeleted == false && x.EndTime >= from && x.StartTime <= to);
        }

        public async Task DeleteAsync(string jobId, string id)
        {
            var update = Builders<JobTaskDataModel>.Update.Set(t => t.IsDeleted, true);

            await context.JobTasks.UpdateOneAsync(t => t.JobId == jobId && t.Id == id, update);
        }

        public async Task<JObject> GetDetailsAsync(string jobId, string id)
        {
            var bson = await context.JobTasks.AsQueryable().Where(t => t.JobId == jobId && t.Id == id)
                .Select(t => t.TaskDefine).FirstOrDefaultAsync();

            return bson?.ToJObject();
        }

        public async Task<List<JobTaskHistoryServiceModel>> GetHistoryAsync(string jobId, string id)
        {
            var dataModel = await context.JobTasks.AsQueryable().Where(t => t.JobId == jobId && t.Id == id)
                .Select(t => t.Status.History).FirstOrDefaultAsync();

            return dataModel?.Select(h => new JobTaskHistoryServiceModel
            {
                IsSucceed = h.IsSucceed,
                DeadAction = h.DeadAction,
                Message = h.Message,
                RunAt = h.RunAt
            })?.ToList();
        }

        public async Task<List<JobTaskServiceModel>> ListPreparedAsync(string jobId, int maxRerunTimes)
        {
            var querable = context.JobTasks.AsQueryable().Where(t => t.JobId == jobId && !t.IsDeleted);

            querable = querable.Where(t => !t.IsRunOnce ||
                t.Status.LastStatus == TaskExecutionResult.准备中 ||
                (t.Status.LastStatus == TaskExecutionResult.执行失败 && t.Status.TryTimes <= maxRerunTimes));

            var result = await querable.ToListAsync();

            var job = await context.Jobs.AsQueryable().FirstOrDefaultAsync(j => j.Id == jobId);

            return result?.Select(t => new JobTaskServiceModel
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                IsRunOnce = t.IsRunOnce,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                Executor = t.Executor?.UserName,
                ExecutorDomain = t.Executor?.Domain,
                ExecutorPassword = t.Executor?.Password,
                MerchantCreationTime = t.MerchantCreationTime,
                Job = new JobServiceModel
                {
                    Id = job.Id,
                    Name = job.Name,
                    JobType = job.JobType
                },
                LastRunAt = t.Status?.LastRunAt,
                LastStatus = t.Status?.LastStatus ?? TaskExecutionResult.准备中,
                LastSucceedAt = t.Status?.LastSucceedAt,
                ManagedAt = t.ManagedAt,
                ManagedBy = t.ManagedBy,
                IsDeleted = t.IsDeleted,
                TaskDefine = t.TaskDefine
            })?.ToList();
        }

        public async Task<List<JobTaskServiceModel>> ListAsync(string jobId,
            string keyword, TaskExecutionResult? status, string managedBy, bool? isRunOnce,
            DateTime? succeedBefore, DateTime? succeedAfter,
            DateTime? managedBefore, DateTime? managedAfter, 
            DateTime? runBefore, DateTime? runAfter, bool ignoreDeleted = true)
        {
            var querable = context.JobTasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(jobId))
                querable = querable.Where(t => t.JobId == jobId);

            if (!string.IsNullOrWhiteSpace(keyword))
                querable = querable.Where(t => t.Name.Contains(keyword) || t.Description.Contains(keyword));

            if (status.HasValue)
                querable = querable.Where(t => t.Status.LastStatus == status);

            if (!string.IsNullOrWhiteSpace(managedBy))
                querable = querable.Where(t => t.ManagedBy == managedBy);

            if (isRunOnce.HasValue)
                querable = querable.Where(t => t.IsRunOnce == isRunOnce);

            if (IsDateTimeNotEmpty(managedBefore))
                querable = querable.Where(t => t.ManagedAt < managedBefore);

            if (IsDateTimeNotEmpty(managedAfter))
                querable = querable.Where(t => t.ManagedAt >= managedAfter);

            if (IsDateTimeNotEmpty(runBefore))
                querable = querable.Where(t => t.Status.LastRunAt < runBefore);

            if (IsDateTimeNotEmpty(runAfter))
                querable = querable.Where(t => t.Status.LastRunAt >= runAfter);

            if (IsDateTimeNotEmpty(succeedBefore))
                querable = querable.Where(t => t.Status.LastSucceedAt < succeedBefore);

            if (IsDateTimeNotEmpty(succeedAfter))
                querable = querable.Where(t => t.Status.LastSucceedAt >= succeedAfter);

            if (ignoreDeleted)
                querable = querable.Where(t => !t.IsDeleted);

            var result = await (from t in querable
                                join j in context.Jobs.AsQueryable() on t.JobId equals j.Id
                                select new JobTaskServiceModel
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    Description = t.Description,
                                    IsRunOnce = t.IsRunOnce,
                                    StartTime = t.StartTime,
                                    EndTime = t.EndTime,
                                    Executor = t.Executor.UserName,
                                    LastRunAt = t.Status.LastRunAt,
                                    LastStatus = t.Status.LastStatus,
                                    LastSucceedAt = t.Status.LastSucceedAt,
                                    ManagedAt = t.ManagedAt,
                                    ManagedBy = t.ManagedBy,
                                    IsDeleted = t.IsDeleted,
                                    Job = new JobServiceModel
                                    {
                                        Id = j.Id,
                                        Name = j.Name
                                    }
                                }).ToListAsync();

            return result;
        }

        public async Task<JobTaskServiceModel> GetAsync(string jobId, string id)
        {
            var result = await context.JobTasks.AsQueryable().FirstOrDefaultAsync(t => t.JobId == jobId && t.Id == id);

            if (result == null)
                ErrorHelper.ThrowException(SchedulerMessages.JobTaskNotExistsMessage, HttpStatusCode.NotFound);

            return new JobTaskServiceModel
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                IsRunOnce = result.IsRunOnce,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Executor = result.Executor?.UserName,
                ExecutorDomain = result.Executor?.Domain,
                ExecutorPassword = result.Executor?.Password,
                MerchantCreationTime = result.MerchantCreationTime,
                LastRunAt = result.Status?.LastRunAt,
                LastStatus = result.Status?.LastStatus ?? TaskExecutionResult.准备中,
                LastSucceedAt = result.Status?.LastSucceedAt,
                ManagedAt = result.ManagedAt,
                ManagedBy = result.ManagedBy,
                IsDeleted = result.IsRunOnce,
                TaskDefine = result.TaskDefine
            };
        }

        public async Task<JobTaskStatus> GetStatusAsync(string jobId, string id)
        {
            var result = await context.JobTasks.AsQueryable().FirstOrDefaultAsync(t => t.JobId == jobId && t.Id == id);

            if (result == null)
                ErrorHelper.ThrowException(SchedulerMessages.JobTaskNotExistsMessage, HttpStatusCode.NotFound);

            return result.Status;
        }

        public async Task UpdateStatusAsync(string id, TaskExecutionResult status)
        {
            await context.JobTasks.UpdateOneAsync(t => t.Id == id, 
                Builders<JobTaskDataModel>.Update.Set(u => u.Status.LastStatus, status));
        }

        public async Task AppendHistoryAsync(string id, JobTaskExecuteResultServiceModel runStatus)
        {
            var currentTime = DateTime.UtcNow;

            var exists = await ExistAsync(id);

            if (!exists)
                return;

            var builder = Builders<JobTaskDataModel>.Update;

            var historiesToKeep = settings.HistoriesToKeep < 1 ? 1 : settings.HistoriesToKeep;

            var histories = await context.JobTasks.AsQueryable().Where(t => t.Id == id).SelectMany(t => t.Status.History).ToListAsync();

            histories = histories.TakeLast(historiesToKeep - 1).ToList();

            histories.Add(new JobTaskHistory
            {
                DeadAction = runStatus.DeadAction,
                IsSucceed = runStatus.IsSucceed,
                Message = runStatus.Message,
                RunAt = runStatus.RunAt,
                FinishedAt = currentTime
            });

            var update = builder.Set(t => t.Status.LastRunAt, runStatus.RunAt)
                .Set(t => t.Status.LastFinishedAt, currentTime);

            if (runStatus.IsSucceed)
            {
                update = update.Set(t => t.Status.LastStatus, TaskExecutionResult.执行成功)
                    .Set(t => t.Status.LastSucceedAt, currentTime)
                    .Set(t => t.Status.TryTimes, 0);
            }
            else
            {
                update = update.Set(t => t.Status.LastStatus, TaskExecutionResult.执行失败)
                    .Inc(t => t.Status.TryTimes, 1);
            }

            update = update.Set(t => t.Status.History, histories);

            await context.JobTasks.UpdateOneAsync(t => t.Id == id, update);
        }

        private bool IsDateTimeNotEmpty(DateTime? dateTime)
        {
            return dateTime.HasValue && dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue;
        }
    }
}
