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
    public class TaskAnnotatorDataModel
    {
        public ObjectId AnnotatorId { get; set; }

        public bool IsManager { get; set; }

        public IEnumerable<TaskDocumentDataModel> Documents { get; set; }
    }
}
