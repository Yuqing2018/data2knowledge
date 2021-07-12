using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Ontology relation list item view model.
    /// </summary>
    public class OntologyRelationListItemViewModel
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

        [Display(Name = "Ontology relation first entity Name")]
        public string FirstEntityName{ get; set; }

        /// <summary>
        /// Ontology relation second entity ID.
        /// </summary>
        [Display(Name = "Ontology relation second entity ID")]
        public string SecondEntityId { get; set; }

        [Display(Name = "Ontology relation second entity Name")]
        public string SecondEntityName { get; set; }

        /// <summary>
        /// Ontology relation description.
        /// </summary>
        [Display(Name = "Ontology relation description")]
        public string Description { get; set; }
    }
}
