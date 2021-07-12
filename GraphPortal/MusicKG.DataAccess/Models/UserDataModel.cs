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
    public class UserDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public UserStatusEnum Status { get; set; }

        public string Salt { get; set; }

        public string Password { get; set; }

        public List<UserRoleEnum> Roles { get; set; }

        public List<UserTokenDataModel> Tokens { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
