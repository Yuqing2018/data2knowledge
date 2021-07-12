using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Workspace type view model.
    /// </summary>
    public class WorkspaceTypeViewModel
    {
        /// <summary>
        /// Workspace type Id.
        /// </summary>
        [Display(Name = "Workspace type Id")]
        public string Id { get; set; }

        /// <summary>
        /// Workspace type name.
        /// </summary>
        [Display(Name = "Workspace type name")]
        public string Name { get; set; }

        /// <summary>
        /// Workflow name.
        /// </summary>
        [Display(Name =  "Workflow name")]
        public string WorkflowName { get; set; }
    }
}
