using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class DocumentProcessStepDataModel
    {
        public ObjectId StepId { get; set; }

        public DocumentProcessStatusEnum Status { get; set; }

        public int Times { get; set; }

        public List<DocumentProcessHistoryDataModel> Histories { get; set; }
    }
}
