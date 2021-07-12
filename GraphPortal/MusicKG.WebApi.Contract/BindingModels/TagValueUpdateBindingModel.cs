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
    /// Tag value update binding model.
    /// </summary>
    public class TagValueUpdateBindingModel
    {
        /// <summary>
        /// Tag value.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TagValue), ResourceType = typeof(Resources.DisplayNameResources))]
        public string Value 
        {
            get => value;
            set
            {
                IsValueAssigned = true;
                this.value = value;
            }
        }
        private string value;

        /// <summary>
        /// Is tag value assigned value.
        /// </summary>
        [JsonIgnore]
        public bool IsValueAssigned { get; private set; }

        /// <summary>
        /// Tag color.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TagColor), ResourceType = typeof(Resources.DisplayNameResources))]
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
        /// Is tag color assigned value.
        /// </summary>
        [JsonIgnore]
        public bool IsColorAssigned { get; private set; }

        /// <summary>
        /// Tag description.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TagDescription), ResourceType = typeof(Resources.DisplayNameResources))]
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
        /// Is tag description assigned value.
        /// </summary>
        [JsonIgnore]
        public bool IsDescriptionAssigned { get; private set; }
    }
}
