using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class WarningTaskDetailDataModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public string TaskId { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Syndrome { get; set; }
        [BsonRepresentation(BsonType.String)]
        public WarningFrequency Frequency { get; set; }

        public DateTime? SpecifiedDate { get; set; }

        public int PendingCount { get; set; }

        public int TotalCount { get; set; }
        /// <summary>
        /// 是否重点关注
        /// </summary>
        public bool IsFocused { get; set; }

        public WarningTaskDetailSettingDataModel Settings { get; set; }

        public DateTime LastFrequencySetDate { get; set; }

        public DateTime? FocusedDate { get; set; }

        public DateTime? CancelFocusedDate { get; set; }
    }
}
