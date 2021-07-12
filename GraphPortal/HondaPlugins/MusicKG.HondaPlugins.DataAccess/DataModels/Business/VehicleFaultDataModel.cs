using MusicKG.HondaPlugins.DataAccess.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Business
{
    [BsonIgnoreExtraElements]
    public class VehicleFaultDataModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DataSource DataSource { get; set; }

        /// <summary>
        /// 源数据Id
        /// </summary>
        public string RawId { get; set; }

        /// <summary>
        /// 车款
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarModel { get; set; }

        /// <summary>
        /// 年款
        /// </summary>
        public string ModelYear { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string FrameNo { get; set; }

        /// <summary>
        /// 里程数
        /// 总局数据无此字段
        /// </summary>
        public int MileAge { get; set; } = -1;

        /// <summary>
        /// 生产日期
        /// 如没有根据车架号从销售表中取
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// 销售日期
        /// 如没有根据车架号从销售表中取
        /// </summary>
        public DateTime InitialRegistDate { get; set; }

        /// <summary>
        /// 故障日期
        /// </summary>
        public DateTime FaultDate { get; set; }

        /// <summary>
        /// 特约店
        /// </summary>
        public string DealerCD { get; set; }

        /// <summary>
        /// 特约店名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 总维修费用
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float CostRepair { get; set; } = 0f;

        /// <summary>
        /// 零件号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PartNo { get; set; }

        /// <summary>
        /// 故障零件名
        /// 标注字段
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 不良症状
        /// 标注字段
        /// </summary>
        public string Syndrome { get; set; }

        /// <summary>
        /// QIS关联信息
        /// </summary>
        public List<VehicleFaultRelatedDataModel> RelatedInfo { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public Dictionary<string, string> Features { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? SyncTimestamp { get; set; }

        /// <summary>
        /// 关联零件信息（手动）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RelatedPart RelatedPartName { get; set; }

        /// <summary>
        /// 数据来源（MQI自动关联QIS为系统匹配，其他人工录入）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DataFrom { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DataFromDesc { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastModifiedAt { get; set; }
    }
}
