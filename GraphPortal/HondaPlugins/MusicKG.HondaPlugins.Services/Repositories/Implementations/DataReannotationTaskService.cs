using Microsoft.Extensions.Logging;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Models.ReannotationTasks;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.Scheduler.DataAccess;
using MusicKG.Scheduler.DataAccess.Enums;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.Service.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class DataReannotationTaskService : IDataReannotationTaskService
    {
        private readonly IJobTaskService jobTaskService;
        private readonly ILogger<DataReannotationTaskService> logger;

        public DataReannotationTaskService(IJobTaskService jobTaskService, ILogger<DataReannotationTaskService> logger)
        {
            this.jobTaskService = jobTaskService;
            this.logger = logger;
        }

        public async Task CreateAsync(DataReannotationTaskManageServiceModel serviceModel)
        {
            if (await jobTaskService.IsTimeSpanOverlap(ConstantSettings.DataReannotationJobId, serviceModel.From, serviceModel.To))
            {
                logger.LogError("The reannotation task fault time span is overlap!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.ReannotationTaskFaultTimeSpanOverlap, HttpStatusCode.BadRequest);
            }

            await jobTaskService.CreateAsync(ConstantSettings.DataReannotationJobId, new JobTaskManagementServiceModel
            {
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                IsRunOnce = true,
                ManagedBy = serviceModel.ManagedBy,
                StartTime = serviceModel.From,
                EndTime = serviceModel.To,
                MerchantCreationTime = DateTime.UtcNow,
                TaskDefine = JObject.FromObject(new object())
            });
        }

        public async Task DeleteAsync(string id)
        {
            await jobTaskService.DeleteAsync(ConstantSettings.DataReannotationJobId, id);
        }

        public async Task<DataReannotationTaskServiceModel> GetAsync(string id)
        {
            var serviceModel = await jobTaskService.GetAsync(ConstantSettings.DataReannotationJobId, id);

            return new DataReannotationTaskServiceModel
            {
                Id = serviceModel.Id,
                From = serviceModel.StartTime ?? DateTime.MinValue,
                To = serviceModel.EndTime ?? DateTime.MaxValue,
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                Status = serviceModel.LastStatus,
                CreatedAt = serviceModel.ManagedAt,
                CreatedBy = serviceModel.ManagedBy,
                SucceedAt = serviceModel.LastSucceedAt
            };
        }

        public async Task<List<DataReannotationTaskServiceModel>> ListAsync(string keyword, string createdBy, DateTime? createdAfter, DateTime? succeedBefore, DateTime? succeedAfter, DateTime? createdBefore, DateTime? lastRunAfter, DateTime? lastRunBefore, TaskExecutionResult? lastRunStatus)
        {
            var serviceModel = await jobTaskService.ListAsync(ConstantSettings.DataReannotationJobId, keyword, lastRunStatus, createdBy, true, succeedBefore, succeedAfter, createdBefore, createdAfter, lastRunBefore, lastRunAfter, true);

            return serviceModel?.Select(task => new DataReannotationTaskServiceModel
            {
                Id = task.Id,
                From = task.StartTime ?? DateTime.MinValue,
                To = task.EndTime ?? DateTime.MaxValue,
                Name = task.Name,
                Description = task.Description,
                Status = task.LastStatus,
                CreatedAt = task.ManagedAt,
                CreatedBy = task.ManagedBy,
                SucceedAt = task.LastStatus == TaskExecutionResult.准备中 ? null : task.LastSucceedAt
            })?.ToList();
        }

        public async Task UpdateAsync(string id, DataReannotationTaskManageServiceModel serviceModel)
        {
            if (await jobTaskService.IsTimeSpanOverlap(ConstantSettings.DataReannotationJobId, serviceModel.From, serviceModel.To))
            {
                logger.LogError("The reannotation task fault time span is overlap!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.ReannotationTaskFaultTimeSpanOverlap, HttpStatusCode.BadRequest);
            }

            await jobTaskService.UpdateAsync(ConstantSettings.DataReannotationJobId, id, new JobTaskManagementServiceModel
            {
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                IsRunOnce = true,
                ManagedBy = serviceModel.ManagedBy,
                StartTime = serviceModel.From,
                EndTime = serviceModel.To,
                MerchantCreationTime = DateTime.UtcNow,
                TaskDefine = JObject.FromObject(new object())
            });
        }
    }
}
