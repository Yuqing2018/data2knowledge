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
using System.Text.RegularExpressions;

namespace MusicKG.Service.Implementations
{
    public partial class TaskService : ITaskService
    {
        /// <summary>
        /// Get Tasks.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="annotatorId">Annotator Id.</param>
        /// <param name="from">From.</param>
        /// <param name="size">Size.</param>
        /// <returns></returns>
        public async Task<Tuple<long, IEnumerable<TaskServiceModel>>> GetTasksAsync(
            string workspaceId,
            string annotatorId,
            string keyword,
            IEnumerable<TaskStatusEnum> statuses,
            int @from,
            int? size,
            string taskType = null)
        {
            var filter = new BsonDocument
            {
                { nameof(TaskDataModel.IsDeleted), false }
            };

            if (!string.IsNullOrEmpty(workspaceId))
                filter.Add(nameof(TaskDataModel.WorkspaceId), new ObjectId(workspaceId));

            if (!string.IsNullOrEmpty(keyword))
                filter.Add(nameof(TaskDataModel.Name), new Regex(keyword));

            if (!string.IsNullOrEmpty(annotatorId))
            {
                filter.Add(nameof(TaskDataModel.Annotators), new BsonDocument("$elemMatch", new BsonDocument
                {
                    { nameof(TaskAnnotatorDataModel.AnnotatorId), new ObjectId(annotatorId) },
                    { nameof(TaskAnnotatorDataModel.Documents), new BsonDocument("$not", new BsonDocument("$size", 0)) }
                }));
            }

            if (!string.IsNullOrWhiteSpace(taskType))
            {
                filter.Add(nameof(TaskDataModel.TaskType), new ObjectId(taskType));
            }

            if (statuses != null && statuses.Count() > 0)
            {
                var statusesToFilter = statuses.Intersect(new[] { TaskStatusEnum.KnowledgeMerged }).ToList();
                var or = new BsonArray();

                foreach (var status in Enum.GetValues(typeof(TaskStatusEnum)).Cast<TaskStatusEnum>())
                    if (statuses.Contains(status))
                        or.Add(GetFilterByStatus(status));

                filter.Add("$or", or);
            }

            var totalCount = await context.Tasks.CountDocumentsAsync(filter);

            var pipeline = new List<BsonDocument>
            {
                new BsonDocument("$match", filter),
                new BsonDocument("$sort", new BsonDocument(nameof(TaskDataModel.CreatedAt), -1))
            };

            if (from > 0)
                pipeline.Add(new BsonDocument("$skip", from));

            if (size.HasValue)
                pipeline.Add(new BsonDocument("$limit", size));

            AddCaculateTaskStatusStep(pipeline);

            pipeline.Add(new BsonDocument("$project", new BsonDocument($"{nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.Documents)}", false)));

            pipeline.Add(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", CollectionNames.User },
                { "localField", $"{nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.AnnotatorId)}" },
                { "foreignField", "_id" },
                { "as", nameof(TaskDocumentResult.AnnotatorObjects) }
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
                { "from", CollectionNames.WorkspaceType },
                { "localField", $"{nameof(TaskDocumentResult.WorkspaceObjects)}.{nameof(WorkspaceDataModel.Type)}" },
                { "foreignField", "_id" },
                { "as", nameof(TaskDocumentResult.WorkspaceTypeObjects) }
            }));

            var tasks = await context.Tasks.Aggregate<TaskDocumentResult>(pipeline).ToListAsync();

