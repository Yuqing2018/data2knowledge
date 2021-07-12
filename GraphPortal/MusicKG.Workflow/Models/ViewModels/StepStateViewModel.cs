using MusicKG.Workflow.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ViewModels
{
    /// <summary>
    /// Step state view model.
    /// </summary>
    public class StepStateViewModel
    {
        /// <summary>
        /// Step Id.
        /// </summary>
        [Display(Name = "Step Id")]
        public string StepId { get; set; }

        /// <summary>
        /// Step name.
        /// </summary>
        [Display(Name = "Step name")]
        public string StepName { get; set; }

        /// <summary>
        /// Step task status.
        /// </summary>
        [Display(Name = "Step state")]
        public WorkflowTaskStatusEnum Status { get; set; }

        /// <summary>
        /// Step action.
        /// </summary>
        [Display(Name = "Step action")]
        public WorkflowStepActionEnum Action { get; set; }
    }
}
