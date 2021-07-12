using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Ontology relation view model.
    /// </summary>
    public class OntologyRelationViewModel
    {
        /// <summary>
        /// Ontology relation ID.
        /// </summary>
        [Display(Name = "Ontology relation ID")]
        public string Id { get; set; }
        /// <summary>
        /// Ontology relation name.
        /// </summary>
        [Display(Name = "Ontology relation name")]
        public string Name { get; set; }

        /// <summary>
        /// Ontology relation first entity ID.
        /// </summary>
        [Display(Name = "Ontology relation first entity ID")]
        public string FirstEntityId { get; set; }

        /// <summary>
        /// Ontology relation second entity ID.
        /// </summary>
        [Display(Name = "Ontology relation second entity ID")]
        public string SecondEntityId { get; set; }

        /// <summary>
        /// Ontology relation properties.
        /// </summary>
        [Display(Name = "Ontology relation properties")]
        public List<OntologyPropertyViewModel> Properties { get; set; }

        /// <summary>
        /// Ontology relation description.
        /// </summary>
        [Display(Name = "Ontology relation description")]
        public string Description { get; set; }
    }
}
