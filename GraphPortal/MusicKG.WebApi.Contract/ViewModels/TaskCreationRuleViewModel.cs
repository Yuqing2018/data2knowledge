using System.Collections.Generic;

namespace MusicKG.WebApi.Contract.ViewModels
{
    public class TaskCreationRuleViewModel
    {
        public bool OnlyCreateWhanMatchDocumentCount { get; set; } = false;

        public List<AutoTaskRuleViewModel> Rules { get; set; }
    }

    public class AutoTaskRuleViewModel
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
