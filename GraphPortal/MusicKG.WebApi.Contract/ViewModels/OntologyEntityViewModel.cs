using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Ontology entity view model.
    /// </summary>
    public class OntologyEntityViewModel
    {
        /// <summary>
        /// Ontology entity ID.
        /// </summary>
        [Display(Name = "Ontology entity ID")]
        public string Id { get; set; }

        /// <summary>
        /// Ontology entity name.
        /// </summary>
        [Display(Name = "Ontology entity name")]
        public string Name { get; set; }

        /// <summary>
        /// Ontology entity properties.
        /// </summary>
        [Display(Name = "Ontology entity properties")]
        public List<OntologyPropertyViewModel> Properties { get; set; }

        /// <summary>
        /// Ontology entity description.
        /// </summary>
        [Display(Name = "Ontology entity description")]
        public string Description { get; set; }

        /// <summary>
        /// Ontology entity mark color.
        /// </summary>
        [Display(Name = "Ontology entity mark color")]
        public string Color { get; set; }
    }
}
