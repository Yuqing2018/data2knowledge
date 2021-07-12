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
    /// Rule update binding model.
    /// </summary>
    public class RuleUpdateBindingModel
    {
        /// <summary>
        /// Rule name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.RuleName), ResourceType = typeof(Resources.DisplayNameResources))]
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
        /// Is rule name assigned value.
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
        /// Regular expressions.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.RegularExpressions), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> Regexes
        {
            get => regexes;
            set
            {
                IsRegexesAssigned = true;
                regexes = value;
            }
        }
        private List<string> regexes;

        /// <summary>
        /// Are regexes assigned value.
        /// </summary>
        [JsonIgnore]
        public bool IsRegexesAssigned { get; private set; }
    }
}
