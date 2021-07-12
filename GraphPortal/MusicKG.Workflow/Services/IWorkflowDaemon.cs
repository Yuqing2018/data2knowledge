using MusicKG.Workflow.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services
{
    public interface IWorkflowDaemon
    {
        Task Run();

        DaemonStateMonitorServiceModel CurrentState { get; }
    }
}
