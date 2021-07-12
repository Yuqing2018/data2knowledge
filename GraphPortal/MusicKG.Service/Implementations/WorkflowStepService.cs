using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MongoDB.Driver.Linq;
using MusicKG.Service.Resources;
using MusicKG.DataAccess.Enums;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// Workflow step service.
    /// </summary>
    public class WorkflowStepService : IWorkflowStepService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<WorkflowStepService> logger;

        public WorkflowStepService(
            IMusicKGContext context,
            ILogger<WorkflowStepService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Get next workflow step.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="parentDocumentId">Parent document ID.</param>
        /// <returns>Workflow step object.</returns>
        public async Task<WorkflowNextStepServiceModel> GetNextWorkflowStepAsync(string workspaceId, string parentDocumentId)
        {
            var workflow = await (from ws in context.Workspaces.AsQueryable()
                                  join wst in context.WorkspaceTypes.AsQueryable() on ws.Type equals wst.Id
                                  join wf in context.Workflows.AsQueryable() on wst.WorkflowId equals wf.Id
                                  where ws.Id == new ObjectId(workspaceId)
                                  select wf).FirstOrDefaultAsync();

            var parentDocument = string.IsNullOrWhiteSpace(parentDocumentId) ? null :
                await context.Documents.Find(d => d.Id == new ObjectId(parentDocumentId)).FirstOrDefaultAsync();

            var step = parentDocument == null ?
                workflow?.Steps?.FirstOrDefault() :
                workflow?.Steps?.Zip(workflow?.Steps?.Skip(1), (p, c) => (p, c))
                .Where(z => z.p.StepId == parentDocument.BirthStep.StepId)
                .Select(z => z.c)
                .FirstOrDefault();

            if (step == null)
            {
                var message = MusicKGMessages.WorkflowStepGetFailedMessage;
                logger?.LogError($"{message} workflowId: [{workflow.Id}], parentDocumentId: [{parentDocumentId}], parentDocumentStepId: [{parentDocument?.BirthStep.StepId}]");

                ErrorHelper.ThrowException(message);
            }

            return new WorkflowNextStepServiceModel
            {
                Id = step.StepId.ToString(),
                Name = step.Name,
                AssemblyName = step.ProcessorAssembly,
                ClassName = step.ProcessorClass,
                ResultDocumentStatus = step.ResultDocumentStatus,
                WorkflowId = workflow.Id.ToString(),
                AutoDoNext = step.AutoDoNext
            };
        }

        /// <summary>
        /// Get preannotation workflow step.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <returns>Workflow step object.</returns>
        public async Task<WorkflowNextStepServiceModel> GetPreannotationWorkflowStepAsync(string workspaceId)
        {
            var workflow = await (from ws in context.Workspaces.AsQueryable()
                                  join wst in context.WorkspaceTypes.AsQueryable() on ws.Type equals wst.Id
                                  join wf in context.Workflows.AsQueryable() on wst.WorkflowId equals wf.Id
                                  where ws.Id == new ObjectId(workspaceId)
                                  select wf).FirstOrDefaultAsync();

            var preannotationStep = workflow?.Steps?
                .Where(step => step.ResultDocumentStatus == DocumentStatusEnum.Preannotated)?
                .FirstOrDefault();

            return new WorkflowNextStepServiceModel
            {
                Id = preannotationStep.StepId.ToString(),
                Name = preannotationStep.Name,
                AssemblyName = preannotationStep.ProcessorAssembly,
                ClassName = preannotationStep.ProcessorClass,
                ResultDocumentStatus = preannotationStep.ResultDocumentStatus,
                WorkflowId = workflow.Id.ToString(),
                AutoDoNext = preannotationStep.AutoDoNext
            };
        }
    }
}
