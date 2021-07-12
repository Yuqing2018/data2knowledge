using MusicKG.Workflow.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ViewModels
{
    /// <summary>
    /// Workflow state view model.
    /// </summary>
    public class WorkflowStateViewModel
    {
        /// <summary>
        /// Workflow Id.
        /// </summary>
        [Display(Name = "Workflow Id")]
        public string WorkflowId { get; set; }

        /// <summary>
        /// Workflow name.
        /// </summary>
        [Display(Name = "Workflow name")]
        public string WorkflowName { get; set; }

        /// <summary>
        /// Workflow task status.
        /// </summary>
        [Display(Name = "Workflow state")]
        public WorkflowTaskStatusEnum Status { get; set; }

        /// <summary>
        /// Status of each step.
        /// </summary>
        [Display(Name = "Step status")]
        public IEnumerable<StepStateViewModel> StepStatus { get; set; }
    }
}
