using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.Service.Test.Fixtures;
using MusicKG.Service.Test.Helpers;
using Xunit;

namespace MusicKG.Service.Test
{
    public partial class TaskServiceTest : IDisposable, IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly string databaseName;

        public TaskServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            databaseName = RandomStringHelper.RandomString(10);
            context = new MusicKGContext(mongoFixture.Context.Client, databaseName);
        }

        #region ApproveTask

        #region Positive

        [Fact]
        public async Task ApproveTask()
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = await TaskDataHelper.PrepareTasksDataWithAllStatus(context);
            var task = tasks.Where(t => t.ExpectedTaskStatus == TaskStatusEnum.Submitted).First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.ApproveTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString());

            var result = await context.Tasks.Find(m => m.Id == task.Id).FirstOrDefaultAsync();

            Assert.Equal(TaskStatusEnum.Accepted, result.Status);
            Assert.All(result.Annotators, a =>
            {
                Assert.All(a.Documents, d =>
                {
                    Assert.Equal(
                        task.Annotators.Where(an => an.AnnotatorId == a.AnnotatorId).First()
                            .Documents.Where(doc => doc.DocumentId == d.DocumentId).First().Status,
                        d.Status);
                });
            });
        }

        #endregion

        #endregion

        #region RejectTask

        #region Positive

        [Fact]
        public async Task RejectTask()
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = await TaskDataHelper.PrepareTasksDataWithAllStatus(context);
            var task = tasks.Where(t => t.ExpectedTaskStatus == TaskStatusEnum.Submitted).First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.RejectTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString());

            var result = await context.Tasks.Find(m => m.Id == task.Id).FirstOrDefaultAsync();

            Assert.Equal(TaskStatusEnum.Rejected, result.Status);
            Assert.All(result.Annotators, a =>
            {
                Assert.All(a.Documents, d =>
                {
                    Assert.Equal(TaskDocumentStatusEnum.Assigned, d.Status);
                });
            });
        }

        [Fact]
        public async Task RejectTaskForAnnotatorId()
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = await TaskDataHelper.PrepareTasksDataWithAllStatus(context);
            var task = tasks.Where(t => t.ExpectedTaskStatus == TaskStatusEnum.Submitted).First();

            var annotatorId = task.Annotators.First().AnnotatorId;

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.RejectTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString(), annotatorId.ToString());

            var result = await context.Tasks.Find(m => m.Id == task.Id).FirstOrDefaultAsync();

            Assert.Equal(TaskStatusEnum.Rejected, result.Status);
            Assert.All(result.Annotators.Where(r => r.AnnotatorId == annotatorId), a =>
            {
                Assert.All(a.Documents, d =>
                {
                    Assert.Equal(TaskDocumentStatusEnum.Assigned, d.Status);
                });
            });

            Assert.All(result.Annotators.Where(r => r.AnnotatorId != annotatorId), a =>
            {
                Assert.All(a.Documents, d =>
                {
                    Assert.Equal(TaskDocumentStatusEnum.Annotated, d.Status);
                });
            });
        }

        #endregion

        #endregion

        public static async Task<(
            IEnumerable<WorkspaceDataModel>,
            IEnumerable<UserDataModel>,
            IEnumerable<TaskDataModel>,
            IEnumerable<DocumentDataModel>)>
            PrepareTasksData(IMusicKGContext context)
        {
            var workspaces = Enumerable.Range(1, 1).Select(i => new WorkspaceDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"workspace #{i}"
            }).ToList();

            var users = Enumerable.Range(1, 3).Select(i => new UserDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = i == 1 ? $"manager #{i}" : $"annotator #{i}",
                Roles = i == 1 ? new List<UserRoleEnum> { UserRoleEnum.Manager } : new List<UserRoleEnum> { UserRoleEnum.Annotator }
            }).ToList();

            var tasks = workspaces.SelectMany(w => Enumerable.Range(1, 3).Select(i => new TaskDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"task #{i}",
                Status = TaskStatusEnum.Created,
                WorkspaceId = w.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = users.First().Id,
                ExpectedDueAt = DateTime.UtcNow.AddDays(i),
                Overlap = 20,
                IsAutoApproved = true,
                IsAutoMerged = true,
                Annotators = users.Select(u => new TaskAnnotatorDataModel
                {
                    AnnotatorId = u.Id,
                    IsManager = u.Roles.Contains(UserRoleEnum.Manager),
                    Documents = Enumerable.Range(1, 10).Select(k => new TaskDocumentDataModel
                    {
                        DocumentId = ObjectId.GenerateNewId(),
                        Status = TaskDocumentStatusEnum.Assigned,
                        Results = new List<TaskDocumentResultDataModel>
                        {
                            new TaskDocumentResultDataModel
                            {
                                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                                ResultDocumentId = ObjectId.GenerateNewId(),
                                CreatedAt = DateTime.UtcNow
                            },
                            new TaskDocumentResultDataModel
                            {
                                ResultType = TaskDocumentResultTypeEnum.ForGraphMerging,
                                ResultDocumentId = ObjectId.GenerateNewId(),
                                CreatedAt = DateTime.UtcNow
                            }
                        }
                    }).ToList()
                }).ToList()
            })).ToList();

            var documents = tasks.SelectMany(t => t.Annotators).SelectMany(a => a.Documents).SelectMany(d =>
            {
                return d.Results.Select(r => new DocumentDataModel
                {
                    Id = r.ResultDocumentId,
                    Name = $"result document {r.ResultDocumentId}",
                    Status = DocumentStatusEnum.Uploaded,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = users.First().Id,
                    ContentType = "application/json"
                }).ToList().Append(new DocumentDataModel
                {
                    Id = d.DocumentId,
                    Name = $"document {d.DocumentId}",
                    Status = DocumentStatusEnum.Assigned,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = users.First().Id,
                    ContentType = "application/json"
                });
            }).ToList();

            await context.Workspaces.InsertManyAsync(workspaces);
            await context.Users.InsertManyAsync(users);
            await context.Tasks.InsertManyAsync(tasks);
            await context.Documents.InsertManyAsync(documents);

            return (workspaces, users, tasks, documents);
        }

        public void Dispose()
        {
            context.Client.DropDatabase(databaseName);
        }
    }
}
