using MusicKG.Workflow.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class WorkflowStateMonitorServiceModel : StateMonitorServiceModel
    {
        private string workflowName;
        public string WorkflowName
        {
            get
            {
                return this.workflowName;
            }
            set
            {
                lock (this.locker)
                {
                    this.workflowName = value;
                }
            }
        }

        public ConcurrentDictionary<string, StepStateMonitorServiceModel> StepStatus { get; }

        public WorkflowStateMonitorServiceModel() : base()
        {
            this.StepStatus = new ConcurrentDictionary<string, StepStateMonitorServiceModel>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Workflow Current Status Overview");
            builder.AppendLine($"Workflow Name: {this.WorkflowName}");
            builder.AppendLine($"Workflow Status: {this.Status}");
            builder.AppendLine($"Current Running Steps (Count: {this.StepStatus.Count})");

            foreach (var stepId in this.StepStatus.Keys)
            {
                builder.AppendLine("------------------------------------");
                builder.AppendLine($"Step: {stepId}");
                builder.Append(this.StepStatus[stepId].ToString());
            }

            return builder.ToString();
        }
    }
}
