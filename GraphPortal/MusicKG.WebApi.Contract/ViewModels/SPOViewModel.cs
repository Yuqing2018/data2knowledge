using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// SPO view model.
    /// </summary>
    public class SPOViewModel
    {
        /// <summary>
        /// SPO ID.
        /// </summary>
        [Display(Name = "SPO ID")]
        public string Id { get; set; }

        /// <summary>
        /// Subject.
        /// </summary>
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Predicate.
        /// </summary>
        [Display(Name = "Predicate")]
        public string Predicate { get; set; }

        /// <summary>
        /// Object.
        /// </summary>
        [Display(Name = "Object")]
        public string Object { get; set; }
    }
}
