using System;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// User login view model.
    /// </summary>
    public class UserLoginViewModel : UserViewModel
    {
        /// <summary>
        /// Token.
        /// </summary>
        [Display(Name = "Token")]
        public string Token { get; set; }

        /// <summary>
        /// Token expired at.
        /// </summary>
        [Display(Name = "Token expired at")]
        public DateTime TokenExpiredAt { get; set; }
    }
}
