using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class TaskCreateServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Name { get; set; }

        public string CreateBy { get; set; }

        public List<string> AnnotatorIds { get; set; }

        public List<string> DocumentIds { get; set; }

        public DateTime ExpectedDueAt { get; set; }

        public int Overlap { get; set; }

        public bool IsAutoApproved { get; set; }

        public bool IsAutoMerged { get; set; }

        public List<string> DictionaryIds { get; set; }

        public string TaskType { get; set; }

        public List<string> InspectorIds { get; set; }

        public List<string> AcceptorIds { get; set; }
    }
}
