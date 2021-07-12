using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Task view model.
    /// </summary>
    public class TaskViewModel
    {
        /// <summary>
        /// Task ID.
        /// </summary>
        [Display(Name = "Task ID")]
        public string Id { get; set; }

        /// <summary>
        /// Task name.
        /// </summary>
        [Display(Name = "Task name")]
        public string Name { get; set; }

        /// <summary>
        /// Overlap.
        /// </summary>
        [Display(Name = "Overlap")]
        public int Overlap { get; set; }

        /// <summary>
        /// Expected due at.
        /// </summary>
        [Display(Name = "Expected due at")]
        public DateTime ExpectedDueAt { get; set; }

        /// <summary>
        /// Annotators.
        /// </summary>
        [Display(Name = "Annotators")]
        public IEnumerable<UserViewModel> Annotators { get; set; }

        /// <summary>
        /// Actual due at.
        /// </summary>
        [Display(Name = "Actual due at")]
        public DateTime? ActualDueAt { get; set; }

        /// <summary>
        /// Workspace.
        /// </summary>
        [Display(Name = "Workspace")]
        public WorkspaceViewModel Workspace { get; set; }

        /// <summary>
        /// Task Status.
        /// </summary>
        [Display(Name = "Task Status")]
        public TaskStatusEnum Status { get; set; }

        /// <summary>
        /// Is annotation result automatically approved.
        /// </summary>
        [Display(Name = "Is annotation result automatically approved")]
        public bool IsAutoApproved { get; set; }

        /// <summary>
        /// Is knowledge automatically merged.
        /// </summary>
        [Display(Name = "Is knowledge automatically merged")]
        public bool IsAutoMerged { get; set; }

        /// <summary>
        /// Created by.
        /// </summary>
        [Display(Name = "Created by")]
        public UserViewModel CreatedBy { get; set; }

        /// <summary>
        /// Dictionary id list.
        /// </summary>
        [Display(Name = "Dictionary id list")]
        public IEnumerable<string> DictionaryIds { get; set; }

        /// <summary>
        /// Task type.
        /// </summary>
        [Display(Name = "Task type")]
        public string TaskType { get; set; }

        /// <summary>
        /// Inspector list.
        /// </summary>
        [Display(Name = "Inspector user list")]
        public List<UserViewModel> Inspectors { get; set; }

        /// <summary>
        /// Acceptor list.
        /// </summary>
        [Display(Name = "Acceptor user list")]
        public List<UserViewModel> Acceptors { get; set; }
    }
}
