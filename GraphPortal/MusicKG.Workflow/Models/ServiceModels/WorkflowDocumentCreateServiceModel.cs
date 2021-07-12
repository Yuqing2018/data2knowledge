using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class WorkflowDocumentCreateServiceModel
    {
        public string ParentId { get; set; }

        public string ParentName { get; set; }

        public string ParentWorkspaceId { get; set; }

        public string ParentWorkflowId { get; set; }

        public List<string> ParentTags { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public string BirthStep { get; set; }

        public DocumentStatusEnum Status { get; set; }
    }
}
