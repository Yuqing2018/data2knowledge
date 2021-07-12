using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class WorkflowStepInputFilterServiceModel
    {
        public string WorkflowId { get; set; }

        public List<DocumentStatusEnum> Status { get; set; }

        public BirthStepFilterDataModel BirthStep { get; set; }

        public DeathStepDataModel DeathStep { get; set; }
    }
}
