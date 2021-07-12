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
    public class TaskDocumentResultDataModel
    {
        public ObjectId ResultDocumentId { get; set; }

        public TaskDocumentResultTypeEnum ResultType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
