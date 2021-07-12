using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MusicKG.Workflow.Models.ServiceModels;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using MusicKG.Workflow.Enums;

namespace MusicKG.Workflow.Services.Implementations
{
    /// <summary>
    /// Workflow process service.
    /// </summary>
    public class WorkflowProcessService : IWorkflowProcessService
    {
        private readonly IWorkflowStepProcessService workflowStepService;
        private readonly ILogger<WorkflowProcessService> logger;

        private readonly ConcurrentDictionary<string, WorkflowStateMonitorServiceModel> status;

        /// <summary>
        /// Workflow process service constructor.
        /// </summary>
        /// <param name="workflowStepService">Workflow step service.</param>
        /// <param name="logger">Logger.</param>
        public WorkflowProcessService(IWorkflowStepProcessService workflowStepService,
            ILogger<WorkflowProcessService> logger)
        {
            this.workflowStepService = workflowStepService;
            this.logger = logger;
            this.status = new ConcurrentDictionary<string, WorkflowStateMonitorServiceModel>();
            this.workflowStepService.WorkflowStepStateUpdated += WorkflowStepService_WorkflowStepStateUpdated;
        }

        private void WorkflowStepService_WorkflowStepStateUpdated(string workflowId, string stepId, StepStateMonitorServiceModel stepState)
        {
            this.status[workflowId].StepStatus.AddOrUpdate(stepId, stepState, (key, value) => stepState);
            this.WorkflowStateUpdated?.Invoke(workflowId, this.status[workflowId]);
        }

        /// <summary>
        /// Workflow state updated event.
        /// </summary>
        public event Action<string, WorkflowStateMonitorServiceModel> WorkflowStateUpdated;

        /// <summary>
        /// Start workflow process service.
        /// </summary>
        /// <param name="workflow">Workflow processor service model.</param>
        /// <returns></returns>
        public async Task Start(WorkflowProcessorServiceModel workflow)
        {
            if (!this.status.TryAdd(workflow.WorkflowId, new WorkflowStateMonitorServiceModel
            {
                WorkflowName = workflow.WorkflowName,
                Status = WorkflowTaskStatusEnum.Started
            }))
            {
                this.logger?.LogWarning($"Workflow: {workflow.WorkflowId} already started.");
                return;
            }

            this.WorkflowStateUpdated?.Invoke(workflow.WorkflowId, this.status[workflow.WorkflowId]);

            List<Task> tasks = new List<Task>();
            this.logger?.LogInformation($"Starting workflow: {workflow.WorkflowId}");
            foreach (var step in workflow.AvailableSteps)
            {
                this.logger?.LogInformation($"Starting step: {step.Name}");
                tasks.Add(this.workflowStepService.Start(step));
            }          
            await Task.WhenAll(tasks.ToArray());
        }
    }
}
