using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.Scheduler.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.HondaPlugins.Services.Models.WarningTasks
{
    public class WarningTaskServiceModel
    {
        public WarningTaskServiceModel() { }
        public WarningTaskServiceModel(string id, BsonDocument document)
        {
            var dataModel = BsonSerializer.Deserialize<WarningTaskDataModel>(document);

            Id = id;
            Name = dataModel.Name;
            CarModels = dataModel.CarModels;
            CarTypes = dataModel.CarTypes;
            WarningIndex = dataModel.WarningIndex?.Select(x=>new WarningIndexServiceModel(x)).ToList();
            WarningStatus = dataModel.WarningStatus;
            WarningUnit = dataModel.WarningUnit;
            YearModels = dataModel.YearModels;
            CreateBy = dataModel.CreateBy;
            CreateTime = dataModel.CreateTime;
        }
        public string Id { get; set; }
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
        /// 预警类别
        /// </summary>
        public string WarningType
        {
            get
            {
                return string.Join(',', WarningIndex?.Select(x => x.WarningType));
            }
        }

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
        /// 预警指标
        /// </summary>
        public List<WarningIndexServiceModel> WarningIndex { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        public JobTaskStatus Status { get; set; }
    }
}
