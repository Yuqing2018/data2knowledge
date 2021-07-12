using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    public class OntologyPropertyViewModel
    {
        /// <summary>
        /// Ontology entity property name.
        /// </summary>
        [Display(Name = "Ontology property name")]
        public string Name { get; set; }

        /// <summary>
        /// Ontology entity property type Id.
        /// </summary>
        [Display(Name = "Ontology property type")]
        public string Type { get; set; }

        /// <summary>
        /// Ontology entity property description.
        /// </summary>
        [Display(Name = "Ontology property description")]
        public string Description { get; set; }
    }
}
