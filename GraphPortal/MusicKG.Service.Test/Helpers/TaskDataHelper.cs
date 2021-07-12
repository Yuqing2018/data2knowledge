using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service.Test.Helpers
{
    public static class TaskDataHelper
    {
        public static ObjectId[] TaskTypes = new ObjectId[]
        {
            ObjectId.GenerateNewId(),
            ObjectId.GenerateNewId(),
            ObjectId.GenerateNewId()
        };

        public static TaskStatusEnum[][] TaskStatuses = new[]
        {
            new[] { TaskStatusEnum.Created, TaskStatusEnum.Created },
            new[] { TaskStatusEnum.Created, TaskStatusEnum.Created },
            new[] { TaskStatusEnum.Submitted, TaskStatusEnum.Created },

            new[] { TaskStatusEnum.Created, TaskStatusEnum.Created },
            new[] { TaskStatusEnum.Created, TaskStatusEnum.Created },
            new[] { TaskStatusEnum.Submitted, TaskStatusEnum.Created },

            new[] { TaskStatusEnum.Created, TaskStatusEnum.Created },
            new[] { TaskStatusEnum.Created, TaskStatusEnum.Created },
            new[] { TaskStatusEnum.Accepted, TaskStatusEnum.Created },

            new[] { TaskStatusEnum.Rejected, TaskStatusEnum.Rejected },
            new[] { TaskStatusEnum.Rejected, TaskStatusEnum.Rejected },
            new[] { TaskStatusEnum.Submitted, TaskStatusEnum.Rejected },

            new[] { TaskStatusEnum.Rejected, TaskStatusEnum.Rejected },
            new[] { TaskStatusEnum.Rejected, TaskStatusEnum.Rejected },
            new[] { TaskStatusEnum.Accepted, TaskStatusEnum.Rejected },

            new[] { TaskStatusEnum.Accepted, TaskStatusEnum.Accepted },
            new[] { TaskStatusEnum.Accepted, TaskStatusEnum.Accepted },
            new[] { TaskStatusEnum.ConflictResolved, TaskStatusEnum.Accepted },

            new[] { TaskStatusEnum.KnowledgeMerged, TaskStatusEnum.KnowledgeMerged }
        };

        private static readonly TaskDocumentStatusEnum[][][] annotatorTaskDocumentStatuses = new[]
        {
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },

            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },

            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },

            new[]
            {
                new[] {TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },

            new[]
            {
                new[] {TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },

            new[]
            {
                new[] { TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Assigned },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },
            new[]
            {
                new[] {TaskDocumentStatusEnum.Assigned, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },
            new[]
            {
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            },

            new[]
            {
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated },
                new[] { TaskDocumentStatusEnum.Annotated, TaskDocumentStatusEnum.Annotated }
            }
        };

        private static readonly int[][] annotatorTaskDocumentCounts = new[]
        {
            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },

            new[] { 100, 100, 0 },
            new[] { 100, 0, 100 },
            new[] { 100, 100, 0 },

            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },

            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },

            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },

            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },
            new[] { 100, 100, 100 },

            new[] { 100, 100, 100 }
        };

        public static bool[] TaskIsAutoApproved = new[]
        {
            false,
            false,
            false,

            false,
            false,
            false,

            true,
            true,
            true,

            false,
            false,
            false,

            true,
            true,
            true,

            false,
            false,
            false,

            false,
        };

        public static bool[] TaskIsAutoMerged = new[]
        {
            false,
            false,
            false,

            false,
            false,
            false,

            false,
            false,
            false,

            false,
            false,
            false,

            false,
            false,
            false,

            false,
            false,
            false,

            false,
        };

        public static IEnumerable<WorkspaceTypeDataModel> WorkspaceTypes = Enumerable.Range(1, 3).Select(i => new WorkspaceTypeDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = $"workspace type #{i}"
        }).ToList();

        public static IEnumerable<WorkspaceDataModel> Workspaces = WorkspaceTypes.Select((wt, i) => new WorkspaceDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = $"workspace #{i}",
            Type = wt.Id
        }).ToList();

        public static IEnumerable<DictionaryDataModel> Dictionaries = Enumerable.Range(1, 20).Select(i => new DictionaryDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = $"dictionary #{i}",
            EntityId = ObjectId.GenerateNewId(i + 1),
            WorkspaceId = Workspaces.FirstOrDefault().Id,
            Vocabularies = new List<string>() { $"vocabularies{i}" }
        }).ToList();

        public static IEnumerable<UserDataModel> Users = Enumerable.Range(1, annotatorTaskDocumentStatuses[0].Length).Select(i => new UserDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = i == 1 ? $"manager #{i}" : $"annotator #{i}",
            Roles = i == 1 ? new List<UserRoleEnum> { UserRoleEnum.Manager, UserRoleEnum.Inspector, UserRoleEnum.Acceptor } : new List<UserRoleEnum> { UserRoleEnum.Annotator }
        }).ToList();

        public static IEnumerable<TaskDataModelTest> Tasks = Workspaces.SelectMany(w => Enumerable.Range(0, TaskStatuses.Length).Select(i => new TaskDataModelTest
        {
            ExpectedTaskStatus = TaskStatuses[i][0],
            Id = ObjectId.GenerateNewId(),
            Name = $"task #{i}",
            TaskType = i % TaskTypes.Length == TaskTypes.Length - 1 ? (ObjectId?)null : TaskTypes[(i % TaskTypes.Length)],
            Status = TaskStatuses[i][1],
            WorkspaceId = w.Id,
            CreatedAt = DateTime.UtcNow.AddMinutes(i),
            CreatedBy = Users.First().Id,
            ExpectedDueAt = DateTime.UtcNow.AddDays(i),
            Overlap = 20,
            IsAutoApproved = TaskIsAutoApproved[i],
            IsAutoMerged = TaskIsAutoMerged[i],
            Annotators = Users.Select((u, j) => new TaskAnnotatorDataModel
            {
                AnnotatorId = u.Id,
                IsManager = u.Roles.Contains(UserRoleEnum.Manager),
                Documents = Enumerable.Range(0, annotatorTaskDocumentCounts[i][j]).Select(k => new TaskDocumentDataModel
                {
                    DocumentId = ObjectId.GenerateNewId(),
                    Status = annotatorTaskDocumentStatuses[i][j][k % 2],
                    Results = new List<TaskDocumentResultDataModel>
                        {
                            new TaskDocumentResultDataModel
                            {
                                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                                ResultDocumentId = ObjectId.GenerateNewId(),
                                CreatedAt = DateTime.UtcNow.AddMinutes(i * 100 + j * 10 + k)
                            },
                            new TaskDocumentResultDataModel
                            {
                                ResultType = TaskDocumentResultTypeEnum.ForGraphMerging,
                                ResultDocumentId = ObjectId.GenerateNewId(),
                                CreatedAt = DateTime.UtcNow.AddMinutes(i * 100 + j * 10 + k)
                            }
                        }
                }).ToList()
            }).ToList(),
            DictionaryIds = Dictionaries.Select(m => m.Id).ToList(),
            InspectorIds = new List<ObjectId> { Users.First().Id },
            AcceptorIds = new List<ObjectId> { Users.First().Id },
        })).ToList();

        public static IEnumerable<DocumentDataModel> Documents = Tasks.SelectMany(t => t.Annotators).SelectMany(a => a.Documents).SelectMany(d =>
        {
            return d.Results.Select(r => new DocumentDataModel
            {
                Id = r.ResultDocumentId,
                Name = $"result document {r.ResultDocumentId}",
                Status = DocumentStatusEnum.Uploaded,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = Users.First().Id,
                ContentType = "application/json"
            }).ToList().Append(new DocumentDataModel
            {
                Id = d.DocumentId,
                Name = $"document {d.DocumentId}",
                Status = DocumentStatusEnum.Assigned,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = Users.First().Id,
                ContentType = "application/json"
            });
        }).ToList();

        public static async Task<(
            IEnumerable<WorkspaceTypeDataModel>,
            IEnumerable<WorkspaceDataModel>,
            IEnumerable<UserDataModel>,
            IEnumerable<TaskDataModelTest>,
            IEnumerable<DocumentDataModel>,
            IEnumerable<DictionaryDataModel>,
            TaskStatusEnum[][])>
        PrepareTasksDataWithAllStatus(IMusicKGContext context)
        {
            await context.WorkspaceTypes.InsertManyAsync(WorkspaceTypes);
            await context.Workspaces.InsertManyAsync(Workspaces);
            await context.Users.InsertManyAsync(Users);
            await context.Tasks.InsertManyAsync(Tasks);
            await context.Documents.InsertManyAsync(Documents);
            await context.Dictionaries.InsertManyAsync(Dictionaries);

            return (WorkspaceTypes, Workspaces, Users, Tasks, Documents, Dictionaries, TaskStatuses);
        }
    }
}
