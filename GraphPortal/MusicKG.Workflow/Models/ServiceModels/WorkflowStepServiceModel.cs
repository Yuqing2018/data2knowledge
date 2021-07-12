using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class WorkflowStepServiceModel
    {
        public string Id { get; set; }

        public string WorkflowId { get; set; }

        public string Name { get; set; }

        public string ProcessorAssembly { get; set; }

        public string ProcessorClass { get; set; }

        public WorkflowStepInputFilterDataModel InputFilter { get; set; }

        public DocumentStatusEnum ResultDocumentStatus { get; set; }
    }
}
