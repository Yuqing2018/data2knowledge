using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Workspace type create binding model.
    /// </summary>
    public class WorkspaceTypeCreateBindingModel
    {
        /// <summary>
        /// Workspace type name.
        /// </summary>
        [Display(Name = "Workspace type name")]
        [Required]
        [StringLength(128, ErrorMessage = "Name cannot be longer than 128 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Workflow Id.
        /// </summary>
        [Display(Name = "Workflow Id")]
        [Required]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "The objectid is not in the correct format.")]
        public string WorkflowId { get; set; }

        /// <summary>
        /// Workspace type status.
        /// </summary>
        [Display(Name = "Workspace type status")]
        [Required]
        public WorkspaceTypeStatusEnum Status { get; set; }
    }
}
