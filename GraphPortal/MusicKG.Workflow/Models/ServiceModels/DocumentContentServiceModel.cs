using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class DocumentContentServiceModel
    {
        public string Id { get; set; }

        public string WorkspaceId { get; set; }

        public string Name { get; set; }

        public List<string> Tags { get; set; }

        public string WorkflowId { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
