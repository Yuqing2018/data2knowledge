using System;
using System.Collections.Generic;
using System.Text;
using MusicKG.DataAccess.Enums;

namespace MusicKG.Service.Models
{
    public class TaskServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Overlap { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ActualDueAt { get; set; }

        public DateTime ExpectedDueAt { get; set; }

        public IEnumerable<TaskAnnotatorServiceModel> Annotators { get; set; }

        public TaskStatusEnum Status { get; set; }

        public WorkspaceServiceModel Workspace { get; set; }

        public UserServiceModel CreatedBy { get; set; }

        public bool IsAutoApproved { get; set; }

        public bool IsAutoMerged { get; set; }

        public IEnumerable<string> DictionaryIds { get; set; }

        public string TaskType { get; set; }

        public IEnumerable<UserServiceModel> Inspectors { get; set; }

        public IEnumerable<UserServiceModel> Acceptors { get; set; }
    }
}
