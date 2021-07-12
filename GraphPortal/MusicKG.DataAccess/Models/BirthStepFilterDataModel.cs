using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class BirthStepFilterDataModel
    {
        public List<ObjectId> Steps { get; set; }

        public List<DocumentProcessStatusEnum> Status { get; set; }
    }
}
