using MusicKG.WebApi.Contract.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// User login binding model.
    /// </summary>
    public class UserLoginBindingModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.UserName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(128)]
        public string Name { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Password), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        public string Password { get; set; }
    }
}
