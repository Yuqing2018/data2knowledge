using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class WorkspaceTypeDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public ObjectId WorkflowId { get; set; }

        public WorkspaceTypeStatusEnum Status { get; set; }

        public List<TaskTypeDataModel> SupportTaskTypes { get; set; }
    }
}
