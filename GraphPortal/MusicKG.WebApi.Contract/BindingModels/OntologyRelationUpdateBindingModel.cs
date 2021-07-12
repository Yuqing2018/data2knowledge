using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Ontology relation update binding model.
    /// </summary>
    public class OntologyRelationUpdateBindingModel
    {
        /// <summary>
        /// Ontology relation name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyRelationName), ResourceType = typeof(Resources.DisplayNameResources))]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name
        {
            get => name;
            set
            {
                IsNameAssigned = true;
                name = value;
            }
        }
        private string name;

        /// <summary>
        /// Is name assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsNameAssigned { get; private set; }

        /// <summary>
        /// Ontology entity properties.
        /// </summary>
        public List<OntologyPropertyBindingModel> Properties
        {
            get => properties;
            set
            {
                IsPropertiesAssigned = true;
                properties = value;
            }
        }
        private List<OntologyPropertyBindingModel> properties;
        /// <summary>
        /// Is Properties updated.
        /// </summary>
        [JsonIgnore]
        public bool IsPropertiesAssigned { get; set; }

        /// <summary>
        /// Ontology relation second entity ID.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.SecondEntityId), ResourceType = typeof(Resources.DisplayNameResources))]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string SecondEntityId
        {
            get => secondEntityId;
            set
            {
                IsSecondEntityAssigned = true;
                secondEntityId = value;
            }
        }
        private string secondEntityId;

        /// <summary>
        /// Is second entity assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsSecondEntityAssigned { get; private set; }

        /// <summary>
        /// Ontology entity description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyRelationDescription), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Description
        {
            get => description;
            set
            {
                IsDescriptionAssigned = true;
                description = value;
            }
        }
        private string description;

        /// <summary>
        /// Is description assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsDescriptionAssigned { get; private set; }
    }
}
