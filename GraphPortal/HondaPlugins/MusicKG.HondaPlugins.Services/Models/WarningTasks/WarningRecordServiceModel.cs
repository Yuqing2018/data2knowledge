using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.Services.Models.WarningTasks
{
    public class WarningRecordServiceModel : WarningRecordDataModel
    {
        public WarningRecordServiceModel(WarningRecordDataModel data)
        {
            Id = data.Id;
            TaskId = data.TaskId;
            PartNo = data.PartNo;
            PartName = data.PartName;
            Syndrome = data.Syndrome;
            WarningTime = data.WarningTime;
            ConfirmRecord = data.ConfirmRecord;
            MultipleMetrics = data.MultipleMetrics;
            RiskMetrics = data.RiskMetrics;
            AgainMetrics = data.AgainMetrics;
            IsMultipleWarning = data.IsMultipleWarning;
            IsRiskWarning = data.IsRiskWarning;
            IsAgainWarning = data.IsAgainWarning;
        }

        public WarningTaskServiceModel WarningTask { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessStatus IsHandled
        {
            get
            {
                return ConfirmRecord == null ? ProcessStatus.待处理 : ProcessStatus.已处理;
            }
        }
        /// <summary>
        /// 不良症状名称
        /// </summary>
        public SyndromeServiceModel SyndromeModel { get; set; }
    }
}
