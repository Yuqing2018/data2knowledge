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
    public class TaskDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public int Overlap { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ActualDueAt { get; set; }

        public DateTime ExpectedDueAt { get; set; }

        public List<TaskAnnotatorDataModel> Annotators { get; set; }

        public TaskIAAScoreDataModel IAAScore { get; set; }

        public TaskStatusEnum Status { get; set; }

        public ObjectId CreatedBy { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool IsAutoApproved { get; set; }

        public bool IsAutoMerged { get; set; }

        public List<ObjectId> DictionaryIds { get; set; }

        public ObjectId? TaskType { get; set; }

        public List<ObjectId> InspectorIds { get; set; }

        public List<ObjectId> AcceptorIds { get; set; }
    }
}
