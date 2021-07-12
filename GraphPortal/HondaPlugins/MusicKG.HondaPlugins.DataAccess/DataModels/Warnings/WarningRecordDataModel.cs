using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class WarningRecordDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string TaskId { get; set; }
        /// <summary>
        /// 零件号
        /// </summary>
        public string PartNo { get; set; }
        /// <summary>
        /// 零件名
        /// </summary>
        public string PartName { get; set; }
        /// <summary>
        /// 不良症状Id
        /// </summary>
        public string Syndrome { get; set; }

        [BsonIgnore]
        public BadGrade SyndromeBadGrade { get; set; }
        /// <summary>
        /// 预警时间
        /// </summary>
        public DateTime WarningTime { get; set; }

        public WarningConfirmRecordDataModel ConfirmRecord { get; set; }
        
        /// <summary>
        /// 多发预警
        /// </summary>
        public Dictionary<string, MultipleWarningIndexMetricsDataModel> MultipleMetrics { get; set; }
        /// <summary>
        /// 风险预警
        /// </summary>
        public RiskWarningIndexMetricsDataModel RiskMetrics { get; set; }
        /// <summary>
        /// 再发预警
        /// </summary>
        public AgainWarningIndexMetricsDataModel AgainMetrics { get; set; }
        /// <summary>
        /// 是否发生多发报警
        /// </summary>
        public bool IsMultipleWarning { get; set; }
        /// <summary>
        /// 是否发生风险报警
        /// </summary>
        public bool IsRiskWarning { get; set; }
        /// <summary>
        /// 是否发生再发报警
        /// </summary>
        public bool IsAgainWarning { get; set; }
        /// <summary>
        /// 是否符合预警频率
        /// </summary>
        public bool MatchedFrequency { get; set; }
    }
}
