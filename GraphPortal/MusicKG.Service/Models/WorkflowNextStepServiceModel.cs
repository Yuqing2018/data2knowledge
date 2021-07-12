using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class WorkflowNextStepServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AssemblyName { get; set; }

        public string ClassName { get; set; }

        public DocumentStatusEnum ResultDocumentStatus { get; set; }

        public string WorkflowId { get; set; }
        
        public bool AutoDoNext { get; set; }
    }
}
