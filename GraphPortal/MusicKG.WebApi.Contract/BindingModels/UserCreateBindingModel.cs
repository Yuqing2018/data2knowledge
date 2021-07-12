using MusicKG.DataAccess.Enums;
using MusicKG.WebApi.Contract.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// User create binding model.
    /// </summary>
    public class UserCreateBindingModel
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

        /// <summary>
        /// User roles.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.UserRoles), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [MinLengthLocalized(1)]
        public List<UserRoleEnum> Roles { get; set; }
    }
}
