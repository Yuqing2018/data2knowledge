using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IWarningTaskService
    {
        Task<WarningTaskServiceModel> GetAsync(string id);

        Task<List<WarningTaskIdAndNameModel>> GetTaskIdAndNameList();

        Task<Tuple<long, IEnumerable<WarningTaskServiceModel>>> GetWarningTasksAsync(
            WarningUnit? warningType, WarningTaskStatus? status, string createdBy,
            List<string> carModels, List<string> carTypes, List<string> yearModels, 
            int from, int? size, List<string> taskIds = null);

        Task UpdateAsync(string id, WarningTaskDataModel updateModel);

        Task<bool> StopAsync(string id);

        Task SaveAsync(WarningTaskDataModel createModel);

        Task DeleteAsync(string id);

        Task<bool> IsExist(string taskId, string name);
    }
}
