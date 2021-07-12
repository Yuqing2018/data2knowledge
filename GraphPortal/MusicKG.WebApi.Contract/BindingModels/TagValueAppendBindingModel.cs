using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Tag value append binding model.
    /// </summary>
    public class TagValueAppendBindingModel
    {
        /// <summary>
        /// Tag value.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TagValue), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Value { get; set; }

        /// <summary>
        /// Tag color.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TagColor), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Color { get; set; }

        /// <summary>
        /// Tag description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TagDescription), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Description { get; set; }
    }
}
