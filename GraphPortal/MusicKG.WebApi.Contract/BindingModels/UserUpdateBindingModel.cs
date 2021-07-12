using MusicKG.DataAccess.Enums;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.Constants;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// User update binding model.
    /// </summary>
    public class UserUpdateBindingModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.UserName), ResourceType = typeof(Resources.DisplayNameResources))]
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
        /// User password.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Password), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Password
        {
            get => password;
            set
            {
                IsPasswordAssigned = true;
                password = value;
            }
        }
        private string password;

        /// <summary>
        /// Is password assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsPasswordAssigned { get; private set; }

        /// <summary>
        /// User roles.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.UserRoles), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<UserRoleEnum> Roles
        {
            get => roles;
            set
            {
                IsRolesAssigned = true;
                roles = value;
            }
        }
        private List<UserRoleEnum> roles;

        /// <summary>
        /// Are roles assigned values.
        /// </summary>
        [JsonIgnore]
        public bool IsRolesAssigned { get; private set; }

        /// <summary>
        /// User status.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.UserStatus), ResourceType = typeof(Resources.DisplayNameResources))]
        public UserStatusEnum? Status { get; set; }
    }
}
