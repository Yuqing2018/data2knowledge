using MusicKG.Service.Models;
using MusicKG.Workflow.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services
{
    public interface IWorkflowStepProcessService
    {
        Task Start(WorkflowStepServiceModel step);

        event Action<string, string, StepStateMonitorServiceModel> WorkflowStepStateUpdated;
    }
}
