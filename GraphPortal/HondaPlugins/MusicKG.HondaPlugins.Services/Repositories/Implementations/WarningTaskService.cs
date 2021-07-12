using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using MusicKG.Scheduler.DataAccess;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.DataAccess.Enums;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class WarningTaskService : IWarningTaskService
    {
        private readonly ISchedulerDbContext context;
        private readonly ILogger<WarningTaskService> logger;

        /// <summary>
        /// WarningTask service constructor.
        /// </summary>
        /// <param name="context">Honda mongodb context.</param>
        public WarningTaskService(
            ISchedulerDbContext context,
            ILogger<WarningTaskService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<WarningTaskServiceModel> GetAsync(string id)
        {
            var taskDataModel = await context.JobTasks.AsQueryable()
                .Where(t => t.JobId == ConstantSettings.WarningCalculationJobId && t.Id == id)
                .FirstOrDefaultAsync();

            return new WarningTaskServiceModel(id, taskDataModel.TaskDefine)
            {
                Status = new JobTaskStatus()
                {
                    LastStatus = taskDataModel.Status.LastStatus,
                    LastRunAt = taskDataModel.Status.LastRunAt,
                    LastFinishedAt = taskDataModel.Status.LastFinishedAt,
                    LastSucceedAt = taskDataModel.Status.LastSucceedAt,
                    TryTimes = taskDataModel.Status.TryTimes
                }
            };
        }

        public async Task<List<WarningTaskIdAndNameModel>> GetTaskIdAndNameList()
        {
            var tasks = await context.JobTasks.AsQueryable()
               .Where(t => t.JobId == ConstantSettings.WarningCalculationJobId).ToListAsync();
               
            var taskIdNames = tasks?.Select(x =>
            {
                var taskDefine = BsonSerializer.Deserialize<WarningTaskDataModel>(x.TaskDefine);
                return new WarningTaskIdAndNameModel
                {
                    Id = x.Id,
                    Name = taskDefine?.Name
                };
            }).OrderBy(x => x.Name).ToList();

            return taskIdNames;
        }

        public async Task<Tuple<long, IEnumerable<WarningTaskServiceModel>>> GetWarningTasksAsync(
            WarningUnit? warningUnit, 
            WarningTaskStatus? status, 
            string createdBy,
            List<string> carModels,
            List<string> carTypes,
            List<string> yearModels,
            int from, int? size,
            List<string> taskIds = null)
        {
            var querable = context.JobTasks.AsQueryable()
                .Where(t => t.JobId == ConstantSettings.WarningCalculationJobId);

            if (taskIds != null && taskIds.Count > 0)
            {
                querable = querable.Where(x => taskIds.Contains(x.Id));
            }

            if (warningUnit.HasValue)
                querable = querable.Where(w => w.TaskDefine[nameof(WarningTaskDataModel.WarningUnit)] == warningUnit.ToString());

            if (status.HasValue)
                querable = querable.Where(w => w.TaskDefine[nameof(WarningTaskDataModel.WarningStatus)] == status.ToString());

            if (!string.IsNullOrWhiteSpace(createdBy))
                querable = querable.Where(w => w.TaskDefine[nameof(WarningTaskDataModel.CreateBy)] == createdBy);

            var totalCount = await querable.LongCountAsync();

            //if (from > 0)
            //    querable = querable.Skip(from);

            //if (size.HasValue)
            //    querable = querable.Take(size.Value);
            
            var warningTasks = await querable.ToListAsync();

            var serviceModel = warningTasks.Select(w => new WarningTaskServiceModel(w.Id, w.TaskDefine)
            {
                Status = new JobTaskStatus()
                {
                    LastStatus = w.Status.LastStatus,
                    LastRunAt = w.Status.LastRunAt,
                    LastFinishedAt = w.Status.LastFinishedAt,
                    LastSucceedAt = w.Status.LastSucceedAt,
                    TryTimes = w.Status.TryTimes
                }
            });

            if (carModels?.Count > 0)
                serviceModel = serviceModel.Where(x => carModels.Any(c => x.CarModels == null || x.CarModels.Contains(c)));

            if (carTypes?.Count > 0)
                serviceModel = serviceModel.Where(x => carTypes.Any(c => x.CarTypes == null || x.CarTypes.Contains(c)));

            if (yearModels?.Count > 0)
                serviceModel = serviceModel.Where(x => yearModels.Any(c => x.YearModels == null || x.YearModels.Contains(c)));

            if (size.HasValue)
                return new Tuple<long, IEnumerable<WarningTaskServiceModel>>(totalCount, serviceModel.Skip(from).Take(size.Value));
            else
                return new Tuple<long, IEnumerable<WarningTaskServiceModel>>(totalCount, serviceModel);
        }

        public async Task UpdateAsync(string id, WarningTaskDataModel updateModel)
        {
            try
            {
                updateModel.LastModifyTime = DateTime.UtcNow;
                var update = Builders<JobTaskDataModel>.Update;
                var result = await context.JobTasks.UpdateOneAsync(t => t.JobId == ConstantSettings.WarningCalculationJobId && t.Id == id,
                    Builders<JobTaskDataModel>.Update.Set(t => t.TaskDefine, updateModel.ToBsonDocument()));
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task<bool> StopAsync(string id)
        {
            var update = Builders<JobTaskDataModel>.Update
                .Set(u => u.IsDeleted, true)
                .Set(u => u.TaskDefine[nameof(WarningTaskDataModel.WarningStatus)], WarningTaskStatus.已终止.ToString())
                .Set(u => u.TaskDefine[nameof(WarningTaskDataModel.LastModifyTime)], DateTime.UtcNow);
            try
            {
                var result = await context.JobTasks.UpdateOneAsync(t => t.JobId == ConstantSettings.WarningCalculationJobId && t.Id == id, update);

                return result.IsAcknowledged && result.ModifiedCount == 1;
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
            return false;
        }

        public async Task SaveAsync(WarningTaskDataModel createModel)
        {
            try
            {
                var now = DateTime.UtcNow;
                createModel.CreateTime = now;
                createModel.LastModifyTime = now;

                var dataModel = new JobTaskDataModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createModel.Name,
                    Description = createModel.Name,
                    IsDeleted = false,
                    IsRunOnce = false,
                    JobId = ConstantSettings.WarningCalculationJobId,
                    ManagedAt = now,
                    ManagedBy = createModel.CreateBy,
                    MerchantCreationTime = now,
                    Status = new JobTaskStatus { LastStatus = TaskExecutionResult.准备中, TryTimes = 0 },
                    TaskDefine = createModel.ToBsonDocument()
                };

                await context.JobTasks.InsertOneAsync(dataModel);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskCreateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await context.JobTasks.DeleteOneAsync(t => t.JobId == ConstantSettings.WarningCalculationJobId && t.Id == id);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskDeleteFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task<bool> IsExist(string taskId, string name)
        {
            var count = await context.JobTasks.AsQueryable()
                .LongCountAsync(x=>x.JobId == ConstantSettings.WarningCalculationJobId 
                && x.Id != taskId 
                && x.TaskDefine[nameof(WarningTaskDataModel.Name)] == name);
            return count > 0;
        }
    }
}
