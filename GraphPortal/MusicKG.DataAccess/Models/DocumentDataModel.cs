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
    public class DocumentDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public string Name { get; set; }

        public DocumentStatusEnum Status { get; set; }

        public string ContentType { get; set; }

        public List<string> Tags { get; set; }

        public string ContentMd5 { get; set; }

        public ObjectId? ParentId { get; set; }

        public ObjectId WorkflowId { get; set; }

        public DocumentProcessStepDataModel BirthStep { get; set; }

        public DocumentProcessStepDataModel NextStep { get; set; }
        
        public ObjectId UploadedBy { get; set; }

        public DateTime UploadedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long ItemCount { get; set; }
    }
}
