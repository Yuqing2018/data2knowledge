using MusicKG.HondaPlugins.DataAccess.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    [BsonIgnoreExtraElements]
    public class WarningTaskDataModel
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 预警单元
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public WarningUnit WarningUnit { get; set; }
        /// <summary>
        /// 预警状态
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public WarningTaskStatus WarningStatus { get; set; }
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
        /// 预警指标名称，预警值
        /// </summary>
        public List<WarningIndexDataModel> WarningIndex { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
    }
}
