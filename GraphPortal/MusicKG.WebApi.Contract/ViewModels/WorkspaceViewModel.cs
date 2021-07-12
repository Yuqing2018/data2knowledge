using MusicKG.DataAccess.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Workspace view model.
    /// </summary>
    public class WorkspaceViewModel
    {
        /// <summary>
        /// Workspace ID.
        /// </summary>
        [Display(Name = "Workspace ID")]
        public string Id { get; set; }

        /// <summary>
        /// Workspace name.
        /// </summary>
        [Display(Name = "Workspace name")]
        public string Name { get; set; }

        /// <summary>
        /// Workspace type.
        /// </summary>
        [Display(Name = "Workspace type")]
        public WorkspaceTypeViewModel Type { get; set; }

        /// <summary>
        /// Workspace language.
        /// </summary>
        [Display(Name = "Workspace language")]
        public LanguageEnum Language { get; set; }

        /// <summary>
        /// If automatically merge annotation result.
        /// </summary>
        [Display(Name = "If automatically merge annotation result")]
        public bool IsAutoMerging { get; set; }

        /// <summary>
        /// Workspace description.
        /// </summary>
        [Display(Name = "Workspace description")]
        public string Description { get; set; }

        /// <summary>
        /// The creator of this workspace.
        /// </summary>
        [Display(Name = "Workspace Creator")]
        public UserViewModel CreateBy { get; set; }

        /// <summary>
        /// Workspace readOnlyUser list.
        /// </summary>
        [Display(Name = "Workspace ReadOnlyUser list")]
        public List<UserViewModel> ReadOnlyUsers { get; set; }
    }
}
