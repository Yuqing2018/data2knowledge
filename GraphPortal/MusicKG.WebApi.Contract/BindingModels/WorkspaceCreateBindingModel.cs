using MusicKG.DataAccess.Enums;
using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Workspace create binding model.
    /// </summary>
    public class WorkspaceCreateBindingModel
    {
        /// <summary>
        /// Workspace name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.WorkspaceName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Workspace type Id.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.WorkspaceTypeId), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string Type { get; set; }

        /// <summary>
        /// Workspace language.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Language), ResourceType = typeof(Resources.DisplayNameResources))]
        public LanguageEnum Language { get; set; }

        /// <summary>
        /// If automatically merge annotation result.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.IsAutoMerging), ResourceType = typeof(Resources.DisplayNameResources))]
        public bool IsAutoMerging { get; set; }

        /// <summary>
        /// Workspace description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.WorkspaceDescription), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Description { get; set; }

        /// <summary>
        /// ReadOnly user list.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.ReadOnlyUserIds), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> ReadOnlyUserIds { get; set; }
    }
}
