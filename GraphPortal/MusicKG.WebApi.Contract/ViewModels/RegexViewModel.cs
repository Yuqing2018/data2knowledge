using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Regex view model.
    /// </summary>
    public class RegexViewModel
    {
        /// <summary>
        /// Regex ID.
        /// </summary>
        [Display(Name = "Regex ID")]
        public string Id { get; set; }

        /// <summary>
        /// Regular expression.
        /// </summary>
        [Display(Name = "Regular expression")]
        public string Regex { get; set; }
    }
}
