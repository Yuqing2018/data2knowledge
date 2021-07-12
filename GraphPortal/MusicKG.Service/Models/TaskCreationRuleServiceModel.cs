using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class TaskCreationRuleServiceModel
    {
        public string WorkspaceId { get; set; }

        public bool OnlyCreateWhanMatchDocumentCount { get; set; } = false;

        public List<AutoTaskRuleServiceModel> Rules { get; set; }
    }

    public class AutoTaskRuleServiceModel
    {
        public string Name { get; set; }

        public List<string> DocumentTags { get; set; }

        public List<string> Annotators { get; set; }

        public int Overlap { get; set; } = 0;

        public bool IsAutoApproved { get; set; } = true;

        public bool IsAutoMerged { get; set; } = true;

        public int MaxFinishDays { get; set; } = 365;

        public int DocumentCount { get; set; } = 1;
    }
}
