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
    public class OptionDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public OptionTypeEnum Type { get; set; }

        public string Value { get; set; }

        public string DisplayName { get; set; }
    }
}
