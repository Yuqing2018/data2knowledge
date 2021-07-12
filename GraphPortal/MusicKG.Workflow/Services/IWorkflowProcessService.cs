using MusicKG.Workflow.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services
{
    public interface IWorkflowProcessService
    {
        Task Start(WorkflowProcessorServiceModel workflow);

        event Action<string, WorkflowStateMonitorServiceModel> WorkflowStateUpdated;
    }
}
