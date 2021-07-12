using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Ontology entity property view model.
    /// </summary>
    public class OntologyPropertyBindingModel
    {
        /// <summary>
        /// Ontology entity property name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyPropertyName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Ontology entity property type Id.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyPropertyType), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string Type { get; set; }

        /// <summary>
        /// Ontology entity property description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyPropertyDescription), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Description { get; set; }
    }
}
