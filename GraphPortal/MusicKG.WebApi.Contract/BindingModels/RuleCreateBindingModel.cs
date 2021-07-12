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
    /// Rule create binding model.
    /// </summary>
    public class RuleCreateBindingModel
    {
        /// <summary>
        /// Rule name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.RuleName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Ontology entity ID.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.OntologyEntityId), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string EntityId { get; set; }

        /// <summary>
        /// Regular expressions.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.RegularExpressions), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [MinLengthLocalized(1)]
        public List<string> Regexes { get; set; }
    }
}
