using MusicKG.Workflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class StepStateMonitorServiceModel : StateMonitorServiceModel
    {
        private string stepName;
        public string StepName
        {
            get
            {
                return this.stepName;
            }
            set
            {
                lock (this.locker)
                {
                    this.stepName = value;
                }
            }
        }

        private WorkflowStepActionEnum action;
        public WorkflowStepActionEnum Action
        {
            get
            {
                return this.action;
            }
            set
            {
                lock (this.locker)
                {
                    this.action = value;
                }
            }
        }

        public StepStateMonitorServiceModel() : base()
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Step Current Status Overview");
            builder.AppendLine($"Step Name: {this.StepName}");
            builder.AppendLine($"Step Status: {this.Status}");
            builder.AppendLine($"Current Step Action: {this.Action})");

            return builder.ToString();
        }
    }
}
