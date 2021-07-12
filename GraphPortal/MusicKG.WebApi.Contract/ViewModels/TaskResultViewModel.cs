using MusicKG.DataAccess.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Task document result view model.
    /// </summary>
    public class TaskResultViewModel
    {
        /// <summary>
        /// Document ID.
        /// </summary>
        [Display(Name = "Document ID")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Task document status.
        /// </summary>
        [Display(Name = "Task document status")]
        public TaskDocumentStatusEnum TaskDocumentStatus { get; set; }

        /// <summary>
        /// Task document result.
        /// </summary>
        [Display(Name = "Task document result document ID")]
        public string ResultDocumentId { get; set; }

        /// <summary>
        /// Task document result type.
        /// </summary>
        [Display(Name = "Task document result type")]
        public TaskDocumentResultTypeEnum ResultType { get; set; }

        /// <summary>
        /// Annotated at.
        /// </summary>
        [Display(Name = "Annotated at")]
        public DateTime AnnotatedAt { get; set; }

        /// <summary>
        /// Annotated by.
        /// </summary>
        [Display(Name = "Annotated by")]
        public UserViewModel AnnotatedBy { get; set; }
    }
}
