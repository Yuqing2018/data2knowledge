using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Ontology relation create binding model.
    /// </summary>
    public class OntologyRelationCreateBindingModel
    {
        /// <summary>
        /// Relation name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyRelationName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// First entity id.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.FirstEntityId), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string FirstEntityId { get; set; }

        /// <summary>
        /// Second entity id.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.SecondEntityId), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string SecondEntityId { get; set; }

        /// <summary>
        /// Relation description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyRelationDescription), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Description { get; set; }
    }
}
