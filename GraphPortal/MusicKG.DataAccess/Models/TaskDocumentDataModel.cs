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
    public class TaskDocumentDataModel
    {
        public ObjectId DocumentId { get; set; }

        public TaskDocumentStatusEnum Status { get; set; }

        public IEnumerable<TaskDocumentResultDataModel> Results { get; set; }

        public DateTime AnnotatedAt { get; set; }
    }
}
