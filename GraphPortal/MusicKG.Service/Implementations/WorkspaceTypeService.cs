using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MusicKG.Service.Helpers;
using System.Net;
using MusicKG.DataAccess.Models;
using MongoDB.Bson;
using System.Linq;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// Workspace type service.
    /// </summary>
    public class WorkspaceTypeService : IWorkspaceTypeService
    {
        private readonly IMusicKGContext context;

        /// <summary>
        /// Workspace type service constructor.
        /// </summary>
        /// <param name="context"></param>
        public WorkspaceTypeService(IMusicKGContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Create workspace type.
        /// </summary>
        /// <param name="serviceModel">Workspace type create service model.</param>
        /// <returns>Workspace type service model.</returns>
        public async Task<WorkspaceTypeServiceModel> CreateWorkspaceTypeAsync(WorkspaceTypeCreateServiceModel serviceModel)
        {
            var count = await this.context.WorkspaceTypes.CountDocumentsAsync(x => x.Name == serviceModel.Name);

            if (count > 0)
            {
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceTypeNameExistMessage, HttpStatusCode.BadRequest);
            }

            if (!ObjectId.TryParse(serviceModel.WorkflowId, out ObjectId workflowId))
            {
                workflowId = ObjectId.Empty;
            }

            var dataModel = new WorkspaceTypeDataModel
            {
                Name = serviceModel.Name,
                Status = serviceModel.Status,
                WorkflowId = workflowId
            };

            await this.context.WorkspaceTypes.InsertOneAsync(dataModel);

            var workflow = await this.context.Workflows.Find(x => x.Id == workflowId).FirstOrDefaultAsync();

            return this.WorkspaceTypeDataModelToServiceModel(dataModel, workflow?.Name);
        }

        /// <summary>
        /// Get workspace type.
        /// </summary>
        /// <param name="id">Workspace type Id.</param>
        /// <returns>Workspace type service model.</returns>
        public async Task<WorkspaceTypeServiceModel> GetWorkspaceTypeAsync(string id)
        {
            var workspaceType = await this.context.WorkspaceTypes.Find(x => x.Id == new ObjectId(id)).FirstOrDefaultAsync();

            if (workspaceType == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceTypeNotExistMessage, HttpStatusCode.BadRequest);
            }

            var workflow = await this.context.Workflows.Find(x => x.Id == workspaceType.WorkflowId).FirstOrDefaultAsync();

            return this.WorkspaceTypeDataModelToServiceModel(workspaceType, workflow?.Name);
        }

        /// <summary>
        /// Get workspace type list.
        /// </summary>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <param name="status">Workspace type status.</param>
        /// <returns>The list of workspace types.</returns>
        public async Task<Tuple<long, IEnumerable<WorkspaceTypeServiceModel>>> GetWorkspaceTypesAsync(int from, int? size, WorkspaceTypeStatusEnum? status = WorkspaceTypeStatusEnum.Enabled)
        {
            var builder = Builders<WorkspaceTypeDataModel>.Filter;

            var filter = builder.Empty;

            if (status.HasValue)
            {
                filter &= builder.Eq(x => x.Status, status.Value);
            }

            var workspaceTypeFound = this.context.WorkspaceTypes.Find(filter);
            var totalCount = await workspaceTypeFound.CountDocumentsAsync();

            if (from > 0)
            {
                workspaceTypeFound = workspaceTypeFound.Skip(from);
            }
            if (size.HasValue)
            {
                workspaceTypeFound = workspaceTypeFound.Limit(size.Value);
            }

            var workspaceTypes = await workspaceTypeFound.ToListAsync();

            var workspaceTypeServiceModels = new List<WorkspaceTypeServiceModel>();

            foreach (var workspaceType in workspaceTypes)
            {
                var workflow = await this.context.Workflows.Find(x => x.Id == workspaceType.WorkflowId).FirstOrDefaultAsync();

                workspaceTypeServiceModels.Add(this.WorkspaceTypeDataModelToServiceModel(workspaceType, workflow?.Name));
            }

            return new Tuple<long, IEnumerable<WorkspaceTypeServiceModel>>(totalCount, workspaceTypeServiceModels);
        }

        /// <summary>
        /// Update workspace type.
        /// </summary>
        /// <param name="id">Workspace type Id.</param>
        /// <param name="serviceModel">Workspace type update service model.</param>
        /// <returns>The workspace type.</returns>
        public async Task<WorkspaceTypeServiceModel> UpdateWorkspaceTypeAsync(string id, WorkspaceTypeUpdateServiceModel serviceModel)
        {
            var workspaceType = await this.context.WorkspaceTypes.Find(x => x.Id == new ObjectId(id)).FirstOrDefaultAsync();

            if (workspaceType == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceTypeNotExistMessage, HttpStatusCode.BadRequest);
            }

            if (serviceModel.IsNameAssigned)
            {
                if (workspaceType.Name != serviceModel.Name)
                {
                    var count = await context.WorkspaceTypes.CountDocumentsAsync(x => string.Equals(x.Name, serviceModel.Name));
                    if (count > 0)
                    {
                        ErrorHelper.ThrowException(MusicKGMessages.WorkspaceTypeNameExistMessage, HttpStatusCode.BadRequest);
                    }
                }
            }

            var update = Builders<WorkspaceTypeDataModel>.Update.Set(u => u.Id, workspaceType.Id);

            if (serviceModel.IsNameAssigned)
            {
                update = update.Set(x => x.Name, serviceModel.Name);
            }
            if (serviceModel.IsStatusAssigned)
            {
                update = update.Set(x => x.Status, serviceModel.Status);
            }

            workspaceType = await this.context.WorkspaceTypes.FindOneAndUpdateAsync<WorkspaceTypeDataModel>(x => x.Id == new ObjectId(id),
                update, new FindOneAndUpdateOptions<WorkspaceTypeDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });

            var workflow = await this.context.Workflows.Find(x => x.Id == workspaceType.WorkflowId).FirstOrDefaultAsync();

            return this.WorkspaceTypeDataModelToServiceModel(workspaceType, workflow?.Name);
        }

        private WorkspaceTypeServiceModel WorkspaceTypeDataModelToServiceModel(WorkspaceTypeDataModel dataModel, string workflowName)
        {
            return new WorkspaceTypeServiceModel
            {
                Id = dataModel.Id.ToString(),
                Name = dataModel.Name,
                WorkflowId = dataModel.WorkflowId.ToString(),
                WorkflowName = workflowName
            };
        }
    }
}
