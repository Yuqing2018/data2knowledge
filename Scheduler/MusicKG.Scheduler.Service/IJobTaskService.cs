using MusicKG.Scheduler.DataAccess.Enums;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Service.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Service
{
    public interface IJobTaskService
    {
        Task<bool> ExistAsync(string id);

        Task<bool> IsTimeSpanOverlap(string jobId, DateTime from, DateTime to);

        Task<List<JobTaskServiceModel>> ListPreparedAsync(string jobId, int maxRerunTimes);

        Task<List<JobTaskServiceModel>> ListAsync(string jobId,
            string keyword, TaskExecutionResult? status, string createdBy, 
            bool? isRunOnce, DateTime? succeedBefore, DateTime? succeedAfter,
            DateTime? createdBefore, DateTime? createdAfter, 
            DateTime? runBefore, DateTime? runAfter, bool ignoreDeleted = true);

        Task<JobTaskServiceModel> GetAsync(string jobId, string id);

        Task<JobTaskStatus> GetStatusAsync(string jobId, string id);

        Task<JObject> GetDetailsAsync(string jobId, string id);

        Task<List<JobTaskHistoryServiceModel>> GetHistoryAsync(string jobId, string id);

        Task CreateAsync(string jobId, JobTaskManagementServiceModel jobTask);

        Task UpdateAsync(string jobId, string id, JobTaskManagementServiceModel jobTask);

        Task DeleteAsync(string jobId, string id);

        Task AppendHistoryAsync(string id, JobTaskExecuteResultServiceModel runStatus);

        Task UpdateStatusAsync(string id, TaskExecutionResult status);
    }
}
