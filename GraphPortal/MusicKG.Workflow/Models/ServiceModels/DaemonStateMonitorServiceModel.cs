using MusicKG.Workflow.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class DaemonStateMonitorServiceModel : StateMonitorServiceModel
    {
        public ConcurrentDictionary<string, WorkflowStateMonitorServiceModel> WorkflowsStatus { get; }

        public DaemonStateMonitorServiceModel() : base()
        {
            this.WorkflowsStatus = new ConcurrentDictionary<string, WorkflowStateMonitorServiceModel>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Daemon Current Status Overview");
            builder.AppendLine($"Daemon Status: {this.Status}");
            builder.AppendLine($"Current Running Workflows (Count: {this.WorkflowsStatus.Count})");
            builder.AppendLine($"Current Running Steps (Count: {this.WorkflowsStatus.Values.Sum(x => x.StepStatus.Count)})");
            foreach (var workflowId in this.WorkflowsStatus.Keys)
            {
                builder.AppendLine("------------------------------------");
                builder.AppendLine($"Workflow: {workflowId}");
                builder.Append(this.WorkflowsStatus[workflowId].ToString());
            }

            return builder.ToString();
        }
    }
}
