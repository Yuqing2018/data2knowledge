using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Entity match binding model.
    /// </summary>
    public class EntityMatchBindingModel
    {
        /// <summary>
        /// SPOs to match.
        /// </summary>
        [Display(Name = "SPOs to match")]
        [Required]
        [MinLength(1)]
        public List<SPOBindingModel> SPOs { get; set; }
    }
}
