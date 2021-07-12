using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Dictionary update binding model.
    /// </summary>
    public class DictionaryUpdateBindingModel
    {
        /// <summary>
        /// Dictionary name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.DictionaryName), ResourceType = typeof(Resources.DisplayNameResources))]
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
        /// Is dictionary name assigned value.
        /// </summary>
        [JsonIgnore]
        public bool IsNameAssigned { get; private set; }

        /// <summary>
        /// Ontology entity ID.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityId), ResourceType = typeof(Resources.DisplayNameResources))]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string EntityId
        {
            get => entityId;
            set
            {
                IsEntityIdAssigned = true;
                entityId = value;
            }
        }
        private string entityId;

        /// <summary>
        /// Is entity ID assigned value.
        /// </summary>
        [JsonIgnore]
        public bool IsEntityIdAssigned { get; private set; }

        /// <summary>
        /// Vocabularies.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Vocabularies), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> Vocabularies
        {
            get => vocabularies;
            set
            {
                IsVocabulariesAssigned = true;
                vocabularies = value;
            }
        }
        private List<string> vocabularies;

        /// <summary>
        /// Are vocabularies assigned value.
        /// </summary>
        [JsonIgnore]
        public bool IsVocabulariesAssigned { get; private set; }
    }
}
