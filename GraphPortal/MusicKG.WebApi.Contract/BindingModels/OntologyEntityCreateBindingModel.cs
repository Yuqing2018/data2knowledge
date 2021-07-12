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
    /// Ontology entity create binding model.
    /// </summary>
    public class OntologyEntityCreateBindingModel
    {
        /// <summary>
        /// Ontology entity name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Ontology entity description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityDescription), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Description { get; set; }

        /// <summary>
        /// Ontology entity mark color.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityMarkColor), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Color { get; set; }
    }
}
