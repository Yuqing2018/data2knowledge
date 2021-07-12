using MusicKG.Workflow.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ViewModels
{
    /// <summary>
    /// Daemon state view model.
    /// </summary>
    public class DaemonStateViewModel
    {
        /// <summary>
        /// Daemon task status.
        /// </summary>
        [Display(Name = "Daemon state")]
        public WorkflowTaskStatusEnum Status { get; set; }

        /// <summary>
        /// Status of each workflow.
        /// </summary>
        [Display(Name = "Workflow status")]
        public IEnumerable<WorkflowStateViewModel> WorkflowStatus { get; set; }
    }
}
