using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IWarningTaskDetailService
    {
        Task<Tuple<long, IEnumerable<WarningTaskDetailServiceModel>>> GetPageListAsync(List<string> taskIds, WarningUnit? warningUnit, string createdBy, List<string> carModels, List<string> carTypes, List<string> yearModels, List<string> partNames, List<string> syndromes, FocusType? focusType, int from, int? size);
        /// <summary>
        /// 导出预警跟踪记录
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="warningUnit"></param>
        /// <param name="createdBy"></param>
        /// <param name="carModels"></param>
        /// <param name="carTypes"></param>
        /// <param name="yearModels"></param>
        /// <param name="partNames"></param>
        /// <param name="syndromes"></param>
        /// <param name="focusType"></param>
        /// <returns></returns>
        Task<List<List<string>>> ExportAsync(List<string> taskIds, WarningUnit? warningUnit, string createdBy, List<string> carModels, List<string> carTypes, List<string> yearModels, List<string> partNames, List<string> syndromes, FocusType? focusType);
        /// <summary>
        /// 导出风险台账
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="warningUnit"></param>
        /// <param name="createdBy"></param>
        /// <param name="carModels"></param>
        /// <param name="carTypes"></param>
        /// <param name="yearModels"></param>
        /// <param name="partNames"></param>
        /// <param name="syndromes"></param>
        /// <param name="focusType"></param>
        /// <returns></returns>
        Task<List<RiskRecordMetricsExportModel>> ExportRiskRecordListAsync(List<string> taskIds, WarningUnit? warningUnit, string createdBy, List<string> carModels, List<string> carTypes, List<string> yearModels, List<string> partNames, List<string> syndromes, FocusType? focusType);

        Task<bool> UpdateAsync(string id, WarningFrequency frequency, bool isFocused, DateTime? date);

        Task<bool> UpdateSettingsAsync(string id, WarningTaskDetailSettingDataModel settings);

        Task<bool> UpdatePendingAndTotalCount(string taskId, string partName, string syndrome, int? pendingCount, int? totalCount);

        Task<WarningTaskDetailDataModel> GetTaskDetail(string taskId, string partName, string syndrome);
        Task<WarningTaskDetailServiceModel> GetAsync(string id);

        Task<int> UnhandledCountAsync(string createdBy);
    }
}
