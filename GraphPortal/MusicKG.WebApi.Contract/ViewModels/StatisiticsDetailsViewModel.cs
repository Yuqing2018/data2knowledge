using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Statisitics Details ViewModel.
    /// </summary>
    public class StatisiticsDetailsViewModel
    {
        /// <summary>
        /// User.
        /// </summary>
        [Display(Name = "User")]
        public UserViewModel User { get; set; }

        /// <summary>
        /// Statisitics details
        /// </summary>
        [Display(Name = "Statisitics details")]
        public IEnumerable<TaskStatisiticsDetailsViewModel> StatisiticsDetails { get; set; }
    }

    /// <summary>
    /// Task Statisitics Details ViewModel.
    /// </summary>
    public class TaskStatisiticsDetailsViewModel
    {
        /// <summary>
        /// Task Id.
        /// </summary>
        [Display(Name = "Task Id")]
        public string TaskId { get; set; }

        /// <summary>
        /// Task name.
        /// </summary>
        [Display(Name = "Task name")]
        public string TaskName { get; set; }

        /// <summary>
        /// Task created at.
        /// </summary>
        [Display(Name = "Task created at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Task expected due at.
        /// </summary>
        [Display(Name = "Task expected due at")]
        public DateTime ExpectedDueAt { get; set; }

        /// <summary>
        /// Task annotators.
        /// </summary>
        [Display(Name = "Task annotators")]
        public IEnumerable<TaskAnnotatorViewModel> Annotators { get; set; }

        /// <summary>
        /// Workspace.
        /// </summary>
        [Display(Name = "Workspace")]
        public WorkspaceViewModel Workspace { get; set; }

        /// <summary>
        /// Finished document count.
        /// </summary>
        [Display(Name = "Finished document count")]
        public long FinishedDocumentCount { get; set; }
    }

    /// <summary>
    /// Task Annotator ViewModel
    /// </summary>
    public class TaskAnnotatorViewModel
    {
        /// <summary>
        /// Annotator Id.
        /// </summary>
        [Display(Name = "Annotator Id")]
        public string AnnotatorId { get; set; }

        /// <summary>
        /// Annotator name.
        /// </summary>
        [Display(Name = "Annotator name")]
        public string AnnotatorName { get; set; }

        /// <summary>
        /// Is manager.
        /// </summary>
        [Display(Name = "Is manager")]
        public bool IsManager { get; set; }
    }
}
