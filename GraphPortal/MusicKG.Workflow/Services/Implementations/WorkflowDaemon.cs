using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicKG.Workflow.Enums;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services.Implementations
{
    /// <summary>
    /// Workflow Daemon
    /// </summary>
    public class WorkflowDaemon : IWorkflowDaemon
    {
        private readonly SupportedWorkflowSettings settings;
        private readonly IWorkflowService workflowService;
        private readonly IWorkflowProcessService workflowProcessService;
        private readonly ILogger<WorkflowDaemon> logger;

        /// <summary>
        /// Constructor of workflow daemon.
        /// </summary>
        /// <param name="options">Supported workflow settings.</param>
        /// <param name="workflowService">Workflow service.</param>
        /// <param name="workflowProcessService">Workflow process service.</param>
        /// <param name="logger">Logger.</param>
        public WorkflowDaemon(IOptions<SupportedWorkflowSettings> options,
            IWorkflowService workflowService,
            IWorkflowProcessService workflowProcessService,
            ILogger<WorkflowDaemon> logger)
        {
            this.settings = options?.Value;
            this.workflowService = workflowService;
            this.workflowProcessService = workflowProcessService;
            this.logger = logger;
            this.CurrentState = new DaemonStateMonitorServiceModel();
            this.workflowProcessService.WorkflowStateUpdated += WorkflowProcessService_WorkflowStateUpdated;
        }

        private void WorkflowProcessService_WorkflowStateUpdated(string workflowId, WorkflowStateMonitorServiceModel workflowState)
        {
            this.CurrentState.WorkflowsStatus.AddOrUpdate(workflowId, workflowState, (key, value) => workflowState);
        }

        /// <summary>
        /// Get the current state for the daemon.
        /// </summary>
        public DaemonStateMonitorServiceModel CurrentState { get; }

        /// <summary>
        /// Run the daemon.
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            List<WorkflowProcessorServiceModel> serviceModels;

            this.CurrentState.Status = WorkflowTaskStatusEnum.Started;

            //Support all workflows.
            if (this.settings == null || this.settings.SupportedWorkflows == null || this.settings.SupportedWorkflows.Count == 0)
            {
                var workflows = await this.workflowService.GetWorkflowsAsync(0, null);

                serviceModels = workflows?.Item2?.Select(x => new WorkflowProcessorServiceModel
                {
                    WorkflowId = x.Id,
                    WorkflowName = x.Name,
                    AvailableSteps = x.Steps?.ToList()
                })?.ToList();
            }
            else
            {
                serviceModels = new List<WorkflowProcessorServiceModel>();
                foreach (var workflowSetting in this.settings.SupportedWorkflows)
                {
                    var workflow = await this.workflowService.GetWorkflowAsync(workflowSetting.WorkflowId);
                    serviceModels.Add(new WorkflowProcessorServiceModel
                    {
                        WorkflowId = workflow.Id,
                        WorkflowName = workflow.Name,
                        AvailableSteps = workflow.Steps?.ToList()
                    });
                }
            }

            if (serviceModels == null || serviceModels.Count == 0)
            {
                this.logger?.LogInformation("There is no available workflows in this system.");
            }
            else
            {
                var tasks = serviceModels.Select(x => this.workflowProcessService.Start(x));

                await Task.WhenAll(tasks);
            }
        }
    }
}
