using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MongoDB.Driver;
using MusicKG.Service.Helpers;
using MongoDB.Bson;
using MusicKG.Workflow.Models.ServiceModels;

namespace MusicKG.Workflow.Services.Implementations
{
    /// <summary>
    /// Workflow management service.
    /// </summary>
    public class WorkflowService : IWorkflowService
    {
        private readonly IMusicKGContext context;

        /// <summary>
        /// Workflow management service constructor.
        /// </summary>
        /// <param name="context">Database context.</param>
        public WorkflowService(IMusicKGContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Create workflow.
        /// </summary>
        /// <param name="serviceModel">Workflow service model.</param>
        /// <returns>Created workflow service model.</returns>
        public async Task<WorkflowServiceModel> CreateWorkflowAsync(WorkflowServiceModel serviceModel)
        {
            var count = await this.context.Workflows.CountDocumentsAsync(x => x.Name == serviceModel.Name);

            if (count > 0)
            {
                ErrorHelper.ThrowException("Workflow name already exist.", System.Net.HttpStatusCode.BadRequest);
            }

            var dataModel = this.WorkflowServiceModelToDataModel(serviceModel);

            await this.context.Workflows.InsertOneAsync(dataModel);

            return this.WorkflowDataModelToServiceModel(dataModel);
        }
        
        /// <summary>
        /// Get workflow.
        /// </summary>
        /// <param name="id">Workflow Id.</param>
        /// <returns>Workflow service model.</returns>
        public async Task<WorkflowServiceModel> GetWorkflowAsync(string id)
        {
            var dataModel = await this.context.Workflows.Find(x => x.Id == new ObjectId(id)).FirstOrDefaultAsync();

            if (dataModel == null)
            {
                ErrorHelper.ThrowException("Workflow does not exist.", System.Net.HttpStatusCode.BadRequest);
            }

            return this.WorkflowDataModelToServiceModel(dataModel);
        }

        /// <summary>
        /// Get workflow list.
        /// </summary>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>The list of workflow.</returns>
        public async Task<Tuple<long, IEnumerable<WorkflowServiceModel>>> GetWorkflowsAsync(int from, int? size)
        {
            var workflowsFound = this.context.Workflows.Find(x => true);

            var totalCount = await workflowsFound.CountDocumentsAsync();

            if (from > 0)
            {
                workflowsFound = workflowsFound.Skip(from);
            }

            if (size.HasValue)
            {
                workflowsFound = workflowsFound.Limit(size.Value);
            }

            var workflows = await workflowsFound.ToListAsync();

            return new Tuple<long, IEnumerable<WorkflowServiceModel>>(totalCount,
                workflows.Select(x => this.WorkflowDataModelToServiceModel(x)));
        }

        private WorkflowServiceModel WorkflowDataModelToServiceModel(WorkflowDataModel dataModel)
        {
            return new WorkflowServiceModel
            {
                Id = dataModel.Id.ToString(),
                Name = dataModel.Name,
                Steps = dataModel.Steps?.Select(x => new WorkflowStepServiceModel
                {
                    Id = x.StepId.ToString(),
                    Name = x.Name,
                    WorkflowId = dataModel.Id.ToString(),
                    ProcessorAssembly = x.ProcessorAssembly,
                    ProcessorClass = x.ProcessorClass,
                    ResultDocumentStatus = x.ResultDocumentStatus,
                    InputFilter = x.InputFilter
                })
            };
        }

        private WorkflowDataModel WorkflowServiceModelToDataModel(WorkflowServiceModel serviceModel)
        {
            return new WorkflowDataModel
            {
                Name = serviceModel.Name,
                Steps = serviceModel.Steps.Select(x => new WorkflowStepDataModel
                {
                    StepId = ObjectId.GenerateNewId(),
                    Name = x.Name,
                    ProcessorAssembly = x.ProcessorAssembly,
                    ProcessorClass = x.ProcessorClass,
                    ResultDocumentStatus = x.ResultDocumentStatus,
                    InputFilter = x.InputFilter
                }).ToList(),
            };
        }
    }
}
