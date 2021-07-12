using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    public class SPOBindingModel
    {
        /// <summary>
        /// Subject.
        /// </summary>
        [Display(Name = "Subject")]
        [Required]
        public string Subject { get; set; }

        /// <summary>
        /// Predicate.
        /// </summary>
        [Display(Name = "Predicate")]
        [Required]
        public string Predicate { get; set; }

        /// <summary>
        /// Predicate.
        /// </summary>
        [Display(Name = "Predicate")]
        [Required]
        public string Object { get; set; }
    }
}
