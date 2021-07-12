using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class WorkflowServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<WorkflowStepServiceModel> Steps { get; set; }
    }
}
