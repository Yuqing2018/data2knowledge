using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class WarningConfirmRecordDataModel
    {
        /// <summary>
        /// 确认人
        /// </summary>
        public string LastConfirmdUser { get; set; }
        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime LastConfirmdTime { get; set; }
        /// <summary>
        /// 确认内容
        /// </summary>
        public string ConfirmedMessage { get; set; }
        /// <summary>
        /// 推进状态
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public PushStatus? PushStatus { get; set; }
        /// <summary>
        /// 对策状态
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public PermanentCntrStatus? PermanentCntrStatus { get; set; }
        /// <summary>
        /// 是否超标
        /// </summary>
        public bool? IsExcessive { get; set; }
    }
}
