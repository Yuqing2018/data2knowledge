using MusicKG.HondaPlugins.Services.Models.ReannotationTasks;
using MusicKG.Scheduler.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IDataReannotationTaskService
    {
        Task<List<DataReannotationTaskServiceModel>> ListAsync(string keyword, string createdBy,
            DateTime? createdAfter, DateTime? succeedBefore,
            DateTime? succeedAfter, DateTime? createdBefore,
            DateTime? lastRunAfter, DateTime? lastRunBefore,
            TaskExecutionResult? lastRunStatus);

        Task<DataReannotationTaskServiceModel> GetAsync(string id);

        Task CreateAsync(DataReannotationTaskManageServiceModel serviceModel);

        Task UpdateAsync(string id, DataReannotationTaskManageServiceModel serviceModel);

        Task DeleteAsync(string id);
    }
}
