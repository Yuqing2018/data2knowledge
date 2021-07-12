using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class AutoTaskCreationRulesDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public List<AutoTaskRuleDataModel> Rules { get; set; }

        public ObjectId CreatedUser { get; set; }

        public bool OnlyCreateWhanMatchDocumentCount { get; set; } = false;

        public AutoTaskCreationRulesDataModel()
        {
            Rules = new List<AutoTaskRuleDataModel>
            {
                new AutoTaskRuleDataModel()
            };
            CreatedUser = ObjectId.Empty;
        }
    }

    [BsonIgnoreExtraElements]
    public class AutoTaskRuleDataModel
    {
        public string Name { get; set; } = "Default";

        public List<string> DocumentTags { get; set; }

        public List<ObjectId> Annotators { get; set; }

        public int DocumentCount { get; set; } = 1;

        public int Overlap { get; set; } = 0;

        public bool IsAutoApproved { get; set; } = true;

        public bool IsAutoMerged { get; set; } = true;

        public int MaxFinishDays { get; set; } = 365;
    }
}
