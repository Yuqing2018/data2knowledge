using MusicKG.Workflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public abstract class StateMonitorServiceModel
    {
        protected readonly object locker;

        protected WorkflowTaskStatusEnum status;
        public virtual WorkflowTaskStatusEnum Status
        {
            get
            {
                return status;
            }
            set
            {
                lock (this.locker)
                {
                    this.status = value;
                }
            }
        }

        public StateMonitorServiceModel()
        {
            this.locker = new object();
            this.status = WorkflowTaskStatusEnum.Stopped;
        }
    }
}
