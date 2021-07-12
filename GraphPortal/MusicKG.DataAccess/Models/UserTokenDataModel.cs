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
    public class UserTokenDataModel
    {
        public string Token { get; set; }

        public DateTime ExpiredAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
