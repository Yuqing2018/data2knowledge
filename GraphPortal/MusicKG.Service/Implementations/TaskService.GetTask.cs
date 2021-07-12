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
using MusicKG.Service.Resources;
using MusicKG.DataAccess.Constants;

namespace MusicKG.Service.Implementations
{
    public partial class TaskService : ITaskService
    {
        /// <summary>
        /// Get Task.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="taskId">Task Id.</param>
        /// <param name="annotatorId">Annotator Id.</param>
        /// <param name="isIncludingDocuments">Is including documents in result.</param>
        /// <returns></returns>
        public async Task<TaskServiceModel> GetTaskAsync(
            string workspaceId,
            string taskId,
            string annotatorId = null,
            bool isIncludingDocuments = true)
        {
            var filter = new BsonDocument
            {
                { nameof(TaskDataModel.IsDeleted), false },
                { "_id", new ObjectId(taskId) },
                { nameof(TaskDataModel.WorkspaceId), new ObjectId(workspaceId) }
            };

            var pipeline = new List<BsonDocument>
            {
                new BsonDocument("$match", filter)
            };

            AddCaculateTaskStatusStep(pipeline);

            if (!isIncludingDocuments)
                pipeline.Add(new BsonDocument("$project", new BsonDocument($"{nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.Documents)}", false)));

            if (!string.IsNullOrWhiteSpace(annotatorId))
                pipeline.Add(new BsonDocument("$addFields", new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$filter", new BsonDocument
                {
                    { "input", $"${nameof(TaskDataModel.Annotators)}" },
                    { "as", "annotator" },
                    { "cond", new BsonDocument("$eq", new BsonArray { $"$$annotator.{nameof(TaskAnnotatorDataModel.AnnotatorId)}", new ObjectId(annotatorId) })}
                }))));

            pipeline.Add(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", CollectionNames.User },
                { "localField", $"{nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.AnnotatorId)}" },
                { "foreignField", "_id" },
                { "as", nameof(TaskDocumentResult.AnnotatorObjects) }
            }));

            if (isIncludingDocuments)
                pipeline.Add(new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", CollectionNames.Document },
                    { "localField", $"{nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.Documents)}.{nameof(TaskDocumentDataModel.DocumentId)}" },
                    { "foreignField", "_id" },
                    { "as", nameof(TaskDocumentResult.DocumentObjects) }
                }));

            pipeline.Add(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", CollectionNames.Workspace },
                { "localField", nameof(TaskDataModel.WorkspaceId) },
                { "foreignField", "_id" },
                { "as", nameof(TaskDocumentResult.WorkspaceObjects) }
            }));

            pipeline.Add(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", CollectionNames.User },
                { "localField", $"{nameof(TaskDataModel.InspectorIds)}" },
                { "foreignField", "_id" },
                { "as", nameof(TaskDocumentResult.InspectorObjects) }
            }));

            pipeline.Add(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", CollectionNames.User },
                { "localField", $"{nameof(TaskDataModel.AcceptorIds)}" },
                { "foreignField", "_id" },
                { "as", nameof(TaskDocumentResult.AcceptorObjects) }
            }));

            var tasks = await context.Tasks.Aggregate<TaskDocumentResult>(pipeline).ToListAsync();

            if (tasks == null || tasks.Count == 0)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.BadRequest);

            var task = tasks.First();

            return new TaskServiceModel
            {
                Id = task.Id.ToString(),
                Name = task.Name,
                Overlap = task.Overlap,
                Workspace = task.WorkspaceObjects.Select(w => new WorkspaceServiceModel
                {
                    Id = w.Id.ToString(),
                    Name = w.Name,
                    Type = new WorkspaceTypeServiceModel
                    {
                        Id = w.Type.ToString()
                    }
                }).FirstOrDefault(),
                ExpectedDueAt = task.ExpectedDueAt,
                ActualDueAt = task.ActualDueAt,
                CreatedAt = task.CreatedAt,
                IsAutoApproved = task.IsAutoApproved,
                IsAutoMerged = task.IsAutoMerged,
                Annotators = task.Annotators.Select(a =>
                {
                    var user = task.AnnotatorObjects.First(ao => ao.Id == a.AnnotatorId);
                    return new TaskAnnotatorServiceModel
                    {
                        Annotator = new UserServiceModel
                        {
                            Id = user.Id.ToString(),
                            Name = user.Name,
                            Roles = user.Roles
                        },
                        IsManager = a.IsManager,
                        TaskDocuments = a.Documents?.Select(td => new { TaskDocument = td, Document = task.DocumentObjects.First(d => d.Id == td.DocumentId) }).Select(d => new TaskDocumentServiceModel
                        {
                            AnnotatorId = a.AnnotatorId.ToString(),
                            Document = new DocumentServiceModel
                            {
                                Id = d.Document.Id.ToString(),
                                Name = d.Document.Name,
                                UploadedAt = d.Document.UploadedAt
                            },
                            Status = d.TaskDocument.Status,
                            LatestResultSavedAt = d.TaskDocument.Results.Any() ? d.TaskDocument.Results.Max(r => r.CreatedAt) : task.CreatedAt
                        }),
                    };
                }),
                Status = task.Status,
                CreatedBy = task.AnnotatorObjects.Where(u => u.Id == task.CreatedBy).Select(u => new UserServiceModel
                {
                    Id = u.Id.ToString(),
                    Name = u.Name
                }).FirstOrDefault(),
                DictionaryIds = task.DictionaryIds?.Select(m => m.ToString()),
                TaskType = task.TaskType?.ToString(),
                Inspectors = task.InspectorObjects.Select(m => new UserServiceModel
                {
                    Id = m.Id.ToString(),
                    Name = m.Name
                }),
                Acceptors = task.AcceptorObjects.Select(m => new UserServiceModel
                {
                    Id = m.Id.ToString(),
                    Name = m.Name
                })
            };
        }
    }
}
