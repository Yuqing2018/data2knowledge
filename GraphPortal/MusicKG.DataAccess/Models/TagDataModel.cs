using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    public class TagDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public TagTypeEnum Type { get; set; }

        public List<TagValueDataModel> Values { get; set; }
    }

    public class TagValueDataModel
    {
        public ObjectId Id { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }
    }
}