            return new Tuple<long, IEnumerable<TaskServiceModel>>(totalCount, tasks.Select(t => new TaskServiceModel
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                TaskType = t.TaskType?.ToString(),
                Overlap = t.Overlap,
                Workspace = t.WorkspaceObjects.Select(w => new WorkspaceServiceModel
                {
                    Id = w.Id.ToString(),
                    Name = w.Name,
                    Type = new WorkspaceTypeServiceModel
                    {
                        Id = w.Type.ToString(),
                        Name = t.WorkspaceTypeObjects.FirstOrDefault(wt => wt.Id == w.Type)?.Name
                    }
                }).FirstOrDefault(),
                ExpectedDueAt = t.ExpectedDueAt,
                Annotators = t.AnnotatorObjects.Select(a => new TaskAnnotatorServiceModel
                {
                    Annotator = new UserServiceModel
                    {
                        Id = a.Id.ToString(),
                        Name = a.Name,
                        Roles = a.Roles
                    }
                }),
                Status = t.Status,
                CreatedBy = t.AnnotatorObjects.Where(u => u.Id == t.CreatedBy).Select(u => new UserServiceModel
                {
                    Id = u.Id.ToString(),
                    Name = u.Name
                }).FirstOrDefault(),
                CreatedAt = t.CreatedAt
            }));
        }

        private void AddCaculateTaskStatusStep(List<BsonDocument> pipeline)
        {
            pipeline.Add(new BsonDocument("$addFields", new BsonDocument
            {
                { "IsAllDocumentsAnnotatedByManager", GetIsAllDocumentsAnnotatedCriteria(true) },
                { "IsAllDocumentsAnnotatedByAnnotators", GetIsAllDocumentsAnnotatedCriteria(false) }
            }));

            pipeline.Add(new BsonDocument("$addFields", new BsonDocument(nameof(TaskDataModel.Status), new BsonDocument("$switch", new BsonDocument
            {
                {
                    "branches",
                    new BsonArray
                    {
                        new BsonDocument
                        {
                            { "case", GetCaseCriteria(new[] { TaskStatusEnum.Created, TaskStatusEnum.Rejected }, true, true) },
                            { "then", TaskStatusEnum.ConflictResolved }
                        },
                        new BsonDocument
                        {
                            { "case", GetCaseCriteria(new[] { TaskStatusEnum.Accepted }, false, true) },
                            { "then", TaskStatusEnum.ConflictResolved }
                        },
                        new BsonDocument
                        {
                            { "case", GetCaseCriteria(new[] { TaskStatusEnum.Created, TaskStatusEnum.Rejected }, true, false, true) },
                            { "then", TaskStatusEnum.ConflictResolved }
                        },
                        new BsonDocument
                        {
                            { "case", GetCaseCriteria(new[] { TaskStatusEnum.Created, TaskStatusEnum.Rejected }, false, false) },
                            { "then", TaskStatusEnum.Submitted }
                        },
                        new BsonDocument
                        {
                            { "case", GetCaseCriteria(new[] { TaskStatusEnum.Created, TaskStatusEnum.Rejected }, true, false) },
                            { "then", TaskStatusEnum.Accepted}
                        }
                    }
                },
                { "default", $"${nameof(TaskDataModel.Status)}" }
            }))));
        }

        private BsonDocument GetIsAllDocumentsAnnotatedCriteria(bool isManager)
        {
            return new BsonDocument("$eq", new BsonArray {
                new BsonDocument("$size", new BsonDocument("$filter", new BsonDocument
                {
                    { "input", $"${nameof(TaskDataModel.Annotators)}" },
                    { "as", "annotator" },
                    {
                        "cond",
                        new BsonDocument("$and", new BsonArray
                        {
                            new BsonDocument("$eq", new BsonArray { $"$$annotator.{nameof(TaskAnnotatorDataModel.IsManager)}", isManager }),
                            new BsonDocument("$ne", new BsonArray { new BsonDocument("$size", new BsonDocument("$filter", new BsonDocument
                            {
                                { "input", $"$$annotator.{nameof(TaskAnnotatorDataModel.Documents)}" },
                                { "as", "document" },
                                { "cond", new BsonDocument("$eq", new BsonArray { $"$$document.{nameof(TaskDocumentDataModel.Status)}", TaskDocumentStatusEnum.Assigned }) }
                            })), 0 })
                        })
                    }
                })),
                0
            });
        }

        private BsonDocument GetCaseCriteria(IEnumerable<TaskStatusEnum> statuses, bool isAutoApporved, bool isManager, bool? isAutoMerge = null)
        {
            var and = new BsonArray
            {
                new BsonDocument("$in", new BsonArray { $"${nameof(TaskDataModel.Status)}", new BsonArray(statuses) }),
                new BsonDocument("$eq", new BsonArray { $"${nameof(TaskDataModel.IsAutoApproved)}", isAutoApporved }),
                new BsonDocument("$eq", new BsonArray { isManager ? "$IsAllDocumentsAnnotatedByManager" : "$IsAllDocumentsAnnotatedByAnnotators", true })
            };

            if (isAutoMerge != null)
            {
                and.Add(new BsonDocument("$eq", new BsonArray { $"${nameof(TaskDataModel.IsAutoMerged)}", isAutoMerge.Value }));
            }

            return new BsonDocument("$and", and);
        }

        private BsonDocument GetFilterByStatus(TaskStatusEnum status)
        {
            var annotatorFilter = new BsonDocument("$elemMatch", new BsonDocument
            {
                { nameof(TaskAnnotatorDataModel.IsManager), false },
                { nameof(TaskAnnotatorDataModel.Documents), new BsonDocument("$elemMatch", new BsonDocument(nameof(TaskDataModel.Status), TaskDocumentStatusEnum.Assigned)) }
            });

            var managerFilter = new BsonDocument("$elemMatch", new BsonDocument
            {
                { nameof(TaskAnnotatorDataModel.IsManager), true },
                { nameof(TaskAnnotatorDataModel.Documents), new BsonDocument("$elemMatch", new BsonDocument(nameof(TaskDataModel.Status), TaskDocumentStatusEnum.Assigned)) }
            });

            switch (status)
            {
                case TaskStatusEnum.Created:
                    return new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(nameof(TaskDataModel.Status), TaskStatusEnum.Created),
                        new BsonDocument(nameof(TaskDataModel.Annotators), annotatorFilter),
                        new BsonDocument(nameof(TaskDataModel.Annotators), managerFilter)
                    });
                case TaskStatusEnum.Rejected:
                    return new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(nameof(TaskDataModel.Status), TaskStatusEnum.Rejected),
                        new BsonDocument(nameof(TaskDataModel.Annotators), annotatorFilter),
                        new BsonDocument(nameof(TaskDataModel.Annotators), managerFilter)
                    });
                case TaskStatusEnum.Submitted:
                    return new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(nameof(TaskDataModel.IsAutoApproved), false),
                        new BsonDocument(nameof(TaskDataModel.Status), new BsonDocument("$in", new BsonArray { TaskStatusEnum.Created, TaskStatusEnum.Rejected })),
                        new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$not", annotatorFilter)),
                        new BsonDocument(nameof(TaskDataModel.Annotators), managerFilter)
                    });
                case TaskStatusEnum.Accepted:
                    return new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument("$or", new BsonArray
                        {
                            new BsonDocument("$and", new BsonArray
                            {
                                new BsonDocument(nameof(TaskDataModel.IsAutoApproved), true),
                                new BsonDocument(nameof(TaskDataModel.Status), new BsonDocument("$in", new BsonArray { TaskStatusEnum.Created, TaskStatusEnum.Rejected }))
                            }),
                            new BsonDocument("$and", new BsonArray
                            {
                                new BsonDocument(nameof(TaskDataModel.IsAutoApproved), false),
                                new BsonDocument(nameof(TaskDataModel.Status), TaskStatusEnum.Accepted)
                            })
                        }),
                        new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$not", annotatorFilter)),
                        new BsonDocument(nameof(TaskDataModel.Annotators), managerFilter)
                    });
                case TaskStatusEnum.ConflictResolved:
                    return new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(nameof(TaskDataModel.Status), new BsonDocument("$in", new BsonArray { TaskStatusEnum.Created, TaskStatusEnum.Rejected, TaskStatusEnum.Accepted })),
                        new BsonDocument("$or", new BsonArray
                        {
                            new BsonDocument("$and", new BsonArray
                            {
                                new BsonDocument(nameof(TaskDataModel.IsAutoMerged), false),
                                new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$not", annotatorFilter)),
                                new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$not", managerFilter))
                            }),
                            new BsonDocument("$and", new BsonArray
                            {
                                new BsonDocument(nameof(TaskDataModel.IsAutoMerged), true),
                                new BsonDocument(nameof(TaskDataModel.Annotators), new BsonDocument("$not", annotatorFilter))
                            })
                        })
                    });
                case TaskStatusEnum.KnowledgeMerged:
                    return new BsonDocument(nameof(TaskDataModel.Status), TaskStatusEnum.KnowledgeMerged);
                default:
                    ErrorHelper.ThrowException(MusicKGMessages.TaskStatusUnknownMessage);
                    return null;
            }
        }
    }
}
