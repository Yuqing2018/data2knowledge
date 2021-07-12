using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Workspace update binding model.
    /// </summary>
    public class WorkspaceUpdateBindingModel
    {
        /// <summary>
        /// Workspace name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.WorkspaceName), ResourceType = typeof(Resources.DisplayNameResources))]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name
        {
            get => name;
            set
            {
                IsNameAssigned = true;
                name = value;
            }
        }
        private string name;

        /// <summary>
        /// Is name assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsNameAssigned { get; private set; }

        /// <summary>
        /// If automatically merge annotation result.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.IsAutoMerging), ResourceType = typeof(Resources.DisplayNameResources))]
        public bool? IsAutoMerging { get; set; }

        /// <summary>
        /// Workspace description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.WorkspaceDescription), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Description
        {
            get => description;
            set
            {
                IsDescriptionAssigned = true;
                description = value;
            }
        }
        private string description;

        /// <summary>
        /// Is description assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsDescriptionAssigned { get; private set; }

        /// <summary>
        /// Workspace readOnlyUser id list.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.ReadOnlyUserIds), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> ReadOnlyUserIds { get; set; }
    }
}
