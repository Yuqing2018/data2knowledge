using MongoDB.Bson;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IWarningRecordService
    {
        /// <summary>
        /// 获取报警详情列表
        /// </summary>
        /// <param name="isHandled"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<Tuple<long, IEnumerable<WarningRecordServiceModel>>> GetPageListAsync(ProcessStatus? isHandled, string detailId, int from, int? size);

        /// <summary>
        /// 获取任务所属的详情列表
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <returns></returns>
        Task<List<WarningRecordDataModel>> GetTaskWarningRecords(string taskId);

        /// <summary>
        /// 单独保存预警记录- 离线计算使用
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(WarningRecordDataModel createModel);

        /// <summary>
        /// 提交确认记录
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="confirm"></param>
        /// <returns></returns>
        Task<bool> UpdateConfirmRecord(string recordId, WarningConfirmRecordDataModel confirm);
        /// <summary>
        /// 更新AI风险等级与是否为训练数据
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="riskLevel"></param>
        /// <param name="usedForModel"></param>
        /// <returns></returns>
        Task<bool> UpdateAIRiskLevel(string recordId, RiskLevel? riskLevel, bool usedForModel);
        /// <summary>
        /// 风险等级
        /// </summary>
        /// <param name="search"></param>
        /// <param name="warningUnit"></param>
        /// <returns></returns>
        Task<(RiskLevel?, RiskLevel?)> GetRiskLevel(BaseSearchModel search, WarningUnit warningUnit);

        /// <summary>
        /// 确认记录展示列表
        /// </summary>
        /// <returns></returns>
        Task<List<WarningTaskConfirmRecordServiceModel>> ListConfirmRecordAsync(BaseSearchModel search);

        Task<WarningConfirmRecordDataModel> GetLastConfirmRecordAsync(string taskId, string partName, string syndrome);

        /// <summary>
        /// 确认记录导出
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<List<string>>> ExportAsync(BaseSearchModel search);

        /// <summary>
        /// 邮件消息列表
        /// </summary>
        /// <returns></returns>
        Task<List<WarningMessageServiceModel>> MessagesToSend();

        Task<(List<ObjectId> recordIds, List<ObjectId> detailIds)> SaveWarningRecordsAsync(string taskId, List<WarningRecordDataModel> records);

        Task<List<bool>> UpdatePendingCountAndTotalCount();

        Task RemoveWarningRecords(List<ObjectId> recordIds);

        Task RemoveWarningDetails(List<ObjectId> recordIds);
    }
}
