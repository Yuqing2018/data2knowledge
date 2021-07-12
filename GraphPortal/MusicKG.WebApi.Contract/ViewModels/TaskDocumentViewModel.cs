using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Task document view model.
    /// </summary>
    public class TaskDocumentViewModel
    {
        /// <summary>
        /// Task document ID.
        /// </summary>
        [Display(Name = "Task document ID")]
        public string Id { get; set; }

        /// <summary>
        /// Document name.
        /// </summary>
        [Display(Name = "Document name")]
        public string Name { get; set; }

        /// <summary>
        /// Task document status.
        /// </summary>
        [Display(Name = "Task document status")]
        public TaskDocumentStatusEnum Status { get; set; }

        /// <summary>
        /// Uploaded at.
        /// </summary>
        [Display(Name = "Uploaded at")]
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Latest result saved at.
        /// </summary>
        [Display(Name = "Latest result saved at")]
        public DateTime LatestResultSavedAt { get; set; }

        /// <summary>
        /// Annotators.
        /// </summary>
        [Display(Name = "Annotators")]
        public IEnumerable<UserViewModel> Annotators { get; set; }
    }
}
