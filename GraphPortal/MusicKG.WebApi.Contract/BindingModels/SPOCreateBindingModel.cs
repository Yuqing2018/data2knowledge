using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// SPO create binding model.
    /// </summary>
    public class SPOCreateBindingModel
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
        /// Object.
        /// </summary>
        [Display(Name = "Object")]
        [Required]
        public string Object { get; set; }
    }
}
