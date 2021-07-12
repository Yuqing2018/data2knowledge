using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Workflow.Enums;
using MusicKG.Workflow.Filters;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Models.ViewModels;
using MusicKG.Workflow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Controllers
{
    /// <summary>
    /// Workflow controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class DaemonController : ControllerBase
    {
        private IWorkflowDaemon workflowDaemon;

        /// <summary>
        /// Workflow controller constructor.
        /// </summary>
        /// <param name="workflowDaemon">Workflow daemon.</param>
        public DaemonController(IWorkflowDaemon workflowDaemon)
        {
            this.workflowDaemon = workflowDaemon;
        }

        /// <summary>
        /// Get daemon state.
        /// </summary>
        /// <returns>Workflow state.</returns>
        [HttpGet]
        [Route("State")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DaemonStateViewModel), (int)HttpStatusCode.OK)]
        public DaemonStateViewModel GetWorkflowStatus()
        {
            return this.DaemonServiceModelToViewModel(this.workflowDaemon.CurrentState);
        }

        private DaemonStateViewModel DaemonServiceModelToViewModel(DaemonStateMonitorServiceModel currentState)
        {
            if (currentState == null)
            {
                return new DaemonStateViewModel
                {
                    Status = WorkflowTaskStatusEnum.Stopped
                };
            }
            else
            {
                return new DaemonStateViewModel
                {
                    Status = currentState.Status,
                    WorkflowStatus = currentState.WorkflowsStatus.Select(x => new WorkflowStateViewModel
                    {
                        Status = x.Value.Status,
                        WorkflowId = x.Key,
                        WorkflowName = x.Value.WorkflowName,
                        StepStatus = x.Value.StepStatus.Select(y => new StepStateViewModel
                        {
                            StepId = y.Key,
                            StepName = y.Value.StepName,
                            Action = y.Value.Action,
                            Status = y.Value.Status
                        })
                    })
                };
            }
        }
    }
}
