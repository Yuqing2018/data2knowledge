using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Models.Overall
{
    [BsonIgnoreExtraElements]
    public class FocusedProjectMetricsModel
    {
        /// <summary>
        /// 关注日期
        /// </summary>
        public DateTime? FocusedDate { get; set; }

        public DateTime? CancelFocusedDate { get; set; }
        /// <summary>
        /// 车款
        /// </summary>
        public List<string> CarModels { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public List<string> YearModels { get; set; }
        public string PartName { get; set; }
        public string Syndrome { get; set; }
        public string SyndromeName { get; set; }
        /// <summary>
        /// 计算风险等级
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public RiskLevel? RiskLevel { get; set; }
        /// <summary>
        /// AI风险等级
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public RiskLevel? AIRiskLevel { get; set; }
        /// <summary>
        /// 对策状态
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public PermanentCntrStatus? CntrStatus { get; set; }
    }
}
