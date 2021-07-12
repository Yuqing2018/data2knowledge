using System;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Category view model.
    /// </summary>
    public class CategoryViewModel
    {
        /// <summary>
        /// Workspace ID.
        /// </summary>
        [Display(Name = "Workspace ID")]
        public string WorkspaceId { get; set; }
        /// <summary>
        /// ID.
        /// </summary>
        [Display(Name = "Category ID")]
        public string Id { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [Display(Name = "Category name")]
        public string Name { get; set; }

        /// <summary>
        /// Category created user.
        /// </summary>
        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Category created time.
        /// </summary>
        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }
    }
}
