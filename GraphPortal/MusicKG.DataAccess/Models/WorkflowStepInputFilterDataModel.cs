using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class WorkflowStepInputFilterDataModel
    {
        public List<DocumentStatusEnum> Status { get; set; }

        public BirthStepFilterDataModel BirthStep { get; set; }

        public DeathStepDataModel DeathStep { get; set; }
    }
}
