using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Helpers;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Resources;
using MusicKG.Service.ResultHandler;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// Task Service.
    /// </summary>
    public partial class TaskService : ITaskService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<TaskService> logger;
        private readonly IWorkflowStepService workflowStepService;
        private readonly IDocumentService documentService;
        private readonly IConfiguration configuration;

        public TaskService(
            IConfiguration configuration,
            IMusicKGContext context,
            ILogger<TaskService> logger,
            IWorkflowStepService workflowStepService,
            IDocumentService documentService)
        {
            this.configuration = configuration;
            this.context = context;
            this.logger = logger;
            this.workflowStepService = workflowStepService;
            this.documentService = documentService;
        }

        /// <summary>
        /// Create Task.
        /// </summary>
        /// <param name="taskCreate">TaskCreateServiceModel.</param>
        /// <returns></returns>
        public async Task<string> CreateTaskAsync(TaskCreateServiceModel taskCreate)
        {
            var filterBuilder = Builders<DocumentDataModel>.Filter;

            var uploadStep = await workflowStepService.GetPreannotationWorkflowStepAsync(taskCreate.WorkspaceId);

            var documentCount = await context.Documents.CountDocumentsAsync(filterBuilder.And(new[]
            {
                filterBuilder.In(d => d.Id, taskCreate.DocumentIds.Select(id => new ObjectId(id))),
                filterBuilder.Eq(d => d.WorkspaceId, new ObjectId(taskCreate.WorkspaceId)),
                filterBuilder.In(d => d.Status, new[] { uploadStep.ResultDocumentStatus, DocumentStatusEnum.Assigned }),
                filterBuilder.Eq(d => d.WorkflowId, new ObjectId(uploadStep.WorkflowId)),
                filterBuilder.Eq(d => d.BirthStep.StepId, new ObjectId(uploadStep.Id))
            }));

            if (documentCount != taskCreate.DocumentIds.Count())
                ErrorHelper.ThrowException(MusicKGMessages.DocumentNotExistMessage, HttpStatusCode.BadRequest);

            var task = new TaskDataModel()
            {
                Name = taskCreate.Name,
                ExpectedDueAt = taskCreate.ExpectedDueAt,
                CreatedAt = DateTime.UtcNow,
                Status = TaskStatusEnum.Created,
                Overlap = taskCreate.Overlap,
                CreatedBy = new ObjectId(taskCreate.CreateBy),
                WorkspaceId = new ObjectId(taskCreate.WorkspaceId),
                IsAutoApproved = taskCreate.IsAutoApproved,
                IsAutoMerged = taskCreate.IsAutoMerged,
                Annotators = OverlapRateHelper.AssignDocumentByOverlap(taskCreate.AnnotatorIds.Count, taskCreate.Overlap, taskCreate.DocumentIds).Select((d, i) => new TaskAnnotatorDataModel
                {
                    AnnotatorId = new ObjectId(taskCreate.AnnotatorIds[i]),
                    IsManager = false,
                    Documents = d.Select(td => new TaskDocumentDataModel()
                    {
                        DocumentId = new ObjectId(td),
                        Status = TaskDocumentStatusEnum.Assigned,
                        Results = new List<TaskDocumentResultDataModel>(),
                        AnnotatedAt = DateTime.MaxValue.ToUniversalTime()
                    })
                }).ToList(),
                DictionaryIds = taskCreate.DictionaryIds != null ? taskCreate.DictionaryIds.Select(m => new ObjectId(m)).ToList() : new List<ObjectId>(),
                InspectorIds = taskCreate.InspectorIds?.Select(m => new ObjectId(m)).ToList(),
                AcceptorIds = taskCreate.AcceptorIds?.Select(m => new ObjectId(m)).ToList()
            };

            if (string.IsNullOrWhiteSpace(taskCreate.TaskType))
            {
                task.TaskType = null;
            }
            else
            {
                task.TaskType = new ObjectId(taskCreate.TaskType);
            }

            task.Annotators.Add(new TaskAnnotatorDataModel()
            {
                AnnotatorId = new ObjectId(taskCreate.CreateBy),
                IsManager = true,
                Documents = taskCreate.DocumentIds.Select(td => new TaskDocumentDataModel()
                {
                    DocumentId = new ObjectId(td),
                    Status = TaskDocumentStatusEnum.Assigned,
                    Results = new List<TaskDocumentResultDataModel>(),
                    AnnotatedAt = DateTime.MaxValue.ToUniversalTime()
                })
            });

            using (var session = await context.Client.StartSessionAsync())
            {
            StartTransaction:
                while (true)
                {
                    session.StartTransaction(new TransactionOptions(
                        readConcern: ReadConcern.Snapshot,
                        writeConcern: WriteConcern.WMajority));

                    try
                    {
                        await context.Documents.UpdateManyAsync(session,
                            filterBuilder.In(d => d.Id, taskCreate.DocumentIds.Select(id => new ObjectId(id)))
                                & filterBuilder.Eq(d => d.Status, DocumentStatusEnum.Preannotated),
                            Builders<DocumentDataModel>.Update.Set(d => d.Status, DocumentStatusEnum.Assigned));

                        await context.Tasks.InsertOneAsync(session, task);
                    }
                    catch (Exception e)
                    {
                        if (session.IsInTransaction)
                            await session.AbortTransactionAsync();

                        if (e is MongoException mongoException && mongoException.HasErrorLabel("TransientTransactionError"))
                            continue;

                        var message = MusicKGMessages.TaskCreateFailedMessage;
                        var statusCode = HttpStatusCode.InternalServerError;

                        if (e is MongoCommandException mongoCommandException && ServerErrorCategory.DuplicateKey.ToString().Equals(mongoCommandException.CodeName))
                        {
                            message = MusicKGMessages.TaskNameExistMessage;
                            statusCode = HttpStatusCode.BadRequest;
                        }

                        logger?.LogError(e, message);

                        ErrorHelper.ThrowException(message, statusCode);
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

                        logger?.LogError(e, MusicKGMessages.TaskCreateFailedMessage);

                        ErrorHelper.ThrowException(MusicKGMessages.TaskCreateFailedMessage, HttpStatusCode.InternalServerError);
                    }

                    break;
                }
            }

            return task.Id.ToString();
        }

        /// <summary>
        /// Update Task.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="taskId">Task Id.</param>
        /// <param name="taskUpdate">TaskUpdateServiceModel.</param>
        /// <returns></returns>
        public async Task UpdateTaskAsync(string workspaceId, string taskId, TaskUpdateServiceModel taskUpdate)
        {
            var update = Builders<TaskDataModel>.Update.Set(u => u.Id, new ObjectId(taskId));

            if (taskUpdate.IsNameAssigned)
                update = update.Set(u => u.Name, taskUpdate.Name);

            if (taskUpdate.ExpectedDueAt.HasValue)
            {
                if (taskUpdate.ExpectedDueAt <= DateTime.UtcNow)
                    ErrorHelper.ThrowException(MusicKGMessages.TaskExpectedDueAtWrongMessage, HttpStatusCode.BadRequest);

                update = update.Set(u => u.ExpectedDueAt, taskUpdate.ExpectedDueAt);
            }

            UpdateResult result = null;
            try
            {
                result = await context.Tasks.UpdateOneAsync(t => t.Id == new ObjectId(taskId) && t.WorkspaceId == new ObjectId(workspaceId), update);
            }
            catch (Exception e)
            {
                var message = MusicKGMessages.TaskUpdateFailedMessage;
                var statusCode = HttpStatusCode.InternalServerError;

                if (e is MongoWriteException && ServerErrorCategory.DuplicateKey == ((MongoWriteException)e).WriteError?.Category)
                {
                    message = MusicKGMessages.TaskNameExistMessage;
                    statusCode = HttpStatusCode.BadRequest;
                }

                logger?.LogError(e, message);

                ErrorHelper.ThrowException(message, statusCode);
            }

            if (result?.MatchedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Approve Task.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="taskId">Task Id.</param>
        /// <returns></returns>
        public async Task ApproveTaskAsync(string workspaceId, string taskId)
        {
            var filter = new BsonDocument
            {
                { "_id", new ObjectId(taskId) },
                { nameof(TaskDataModel.WorkspaceId), new ObjectId(workspaceId) },
                { nameof(TaskDataModel.Status), new BsonDocument("$in", new BsonArray { TaskStatusEnum.Created, TaskStatusEnum.Rejected }) },
                {
                    nameof(TaskDataModel.Annotators),
                    new BsonDocument("$not", new BsonDocument("$elemMatch", new BsonDocument
                    {
                        { nameof(TaskAnnotatorDataModel.IsManager), false },
                        { nameof(TaskAnnotatorDataModel.Documents), new BsonDocument("$elemMatch", new BsonDocument(nameof(TaskDocumentDataModel.Status), TaskDocumentStatusEnum.Assigned)) }
                    }))
                }
            };

            var update = Builders<TaskDataModel>.Update.Set(u => u.Status, TaskStatusEnum.Accepted);

            var result = await context.Tasks.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.NoTaskCanBeApprovedMessage, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Reject Task.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="taskId">Task Id.</param>
        /// <returns></returns>
        public async Task RejectTaskAsync(string workspaceId, string taskId, string annotatorId = null)
        {
            var filter = new BsonDocument
            {
                { "_id", new ObjectId(taskId) },
                { nameof(TaskDataModel.WorkspaceId), new ObjectId(workspaceId) },
                { nameof(TaskDataModel.Status), TaskStatusEnum.Created },
                {
                    "$and",
                    new BsonArray
                    {
                        new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$not", new BsonDocument("$elemMatch", new BsonDocument
                        {
                            { nameof(TaskAnnotatorDataModel.IsManager), false },
                            { nameof(TaskAnnotatorDataModel.Documents), new BsonDocument("$elemMatch", new BsonDocument(nameof(TaskDocumentDataModel.Status), TaskDocumentStatusEnum.Assigned)) }
                        }))),
                        new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$not", new BsonDocument("$elemMatch", new BsonDocument
                        {
                            { nameof(TaskAnnotatorDataModel.IsManager), true },
                            { nameof(TaskAnnotatorDataModel.Documents), new BsonDocument("$elemMatch", new BsonDocument(nameof(TaskDocumentDataModel.Status), TaskDocumentStatusEnum.Annotated)) }
                        })))
                    }
                }
            };

            var update = Builders<TaskDataModel>.Update
                .Set(t => t.Status, TaskStatusEnum.Rejected)
                .Set($"{nameof(TaskDataModel.Annotators)}.$[annotator].{nameof(TaskAnnotatorDataModel.Documents)}.$[document].{nameof(TaskDocumentDataModel.Status)}", TaskDocumentStatusEnum.Assigned)
                .Set($"{nameof(TaskDataModel.Annotators)}.$[annotator].{nameof(TaskAnnotatorDataModel.Documents)}.$[document].{nameof(TaskDocumentDataModel.AnnotatedAt)}", DateTime.MaxValue.ToUniversalTime());

            var arrayFilter = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument($"document.{nameof(TaskDocumentDataModel.Status)}", TaskDocumentStatusEnum.Annotated))
            };

            if (!string.IsNullOrEmpty(annotatorId))
            {
                arrayFilter.Add(new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument()
                {
                    { $"annotator.{nameof(TaskAnnotatorDataModel.AnnotatorId)}", new ObjectId(annotatorId) },
                    { $"annotator.{nameof(TaskAnnotatorDataModel.IsManager)}", false}
                }));
            }
            else
            {
                arrayFilter.Add(new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument($"annotator.{nameof(TaskAnnotatorDataModel.IsManager)}", false)));
            }

            var result = await context.Tasks.UpdateOneAsync(filter, update, new UpdateOptions
            {
                ArrayFilters = arrayFilter
            });

            if (result.MatchedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.NoTaskCanBeRejectMessage, HttpStatusCode.BadRequest);
        }

        public async Task<List<string>> GetTaskResultsAsync(string workspaceId, string taskId, TaskDocumentResultTypeEnum? resultType)
        {
            var task = await context.Tasks.AsQueryable().FirstOrDefaultAsync(t => t.WorkspaceId == new ObjectId(workspaceId) && t.Id == new ObjectId(taskId));

            if (task == null)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.NotFound);

            var taskDocumentsTotal = task.Annotators
               .SelectMany(a => a.Documents.Select(d => (annotator: a, taskDocument: d)))
               .GroupBy(td => td.taskDocument.DocumentId)
               .OrderBy(g => g.Key)
               .ToList();

            var taskResults = taskDocumentsTotal
                .Select(g =>
                {
                    IEnumerable<TaskDocumentResultDataModel> results;
                    if (!task.IsAutoMerged)
                    {
                        return g.FirstOrDefault(td => td.annotator.IsManager).taskDocument.Results?.Where(r => r.ResultType == resultType).OrderByDescending(r => r.CreatedAt).FirstOrDefault();
                    }
                    else
                    {
                        results = g.SelectMany(td => td.taskDocument.Results);
                    }

                    if (resultType.HasValue)
                        return results?.Where(r => r.ResultType == resultType).OrderByDescending(r => r.CreatedAt).FirstOrDefault();
                    else
                        return results?.OrderByDescending(r => r.CreatedAt).FirstOrDefault();
                })
                .ToList();

            return taskResults?.Select(t => t.ResultDocumentId.ToString())?.ToList();
        }

        public async Task HandleTaskResultAsync(string workspaceId, string taskId)
        {
            var workspace = await context.Workspaces.AsQueryable().FirstOrDefaultAsync(w => w.Id == new ObjectId(workspaceId));

            var task = await context.Tasks.AsQueryable().FirstOrDefaultAsync(t => t.WorkspaceId == new ObjectId(workspaceId) && t.Id == new ObjectId(taskId));

            if (task == null)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.NotFound);

            await UpdateTaskStatusAsync(workspaceId, taskId, TaskStatusEnum.KnowledgeMerged);
        }

        private async Task HandleResultAsync(string workspaceId, string taskId, string resultDocumentId, IResultHandler handler)
        {
            var result = await documentService.GetDocumentContentAsync(workspaceId, resultDocumentId);

            if (result != null && result.Length > 0)
            {
                await handler.HandleResultAsync(configuration, result);
            }
        }

        private async Task UpdateTaskStatusAsync(string workspaceId, string taskId, TaskStatusEnum status)
        {
            await context.Tasks.UpdateOneAsync(task => task.WorkspaceId == new ObjectId(workspaceId) && task.Id == new ObjectId(taskId),
                Builders<TaskDataModel>.Update.Set(t => t.Status, status));
        }

        private IResultHandler GetResultHandler(string assemblyName, string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return null;

            try
            {
                Assembly assembly;

                if (string.IsNullOrWhiteSpace(assemblyName))
                {
                    assembly = Assembly.GetExecutingAssembly();
                }
                else
                {
                    assembly = Assembly.Load(assemblyName);
                }

                return assembly.CreateInstance(className) as IResultHandler;
            }
            catch (Exception ex)
            {
                logger.LogError($"Create result handler failed, details: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Delete Task.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="taskId">Task Id.</param>
        /// <returns></returns>
        public async Task DeleteTaskAsync(string workspaceId, string taskId)
        {
            var task = await context.Tasks
                .Find(t => t.Id == new ObjectId(taskId) && t.WorkspaceId == new ObjectId(workspaceId) && t.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (task == null)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.BadRequest);

            var update = Builders<TaskDataModel>.Update
                .Set(t => t.IsDeleted, true)
                .Set(t => t.Name, task.Name + DateTime.UtcNow)
                .Set(t => t.DeletedAt, DateTime.UtcNow);

            var result = await context.Tasks.UpdateOneAsync(t => t.Id == task.Id && t.IsDeleted == false, update);

            if (result.MatchedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.BadRequest);
        }
    }
}
