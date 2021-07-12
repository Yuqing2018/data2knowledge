using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Ontology entity list item view model.
    /// </summary>
    public class OntologyEntityListItemViewModel
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
        /// Ontology entity mark color.
        /// </summary>
        [Display(Name = "Ontology entity mark color")]
        public string Color { get; set; }
    }
}
