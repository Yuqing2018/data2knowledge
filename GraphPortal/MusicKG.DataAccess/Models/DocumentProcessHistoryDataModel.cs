using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class DocumentProcessHistoryDataModel
    {
        public DateTime StartedAt { get; set; }

        public DateTime FinishedAt { get; set; }

        public DocumentProcessStatusEnum Status { get; set; }

        public string Message { get; set; }
    }
}
