using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class WorkflowProcessorServiceModel
    {
        public string WorkflowId { get; set; }

        public string WorkflowName { get; set; }

        public List<WorkflowStepServiceModel> AvailableSteps { get; set; }
    }
}
