using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Document view model.
    /// </summary>
    public class DocumentViewModel
    {
        /// <summary>
        /// Workspace ID.
        /// </summary>
        [Display(Name = "Workspace ID")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// Document ID.
        /// </summary>
        [Display(Name = "Document ID")]
        public string Id { get; set; }

        /// <summary>
        /// Document name.
        /// </summary>
        [Display(Name = "Document name")]
        public string Name { get; set; }

        /// <summary>
        /// Document status.
        /// </summary>
        [Display(Name = "Document status")]
        public DocumentStatusEnum Status { get; set; }

        /// <summary>
        /// Document tags.
        /// </summary>
        [Display(Name = "Document tags")]
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Uploaded at.
        /// </summary>
        [Display(Name = "Uploaded at")]
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Uploaded by.
        /// </summary>
        [Display(Name = "Uploaded by")]
        public UserViewModel UploadedBy { get; set; }
    }
}
