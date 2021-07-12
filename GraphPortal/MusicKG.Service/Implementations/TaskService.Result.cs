using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.Service.Resources;
using Microsoft.Extensions.Logging;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// Task Service.
    /// </summary>
    public partial class TaskService : ITaskService
    {
        /// <summary>
        /// Get Task result by document ID.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="annotatorId">Annotator ID.</param>
        /// <param name="documentId">Document ID.</param>
        /// <param name="resultType">Result type.</param>
        /// <returns>List of task results.</returns>
        public async Task<IEnumerable<TaskDocumentResultServiceModel>> GetTaskDocumentResultAsync(
            string workspaceId,
            string taskId,
            string annotatorId,
            string documentId,
            TaskDocumentResultTypeEnum? resultType)
        {
            var filter = new BsonDocument
            {
                { "_id", new ObjectId(taskId) },
                { "IsDeleted", false },
                { "WorkspaceId", new ObjectId(workspaceId) },
                { "Annotators.Documents.DocumentId", new ObjectId(documentId) }
            };

            if (!string.IsNullOrEmpty(annotatorId))
                filter.Add("Annotators.AnnotatorId", new ObjectId(annotatorId));

            var match = new BsonDocument("$match", filter);

            var redact = new BsonDocument("$redact", new BsonDocument("$cond", new BsonDocument
            {
                { "if", new BsonDocument("$or", new BsonArray
                {
                    new BsonDocument("$eq", new BsonArray { "missing", new BsonDocument("$type", "$DocumentId") }),
                    new BsonDocument("$eq", new BsonArray { "$DocumentId", new ObjectId(documentId) })
                })
                },
                { "then", "$$DESCEND" },
                { "else", "$$PRUNE" }
            }));

            var lookup = new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Users" },
                { "localField", "Annotators.AnnotatorId" },
                { "foreignField", "_id" },
                { "as", "AnnotatorObjects" }
            });

            var pipeline = new[]
            {
                match,
                redact,
                lookup
            };

            var task = await context.Tasks.Aggregate<TaskDocumentResult>(pipeline).FirstOrDefaultAsync();

            return task?.Annotators?.SelectMany(a => a?.Documents?.SelectMany(
                d => d?.Results?.Where(r => !resultType.HasValue || r.ResultType == resultType.Value).Select(
                    r => new TaskDocumentResultServiceModel
                    {
                        DocumentId = d.DocumentId.ToString(),
                        TaskDocumentStatus = d.Status,
                        ResultType = r.ResultType,
                        ResultDocumentId = r.ResultDocumentId.ToString(),
                        CreatedBy = task.AnnotatorObjects?.Where(an => an.Id == a.AnnotatorId).Select(u => new UserServiceModel
                        {
                            Id = u.Id.ToString(),
                            Name = u.Name,
                            Roles = u.Roles
                        }).FirstOrDefault(),
                        CreatedAt = r.CreatedAt
                    }))) ?? new List<TaskDocumentResultServiceModel>();
        }

        /// <summary>
        /// Save task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="annotatorId">Annotator ID.</param>
        /// <param name="documentId">Document ID.</param>
        /// <param name="result">Task document result.</param>
        /// <returns></returns>
        public async Task SaveTaskDocumentResultAsync(
            string workspaceId,
            string taskId,
            string annotatorId,
            string documentId,
            TaskDocumentResultSaveServiceModel result)
        {
            var filterBuilder = Builders<TaskDataModel>.Filter;
            var filter = filterBuilder.Eq(t => t.Id, new ObjectId(taskId))
                & filterBuilder.Eq(t => t.WorkspaceId, new ObjectId(workspaceId))
                & filterBuilder.Where(t => t.Status == TaskStatusEnum.Created || t.Status == TaskStatusEnum.Rejected)
                & filterBuilder.Eq(t => t.IsDeleted, false);

            var updateBuilder = Builders<TaskDataModel>.Update;

            var newTaskDocumentResult = new TaskDocumentResultDataModel
            {
                ResultDocumentId = new ObjectId(result.ResultDocumentId),
                ResultType = result.ResultType,
                CreatedAt = DateTime.UtcNow
            };

            var push = updateBuilder.Push("Annotators.$[annotator].Documents.$[document].Results", newTaskDocumentResult)
                .Set("Annotators.$[annotator].Documents.$[document].Status", result.Status);

            if (result.Status == TaskDocumentStatusEnum.Annotated)
            {
                push = push.Set("Annotators.$[annotator].Documents.$[document].AnnotatedAt", newTaskDocumentResult.CreatedAt);
            }

            var pushArrayFilters = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("annotator.AnnotatorId", new ObjectId(annotatorId))),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument
                {
                    { "document.DocumentId", new ObjectId(documentId) },
                    { "document.Status", TaskDocumentStatusEnum.Assigned },
                    { "$nor", new BsonArray {
                        new BsonDocument("document.Results",  new BsonDocument("$elemMatch", new BsonDocument("ResultType", result.ResultType))) }}
                })
            };

            var pushOption = new UpdateOptions
            {
                ArrayFilters = pushArrayFilters
            };

            var pushResult = await context.Tasks.UpdateOneAsync(filter, push, pushOption);

            if (pushResult.MatchedCount == 1 && pushResult.ModifiedCount == 1)
                return;

            var update = updateBuilder.Set("Annotators.$[annotator].Documents.$[document].Results.$[result]", newTaskDocumentResult)
                .Set("Annotators.$[annotator].Documents.$[document].Status", result.Status);

            if (result.Status == TaskDocumentStatusEnum.Annotated)
            {
                update = update.Set("Annotators.$[annotator].Documents.$[document].AnnotatedAt", newTaskDocumentResult.CreatedAt);
            }

            var updateArrayFilters = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("annotator.AnnotatorId", new ObjectId(annotatorId))),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument
                {
                    { "document.DocumentId", new ObjectId(documentId) },
                    { "document.Status", TaskDocumentStatusEnum.Assigned }
                }),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("result.ResultType", result.ResultType))
            };

            var updateOption = new UpdateOptions
            {
                ArrayFilters = updateArrayFilters
            };

            var updateResult = await context.Tasks.UpdateOneAsync(filter, update, updateOption);

            if (updateResult.MatchedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.BadRequest);

            if (updateResult.ModifiedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.TaskAnnotatorOrDocumentNotExistMessage, HttpStatusCode.BadRequest);
        }

        public async Task SaveBatchTaskDocumentResults(string workspaceId, string annotatorId, List<string> taskIdList)
        {
            this.logger?.LogInformation($"{nameof(TaskService)} - SaveBatchTaskDocumentResults, workspaceId:{workspaceId}, AnnotatorId:{annotatorId},taskIdList:{string.Join(',', taskIdList)}");
            BsonArray bs = new BsonArray();
            bs.AddRange(taskIdList.Select(x => new ObjectId(x)).ToList());
            
            var filter = new BsonDocument
            {
                { "_id",  new BsonDocument("$in", bs) },
                { nameof(TaskDataModel.WorkspaceId), new ObjectId(workspaceId) },
                { nameof(TaskDataModel.Status), new BsonDocument("$in", new BsonArray { TaskStatusEnum.Created, TaskStatusEnum.Rejected }) },
                { nameof(TaskDataModel.IsDeleted), false},
                {
                    nameof(TaskDataModel.Annotators),
                    new BsonDocument("$elemMatch", new BsonDocument
                    {
                        { nameof(TaskAnnotatorDataModel.AnnotatorId), new ObjectId(annotatorId) },
                        { nameof(TaskAnnotatorDataModel.IsManager), false },
                        { nameof(TaskAnnotatorDataModel.Documents), new BsonDocument("$elemMatch", new BsonDocument(nameof(TaskDocumentDataModel.Status), TaskDocumentStatusEnum.Assigned)) }
                    }) 
                }
            };

            var selectedTasks = await context.Tasks.Find(filter).ToListAsync();

            if (selectedTasks.Count == 0)
            {
                this.logger?.LogWarning($"{nameof(TaskService)}- no matching tasks.");
            }

            var replaceTaskCounts = 0;
            selectedTasks.ForEach(async task =>
            {
                var flag = false;
                var annotators = task.Annotators.Where(a => a.AnnotatorId == new ObjectId(annotatorId)).ToList();
                annotators.ForEach(annotator =>
                  {
                      foreach (var document in annotator.Documents)
                      {
                          var lastResult = document.Results?.LastOrDefault();
                          var documentId = lastResult == null ? document.DocumentId : lastResult.ResultDocumentId;
                          var newResultDataModel = new TaskDocumentResultDataModel()
                          {
                              ResultDocumentId = documentId,
                              ResultType = TaskDocumentResultTypeEnum.ForGraphMerging,
                              CreatedAt = DateTime.UtcNow
                          };
                          var results = lastResult != null ? document.Results.ToList() : new List<TaskDocumentResultDataModel>();
                          results.Add(newResultDataModel);

                          document.Results = results;
                          document.Status = TaskDocumentStatusEnum.Annotated;
                          document.AnnotatedAt = DateTime.UtcNow;
                          flag = true;
                      }
                  });

                if (flag)
                {
                    var replaceResult = await context.Tasks.ReplaceOneAsync(t => t.WorkspaceId == new ObjectId(workspaceId) && t.Id == task.Id, task);

                    if (replaceResult.IsAcknowledged && replaceResult.IsModifiedCountAvailable)
                        replaceTaskCounts++;
                    else
                        this.logger?.LogError($"{nameof(TaskService)}- task: {task.Id} submit result failed.");
                }
            });

            this.logger?.LogInformation($"{nameof(TaskService)}- there are {replaceTaskCounts} task documents to be submitted.");
        }

        [BsonIgnoreExtraElements]
        private class TaskDocumentResult : TaskDataModel
        {
            public IEnumerable<UserDataModel> AnnotatorObjects { get; set; }
            public IEnumerable<WorkspaceDataModel> WorkspaceObjects { get; set; }
            public IEnumerable<WorkspaceTypeDataModel> WorkspaceTypeObjects { get; set; }
            public IEnumerable<DocumentDataModel> DocumentObjects { get; set; }
            public IEnumerable<UserDataModel> InspectorObjects { get; set; }
            public IEnumerable<UserDataModel> AcceptorObjects { get; set; }
        }
    }
}
