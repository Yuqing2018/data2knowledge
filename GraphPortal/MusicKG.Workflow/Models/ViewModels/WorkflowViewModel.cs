using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ViewModels
{
    /// <summary>
    /// Workflow view model.
    /// </summary>
    public class WorkflowViewModel
    {
        /// <summary>
        /// Workflow ID.
        /// </summary>
        [Display(Name = "Workspace ID")]
        public string Id { get; set; }

        /// <summary>
        /// Workflow name.
        /// </summary>
        [Display(Name = "Workspace name")]
        public string Name { get; set; }
    }
}
