using MusicKG.HondaPlugins.DataAccess.Enums;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    [BsonIgnoreExtraElements]
    public class RiskWarningIndexMetricsDataModel
    {
        #region 各渠道特征
        public Dictionary<string, DataSourceMetricsDataModel> DataSourceMetrics { get; set; }
        #endregion

        #region 推进情况
        ///推进分类
        [BsonRepresentation(BsonType.String)]
        public PushStatus? PushStatus { get; set; }
        /// <summary>
        /// 不良原因
        /// </summary>
        public string CntrMesrReasonDesc { get; set; }
        /// <summary>
        /// 对策内容
        /// </summary>
        public string PermanentCntr { get; set; }
        /// <summary>
        /// 对策时间
        /// </summary>
        public DateTime? PermanentCntrTime { get; set; }
        #endregion

        #region 风险特征
        /// <summary>
        /// 保修金额
        /// </summary>
        public double MeanCostRepair { get; set; }
        /// <summary>
        /// 超标影响
        /// </summary>
        public Boolean? IsExcessive { get; set; }
        /// <summary>
        /// 对策状态
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public PermanentCntrStatus? PermanentCntrStatus { get; set; }
        #endregion

        /// <summary>
        /// 风险得分
        /// </summary>
        public double RiskScore { get; set; }

        /// <summary>
        /// 风险等级（计算）
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public RiskLevel? RiskLevel { get; set; }

        /// <summary>
        /// AI风险等级
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public RiskLevel? AIRiskLevel { get; set; }
        /// <summary>
        /// 是否为训练数据
        /// </summary>
        public bool UsedForModel { get; set; }
    }

    public class DataSourceMetricsDataModel
    {
        /// <summary>
        /// 件数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 不良率
        /// </summary>
        public double DefectRate { get; set; }
        /// <summary>
        /// 近三月上升率
        /// </summary>
        public double LastThreeMonthAscentRate { get; set; }
    }
}
