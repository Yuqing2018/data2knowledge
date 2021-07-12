using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MusicKG.Service.Models;
using MongoDB.Bson;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Resources;
using MusicKG.DataAccess.Constants;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// Workspace service.
    /// </summary>
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<WorkspaceService> logger;

        /// <summary>
        /// Workspace service constructor.
        /// </summary>
        /// <param name="userService">User service.</param>
        /// <param name="context">MusicKG mongodb context.</param>
        public WorkspaceService(
            IMusicKGContext context,
            ILogger<WorkspaceService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Get workspace list.
        /// </summary>
        /// <param name="language">Workspace language filter.</param>
        /// <param name="createdBy">Created by filter.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>The list of workspace object.</returns>
        public async Task<Tuple<long, IEnumerable<WorkspaceServiceModel>>> GetWorkspacesAsync(
            LanguageEnum? language = null,
            string userId = null,
            int from = 0,
            int? size = null)
        {
            var workspacesFilter = context.Workspaces.AsQueryable().Where(w => w.IsDeleted == false);

            if (language.HasValue)
                workspacesFilter = workspacesFilter.Where(w => w.Language == language);

            if (!string.IsNullOrWhiteSpace(userId))
                workspacesFilter = workspacesFilter.Where(w => w.CreatedBy == new ObjectId(userId) || w.ReadOnlyUserIds.Contains(new ObjectId(userId)));

            var totalCount = await workspacesFilter.CountAsync();

            workspacesFilter = workspacesFilter.Skip(from).Take(size ?? int.MaxValue);

            var workspaces = await (from w in workspacesFilter
                                    join t in context.WorkspaceTypes.AsQueryable() on w.Type equals t.Id
                                    join u in context.Users.AsQueryable() on w.CreatedBy equals u.Id
                                    select new WorkspaceResult
                                    {
                                        ObjectId = w.Id,
                                        Name = w.Name,
                                        Description = w.Description,
                                        Language = w.Language,
                                        IsAutoMerging = w.IsAutoMerging,
                                        CreatedAt = w.CreatedAt,
                                        TypeResult = new WorkspaceTypeResult
                                        {
                                            ObjectId = t.Id,
                                            Name = t.Name,
                                            WorkflowObjectId = t.WorkflowId
                                        },
                                        CreatedByResult = new UserResult
                                        {
                                            ObjectId = u.Id,
                                            Name = u.Name
                                        }
                                    }).ToListAsync();

            return new Tuple<long, IEnumerable<WorkspaceServiceModel>>(totalCount, workspaces);
        }

        /// <summary>
        /// Get workspace by workspace ID.
        /// </summary>
        /// <param name="id">Workspace ID.</param>
        /// <returns>Workspace service object.</returns>
        public async Task<WorkspaceServiceModel> GetWorkspaceAsync(string id)
        {
            var filter = new BsonDocument
            {
                { nameof(WorkspaceDataModel.IsDeleted), false },
                { "_id", new ObjectId(id) }
            };

            var pipeline = new List<BsonDocument>
            {
                new BsonDocument("$match", filter),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", CollectionNames.User },
                    { "localField", $"{nameof(WorkspaceDataModel.CreatedBy)}" },
                    { "foreignField", "_id" },
                    { "as", nameof(WorkspaceDataModelResult.CreateUsers) }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", CollectionNames.WorkspaceType },
                    { "localField", $"{nameof(WorkspaceDataModel.Type)}" },
                    { "foreignField", "_id" },
                    { "as", nameof(WorkspaceDataModelResult.WorkspaceTypes) }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", CollectionNames.User },
                    { "localField", $"{nameof(WorkspaceDataModel.ReadOnlyUserIds)}" },
                    { "foreignField", "_id" },
                    { "as", nameof(WorkspaceDataModelResult.ReadOnlyUsers) }
                })
            };

            var workspace = await context.Workspaces.Aggregate<WorkspaceDataModelResult>(pipeline).FirstOrDefaultAsync();

            if (workspace == null)
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceNotExistMessage, HttpStatusCode.BadRequest);

            return new WorkspaceResult()
            {
                ObjectId = workspace.Id,
                Name = workspace.Name,
                Description = workspace.Description,
                Language = workspace.Language,
                IsAutoMerging = workspace.IsAutoMerging,
                CreatedAt = workspace.CreatedAt,
                TypeResult = new WorkspaceTypeResult
                {
                    ObjectId = workspace.WorkspaceTypes.First().Id,
                    Name = workspace.WorkspaceTypes.First().Name,
                    WorkflowObjectId = workspace.WorkspaceTypes.First().WorkflowId
                },
                CreatedByResult = new UserResult
                {
                    ObjectId = workspace.CreateUsers.First().Id,
                    Name = workspace.CreateUsers.First().Name
                },
                ReadOnlyUsers = workspace.ReadOnlyUsers?.Select(m => new UserServiceModel
                {
                    Id = m.Id.ToString(),
                    Name = m.Name
                }).ToList()
            };
        }

        /// <summary>
        /// Create workspace.
        /// </summary>
        /// <param name="serviceModel">Workspace create service object.</param>
        /// <returns>Workspace service object.</returns>
        public async Task<string> CreateWorkspaceAsync(WorkspaceCreateServiceModel serviceModel)
        {
            var workspaceTypeCount = await context.WorkspaceTypes.CountDocumentsAsync(wt => wt.Id == new ObjectId(serviceModel.Type) && wt.Status == WorkspaceTypeStatusEnum.Enabled);
            if (workspaceTypeCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceTypeNotExistMessage, HttpStatusCode.BadRequest);

            var workspace = new WorkspaceDataModel
            {
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                IsAutoMerging = serviceModel.IsAutoMerging,
                Type = new ObjectId(serviceModel.Type),
                Language = serviceModel.Language,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = new ObjectId(serviceModel.CreatedBy),
                IsDeleted = false,
                ReadOnlyUserIds = serviceModel.ReadOnlyUsers != null ? serviceModel.ReadOnlyUsers.Select(m => new ObjectId(m)).ToList() : new List<ObjectId>(),
            };

            try
            {
                await context.Workspaces.InsertOneAsync(workspace);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.WorkspaceCreateFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = MusicKGMessages.WorkspaceNameExistMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return workspace.Id.ToString();
        }

        /// <summary>
        /// Update workspace.
        /// </summary>
        /// <param name="id">Workspace id to be updated.</param>
        /// <param name="serviceModel">Workspace update service model.</param>
        /// <returns></returns>
        public async Task UpdateWorkspaceAsync(string id, WorkspaceUpdateServiceModel serviceModel)
        {
            var update = Builders<WorkspaceDataModel>.Update.Set(u => u.Id, new ObjectId(id));
            if (serviceModel.IsNameAssigned)
            {
                update = update.Set(x => x.Name, serviceModel.Name);
            }
            if (serviceModel.IsDescriptionAssigned)
            {
                update = update.Set(x => x.Description, serviceModel.Description);
            }
            if (serviceModel.IsAutoMerging != null)
            {
                update = update.Set(x => x.IsAutoMerging, serviceModel.IsAutoMerging);
            }
            if (serviceModel.ReadOnlyUserIds != null)
            {
                update = update.Set(x => x.ReadOnlyUserIds, serviceModel.ReadOnlyUserIds.Select(m => new ObjectId(m)));
            }

            UpdateResult result = null;
            try
            {
                result = await context.Workspaces.UpdateOneAsync(w => w.Id == new ObjectId(id) && w.IsDeleted == false, update);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.WorkspaceUpdateFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = MusicKGMessages.WorkspaceNameExistMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            if (result?.MatchedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceNotExistMessage, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Delete workspace by ID.
        /// </summary>
        /// <param name="id">Workspace ID to be deleted.</param>
        public async Task DeleteWorkspaceAsync(string id)
        {
            var workspace = await context.Workspaces.Find(w => w.Id == new ObjectId(id) && w.IsDeleted == false).FirstOrDefaultAsync();
            if (workspace == null)
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceNotExistMessage, HttpStatusCode.BadRequest);

            var update = Builders<WorkspaceDataModel>.Update
                .Set(w => w.IsDeleted, true)
                .Set(w => w.Name, workspace.Name + DateTime.UtcNow)
                .Set(w => w.DeletedAt, DateTime.UtcNow);

            using (var session = await context.Client.StartSessionAsync())
            {
            StartTransaction:
                while (true)
                {
                    session.StartTransaction(new TransactionOptions(
                        readConcern: ReadConcern.Snapshot,
                        writeConcern: WriteConcern.WMajority));

                    UpdateResult result = null;
                    try
                    {
                        result = await context.Workspaces.UpdateOneAsync(session, w => w.Id == new ObjectId(id) && w.IsDeleted == false, update);
                        if (result?.MatchedCount == 0)
                            ErrorHelper.ThrowException(MusicKGMessages.WorkspaceNotExistMessage, HttpStatusCode.BadRequest);

                        var tasks = await context.Tasks
                            .Find(session, t => t.WorkspaceId == new ObjectId(id) && t.IsDeleted == false)
                            .ToListAsync();

                        if (tasks.Count() == 0)
                            break;

                        var filterBuilder = Builders<TaskDataModel>.Filter;
                        var updateBuilder = Builders<TaskDataModel>.Update;
                        var bulkUpdate = tasks.Select(t => new UpdateOneModel<TaskDataModel>(
                            filterBuilder.Eq(ti => ti.Id, t.Id),
                            updateBuilder
                                .Set(ti => ti.IsDeleted, true)
                                .Set(ti => ti.Name, t.Name + DateTime.UtcNow)
                                .Set(ti => ti.DeletedAt, DateTime.UtcNow)));

                        await context.Tasks.BulkWriteAsync(session, bulkUpdate);
                    }
                    catch (Exception e)
                    {
                        if (session.IsInTransaction)
                            await session.AbortTransactionAsync();

                        if (e is MongoException mongoException && mongoException.HasErrorLabel("TransientTransactionError"))
                            continue;

                        if (e is ErrorHelper.ErrorMessageException)
                            throw;

                        logger?.LogError(e, MusicKGMessages.WorkspaceDeleteFailedMessage);

                        ErrorHelper.ThrowException(MusicKGMessages.WorkspaceDeleteFailedMessage, HttpStatusCode.InternalServerError);
                    }

                    break;
                }

                while (true)
                {
                    try
                    {
                        await session.CommitTransactionAsync();
                    }
                    catch (Exception e)
                    {
                        if (session.IsInTransaction)
                            await session.AbortTransactionAsync();

                        if (e is MongoException mongoException)
                        {
                            if (mongoException.HasErrorLabel("UnknownTransactionCommitResult"))
                                continue;

                            if (mongoException.HasErrorLabel("TransientTransactionError"))
                                goto StartTransaction;
                        }

                        logger?.LogError(e, MusicKGMessages.WorkspaceDeleteFailedMessage);

                        ErrorHelper.ThrowException(MusicKGMessages.WorkspaceDeleteFailedMessage, HttpStatusCode.InternalServerError);
                    }

                    break;
                }
            }
        }

        [BsonIgnoreExtraElements]
        private class WorkspaceDataModelResult : WorkspaceDataModel
        {
            public IEnumerable<UserDataModel> CreateUsers { get; set; }

            public IEnumerable<UserDataModel> ReadOnlyUsers { get; set; }

            public IEnumerable<WorkspaceTypeDataModel> WorkspaceTypes { get; set; }
        }
    }

    [BsonIgnoreExtraElements]
    public class WorkspaceResult : WorkspaceServiceModel
    {
        public ObjectId ObjectId { set { Id = value.ToString(); } }

        public WorkspaceTypeResult TypeResult { set { Type = value; } }

        public UserResult CreatedByResult { set { CreatedBy = value; } }
    }

    [BsonIgnoreExtraElements]
    public class WorkspaceTypeResult : WorkspaceTypeServiceModel
    {
        public ObjectId ObjectId { set { Id = value.ToString(); } }

        public ObjectId WorkflowObjectId { set { WorkflowId = value.ToString(); } }
    }

    [BsonIgnoreExtraElements]
    public class UserResult : UserServiceModel
    {
        public ObjectId ObjectId { set { Id = value.ToString(); } }
    }


}
