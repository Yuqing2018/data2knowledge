using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.Services.Models.WarningTasks
{
    public class RiskRecordExportModel
    {
        public string TaskId { get; set; }

        public string PartNo { get; set; }

        public string PartName { get; set; }

        public string SyndromeName { get; set; }

        public BadGrade BadGrade { get; set; }

        public DateTime? WarningTime { get; set; }

        public List<WarningConfirmRecordDataModel> ConfirmRecord { get; set; }

        public RiskWarningIndexMetricsDataModel RiskMetrics { get; set; }
    }

    public class RiskRecordMetricsExportModel
    {
        /// <summary>
        /// 预警任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 车款
        /// </summary>
        public List<string> CarModels { get; set; }
        /// <summary>
        /// 车型，根据车款多选
        /// </summary>
        public List<string> CarTypes { get; set; }
        /// <summary>
        /// 年款：根据车款车型多选
        /// </summary>
        public List<string> YearModels { get; set; }
        /// <summary>
        /// 零件号
        /// </summary>
        public string PartNo { get; set; }
        /// <summary>
        /// 零件名
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 不良症状
        /// </summary>
        public string Syndrome { get; set; }

        /// <summary>
        /// 不良等级
        /// </summary>
        public BadGrade BadGrade { get; set; }

        public WarningConfirmRecordDataModel ConfirmRecord { get; set; }

        /// <summary>
        /// 风险计算数据
        /// </summary>
        public RiskWarningIndexMetricsDataModel RiskMetrics { get; set; }

        /// <summary>
        /// 预警时间
        /// </summary>
        public DateTime? WarningTime { get; set; }
    }
}
