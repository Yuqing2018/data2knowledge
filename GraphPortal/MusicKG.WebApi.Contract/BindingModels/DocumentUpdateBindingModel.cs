using MusicKG.DataAccess.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Document update binding model.
    /// </summary>
    public class DocumentUpdateBindingModel
    {
        /// <summary>
        /// Document name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.DocumentName), ResourceType = typeof(Resources.DisplayNameResources))]
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
        /// Document status.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.DocumentStatus), ResourceType = typeof(Resources.DisplayNameResources))]
        public DocumentStatusEnum Status
        {
            get => status;
            set
            {
                IsStatusAssigned = true;
                status = value;
            }
        }
        private DocumentStatusEnum status;

        /// <summary>
        /// Is status assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsStatusAssigned { get; private set; }

        /// <summary>
        /// Document tags.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.DocumentTags), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> Tags
        {
            get => tags;
            set
            {
                IsTagsAssigned = true;
                tags = value;
            }
        }
        private List<string> tags;

        /// <summary>
        /// Is tags assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsTagsAssigned { get; private set; }
    }
}
