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
    /// Dictionary create binding model.
    /// </summary>
    public class DictionaryCreateBindingModel
    {
        /// <summary>
        /// Dictionary name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.DictionaryName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Ontology entity ID.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityId), ResourceType = typeof(Resources.DisplayNameResources))]
        public string EntityId { get; set; }
        
        /// <summary>
        /// Vocabularies.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Vocabularies), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [MinLengthLocalized(1)]
        public List<string> Vocabularies { get; set; }
    }
}
