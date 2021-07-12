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
    /// Task update binding model.
    /// </summary>
    public class TaskUpdateBindingModel
    {
        /// <summary>
        /// Task name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TaskName), ResourceType = typeof(Resources.DisplayNameResources))]
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
        /// Expected due at.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.ExpectedDueAt), ResourceType = typeof(Resources.DisplayNameResources))]
        public DateTime? ExpectedDueAt { get; set; }
    }
}
