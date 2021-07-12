using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Ontology entity update binding model.
    /// </summary>
    public class OntologyEntityUpdateBindingModel
    {
        /// <summary>
        /// Ontology entity name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityName), ResourceType = typeof(Resources.DisplayNameResources))]
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
        public List<OntologyPropertyBindingModel> Properties {
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
        /// Ontology entity description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityDescription), ResourceType = typeof(Resources.DisplayNameResources))]
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

        /// <summary>
        /// Ontology entity mark color.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityMarkColor), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Color
        {
            get => color;
            set
            {
                IsColorAssigned = true;
                color = value;
            }
        }
        private string color;

        /// <summary>
        /// Is Color assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsColorAssigned { get; private set; }
    }
}
