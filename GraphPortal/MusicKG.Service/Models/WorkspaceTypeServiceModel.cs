using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class WorkspaceTypeServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WorkflowId { get; set; }

        public string WorkflowName { get; set; }
    }
}
