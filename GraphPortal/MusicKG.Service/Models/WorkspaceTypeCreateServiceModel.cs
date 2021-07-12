using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class WorkspaceTypeCreateServiceModel
    {
        public string Name { get; set; }

        public string WorkflowId { get; set; }

        public WorkspaceTypeStatusEnum Status { get; set; }
    }
}
