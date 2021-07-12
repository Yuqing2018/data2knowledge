using MusicKG.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Option view model.
    /// </summary>
    public class OptionViewModel
    {
        /// <summary>
        /// Option type.
        /// </summary>
        [Display(Name = "Option type")]
        public OptionTypeEnum Type { get; set; }

        /// <summary>
        /// Option Value.
        /// </summary>
        [Display(Name = "Option value")]
        public string Value { get; set; }

        /// <summary>
        /// Option name to be displayed.
        /// </summary>
        [Display(Name = "Option display name")]
        public string DisplayName { get; set; }
    }
}
