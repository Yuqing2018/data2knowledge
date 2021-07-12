using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Task list item view model.
    /// </summary>
    public class  TaskListItemViewModel
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
        /// Task type.
        /// </summary>
        [Display(Name = "Task type")]
        public string TaskType { get; set; }

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
        /// Created By.
        /// </summary>
        [Display(Name="Created by")]
        public UserViewModel CreatedBy { get; set; }
    }
}
