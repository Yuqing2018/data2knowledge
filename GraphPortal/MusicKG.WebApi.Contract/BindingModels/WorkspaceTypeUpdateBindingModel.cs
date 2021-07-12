using MusicKG.DataAccess.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Workspace type update binding model.
    /// </summary>
    public class WorkspaceTypeUpdateBindingModel
    {
        /// <summary>
        /// Workspace type name.
        /// </summary>
        [Display(Name = "Workspace type name")]
        [StringLength(128, ErrorMessage = "Name cannot be longer than 128 characters.")]
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
        /// Workspace type status.
        /// </summary>
        [Display(Name = "Workspace type status")]
        public WorkspaceTypeStatusEnum Status
        {
            get => status;
            set
            {
                IsStatusAssigned = true;
                status = value;
            }
        }
        private WorkspaceTypeStatusEnum status;

        /// <summary>
        /// Is name assigned a value.
        /// </summary>
        [JsonIgnore]
        public bool IsStatusAssigned { get; private set; }
    }
}
