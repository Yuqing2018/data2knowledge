using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// User view model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// User ID.
        /// </summary>
        [Display(Name = "User ID")]
        public string Id { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        [Display(Name = "User name")]
        public string Name { get; set; }

        /// <summary>
        /// User roles.
        /// </summary>
        [Display(Name = "User roles")]
        public IEnumerable<UserRoleEnum> Roles { get; set; }

        /// <summary>
        /// User status.
        /// </summary>
        [Display(Name = "User status")]
        public UserStatusEnum Status { get; set; }

        /// <summary>
        /// User created by.
        /// </summary>
        [Display(Name = "User created by")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// User created at.
        /// </summary>
        [Display(Name = "User created at")]
        public DateTime CreatedAt { get; set; }
    }
}
