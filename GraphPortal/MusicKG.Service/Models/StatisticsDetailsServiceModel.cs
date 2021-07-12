using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class StatisticsDetailsServiceModel
    {
        public UserServiceModel User { get; set; }

        public IEnumerable<TaskStatisticsDetailsServiceModel> TaskStatisticsDetails { get;set;}
    }

    public class TaskStatisticsDetailsServiceModel
    {
        public string TaskId { get; set; }

        public string TaskName { get; set; }

        public WorkspaceServiceModel Workspace { get; set; }

        public DateTime ExpectedDueAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<TaskAnnotatorServiceModel> RelatedUsers { get; set; }

        public long FinishedDocumentCount { get; set; }
    }
}
