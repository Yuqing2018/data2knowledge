using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Enums;
using System;

namespace MusicKG.HondaPlugins.Services.Models.WarningTasks
{
    public class WarningTaskDetailServiceModel
    {
        public string Id { get; set; }

        public WarningTaskServiceModel WarningTask { get; set; }

        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Syndrome { get; set; }
        public SyndromeServiceModel SyndromeModel { get; set; }

        public WarningFrequency Frequency { get; set; }
        /// <summary>
        /// 当Frequency = 指定日期时，该字段为具体日期
        /// </summary>
        public DateTime? SpecifiedDate { get; set; }

        /// <summary>
        /// 关注状态
        /// </summary>
        public FocusType FocusType { get; set; }

        ///待处理条数
        public int PendingCount { get; set; }

        ///总预警条数
        public int TotalCount { get; set; }

        public WarningTaskDetailSettingDataModel Settings { get; set; }

        public DateTime LastFrequencySetDate { get; set; }

        public DateTime? FocusedDate { get; set; }

        public DateTime? CancelFocusedDate { get; set; }
    }
}
