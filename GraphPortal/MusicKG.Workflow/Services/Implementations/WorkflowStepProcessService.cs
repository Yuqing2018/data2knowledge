using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Settings;
using System.Reflection;
using System.IO;
using System.Linq;
using MusicKG.Service.Models;
using Microsoft.Extensions.Logging;
using MusicKG.DataAccess.Enums;
using System.Collections.Concurrent;
using MusicKG.Workflow.Enums;

namespace MusicKG.Workflow.Services.Implementations
{
    /// <summary>
    /// Workflow step process service.
    /// </summary>
    public class WorkflowStepProcessService : IWorkflowStepProcessService
    {
        private readonly IOptions<WorkflowStepServiceSettings> options;
        private readonly IWorkflowDocumentService workflowDocumentService;
        private readonly IProcessorFactory processorFactory;
        private readonly ILogger<WorkflowStepProcessService> logger;

        private readonly ConcurrentDictionary<string, StepStateMonitorServiceModel> status;

        /// <summary>
        /// Workflow step process service constructor.
        /// </summary>
        /// <param name="options">Options of workflow step process service.</param>
        /// <param name="workflowDocumentService">Workflow document service.</param>
        /// <param name="processorFactory">Processor factory.</param>
        /// <param name="logger">Logger.</param>
        public WorkflowStepProcessService(IOptions<WorkflowStepServiceSettings> options,
            IWorkflowDocumentService workflowDocumentService,
            IProcessorFactory processorFactory,
            ILogger<WorkflowStepProcessService> logger)
        {
            this.options = options;
            this.workflowDocumentService = workflowDocumentService;
            this.processorFactory = processorFactory;
            this.logger = logger;
            this.status = new ConcurrentDictionary<string, StepStateMonitorServiceModel>();
        }

        /// <summary>
        /// Workflow step state updated event.
        /// </summary>
        public event Action<string, string, StepStateMonitorServiceModel> WorkflowStepStateUpdated;

        /// <summary>
        /// Start workflow step.
        /// </summary>
        /// <param name="step">Workflow step configuraiton.</param>
        /// <returns></returns>
        public async Task Start(WorkflowStepServiceModel step)
        {
            string key = $"{step.WorkflowId}_{step.Id}";

            if (!this.status.TryAdd(key, new StepStateMonitorServiceModel { StepName = step.Name, Status = WorkflowTaskStatusEnum.Started, Action = WorkflowStepActionEnum.CreatingProcessor }))
            {
                this.logger?.LogWarning($"Workflow: {step.WorkflowId} [Step: {step.Id}] already startted...");
                return;
            }

            this.WorkflowStepStateUpdated?.Invoke(step.WorkflowId, step.Id, this.status[key]);

            this.logger?.LogInformation($"Creating Processsor for step {step.Name}");

            var processor = this.processorFactory.CreateProcessor(step.ProcessorAssembly, step.ProcessorClass);

            if (processor == null)
            {
                return;
            }

            this.logger?.LogInformation($"Processor for step {step.Name} has been created.");

            var filterServiceModel = new WorkflowStepInputFilterServiceModel
            {
                WorkflowId = step.WorkflowId,
                Status = step.InputFilter?.Status,
                BirthStep = step.InputFilter?.BirthStep,
                DeathStep = step.InputFilter?.DeathStep
            };

            while (true)
            {
                this.TriggerStatusUpdatedEvent(step.WorkflowId, step.Id, this.status[key], WorkflowStepActionEnum.FetchingDocuments);

                this.logger?.LogInformation($"Fetching input documents for step {step.Name}");

                var documents = await this.workflowDocumentService.FetchDocumentsAsync(filterServiceModel, options.Value.ProcessorCount);

                if (documents == null || documents.Count == 0)
                {
                    this.TriggerStatusUpdatedEvent(step.WorkflowId, step.Id, this.status[key], WorkflowStepActionEnum.WaitingForDocuments);

                    this.logger?.LogInformation($"There is no document match the input filter or some error is occured when fetching input documents for step {step.Name}, " +
                        $"the service will sleep {options.Value.ScanRate} milliseconds and then retry.");
                    await Task.Delay(this.options.Value.ScanRate);
                    continue;
                }

                this.TriggerStatusUpdatedEvent(step.WorkflowId, step.Id, this.status[key], WorkflowStepActionEnum.ProcessingDocuments);

                this.logger?.LogInformation($"{documents.Count} input doucments were found for step {step.Name} and ready for processing.");

                var processTasks = documents.Select(x => processor.ProcessAsync(x, step.Id, step.ResultDocumentStatus)).ToList();

                var updateTasks = new List<Task>();

                this.TriggerStatusUpdatedEvent(step.WorkflowId, step.Id, this.status[key], WorkflowStepActionEnum.UpdatingParentDocuments);

                while (processTasks.Count > 0)
                {
                    var finishedTask = await Task.WhenAny(processTasks);
                    updateTasks.Add(this.workflowDocumentService.UpdateDocumentAsync(step.Id, finishedTask.Result));
                    processTasks.Remove(finishedTask);
                }

                await Task.WhenAll(updateTasks);

                this.logger?.LogInformation($"{documents.Count} input doucments processing was finished for step {step.Name}.");
            }
        }

        private void TriggerStatusUpdatedEvent(string workflowId, string stepId,
            StepStateMonitorServiceModel currentStatus, WorkflowStepActionEnum action)
        {
            currentStatus.Action = action;

            this.WorkflowStepStateUpdated?.Invoke(workflowId, stepId, currentStatus);
        }
    }
}
