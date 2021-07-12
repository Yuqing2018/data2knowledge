using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Moq;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using Xunit;
using Microsoft.AspNetCore.Http;
using static System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MusicKG.Service.Helpers;

namespace MusicKG.WebApi.Test.Controllers
{
    public partial class TaskControllerTest
    {
        #region TaskController

        #region Get

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetTasks(bool isManager)
        {
            var (workspaces, users, documents, tasks, dictionaryIds) = PrepareTaskTestData();

            var workspace = workspaces.First();
            var user = users.Where(u => u.Roles.Contains(isManager ? UserRoleEnum.Manager : UserRoleEnum.Annotator)).First();
            var tasksExpected = tasks.Where(t => t.Workspace.Id.Equals(workspace.Id)).ToList();

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(m => m.GetTasksAsync(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<IEnumerable<TaskStatusEnum>>(), It.IsAny<int>(), null, null)).Returns(
                FromResult(new Tuple<long, IEnumerable<TaskServiceModel>>(tasksExpected.Count(), tasksExpected)));

            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, string.Join(',', user.Roles))
            });

            var controller = new TaskController(taskServiceMock.Object, null, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            var result = await controller.GetTasks(workspace.Id, null, null, null, 0, null);

            Assert.NotNull(result);

            Assert.Equal(tasksExpected.Count(), result.TotalCount);
            Assert.Equal(tasksExpected.Count(), result.Count);
            Assert.Equal(0, result.From);

            var tasksReturned = result.Items;
            Assert.NotNull(tasksReturned);
            Assert.Equal(tasksExpected.Count(), tasksReturned.Count());
            Assert.All(tasksReturned, t =>
            {
                var taskExpected = tasksExpected.Where(te => te.Id.Equals(t.Id)).First();
                Assert.Equal(taskExpected.Name, t.Name);
                Assert.Equal(taskExpected.ExpectedDueAt.ToString(), t.ExpectedDueAt.ToString());
                Assert.Equal(taskExpected.Overlap, t.Overlap);
                Assert.Equal(taskExpected.Status, t.Status);
                Assert.Equal(taskExpected.TaskType, t.TaskType);
                Assert.All(t.Annotators, a =>
                {
                    var userExpected = users.Where(u => u.Id.Equals(a.Id)).First();
                    Assert.Equal(userExpected.Name, a.Name);
                    Assert.Equal(userExpected.Roles, a.Roles);
                });
                Assert.Equal(workspace.Id, t.Workspace.Id);
                Assert.Equal(workspace.Name, t.Workspace.Name);
                Assert.Equal(taskExpected.CreatedBy.Id, t.CreatedBy.Id);
                Assert.Equal(taskExpected.CreatedBy.Name, t.CreatedBy.Name);
                Assert.Equal(taskExpected.Workspace.Type.Id, t.Workspace.Type.Id);
                Assert.Equal(taskExpected.Workspace.Type.Name, t.Workspace.Type.Name);
            });
        }

        [Fact]
        public async Task GetTask()
        {
            var (workspaces, users, documents, tasks, dictionaryIds) = PrepareTaskTestData();

            var task = tasks.First();

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(m => m.GetTaskAsync(It.IsAny<string>(), It.IsAny<string>(), null, false))
                .Returns(FromResult(task));

            var controller = new TaskController(taskServiceMock.Object, null, null);

            var taskReturned = await controller.GetTask(task.Workspace.Id, task.Id);

            Assert.Equal(task.Name, taskReturned.Name);
            Assert.Equal(task.TaskType, taskReturned.TaskType);
            Assert.Equal(task.ExpectedDueAt.ToString(), taskReturned.ExpectedDueAt.ToString());
            Assert.Equal(task.Overlap, taskReturned.Overlap);
            Assert.Equal(task.Status, taskReturned.Status);
            Assert.All(taskReturned.Annotators, a =>
            {
                var userExpected = users.Where(u => u.Id.Equals(a.Id)).First();
                Assert.Equal(userExpected.Name, a.Name);
                Assert.Equal(userExpected.Roles, a.Roles);
            });
            Assert.Equal(task.CreatedBy.Id, taskReturned.CreatedBy.Id);
            Assert.Equal(task.CreatedBy.Name, taskReturned.CreatedBy.Name);

            for (int i = 0; i < dictionaryIds.Count(); i++)
            {
                var expected = dictionaryIds.ToList()[i].ToString();
                var actual = task.DictionaryIds.ToList()[i].ToString();
                Assert.Equal(expected, actual);
            }

            var workspace = workspaces.First(w => w.Id.Equals(task.Workspace.Id));
            Assert.Equal(workspace.Id, taskReturned.Workspace.Id);
            Assert.Equal(workspace.Name, taskReturned.Workspace.Name);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create()
        {
            var taskId = ObjectId.GenerateNewId().ToString();

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(m => m.CreateTaskAsync(It.IsAny<TaskCreateServiceModel>()))
                .Returns(FromResult(taskId));

            var controller = new TaskController(taskServiceMock.Object, null, null);

            var createdTaskId = await controller.CreateTask(ObjectId.GenerateNewId().ToString(), new TaskCreateBindingModel()
            {
                AnnotatorIds = new List<string>() { "aa", "bb", "cc" },
                DocumentIds = new List<string>(),
                ExpectedDueAt = DateTime.UtcNow.AddSeconds(1),
                Name = "Test1",
                Overlap = 22,
                IsAutoApproved = true,
                IsAutoMerged = true
            });

            Assert.Equal(taskId, createdTaskId);
        }

        [Fact]
        public async Task CreateTaskWithInvalidExpectedDueAt()
        {
            var taskId = ObjectId.GenerateNewId().ToString();

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(m => m.CreateTaskAsync(It.IsAny<TaskCreateServiceModel>()))
                .Returns(FromResult(taskId));

            var controller = new TaskController(taskServiceMock.Object, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => controller.CreateTask(ObjectId.GenerateNewId().ToString(), new TaskCreateBindingModel()
            {
                AnnotatorIds = new List<string>() { "aa", "bb", "cc" },
                DocumentIds = new List<string>(),
                ExpectedDueAt = DateTime.UtcNow.AddSeconds(-1),
                Name = "Test1",
                Overlap = 22,
                IsAutoApproved = true,
                IsAutoMerged = true
            }));

            Assert.Equal(Service.Resources.MusicKGMessages.TaskExpectedDueAtWrongMessage, exception.Message);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update()
        {
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(m => m.UpdateTaskAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<TaskUpdateServiceModel>())).Returns(CompletedTask);

            var controller = new TaskController(taskServiceMock.Object, null, null);

            await controller.UpdateTask(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(),
                new TaskUpdateBindingModel()
                {
                    ExpectedDueAt = DateTime.Now,
                    Name = "Update"
                });

            taskServiceMock.Verify(t => t.UpdateTaskAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskUpdateServiceModel>()), Times.Once);
        }

        #endregion

        #endregion

        #region GetTaskDocuments

        [Theory]
        [InlineData(true, 0, null)]
        [InlineData(false, 0, null)]
        [InlineData(true, 0, 1)]
        [InlineData(false, 0, 1)]
        public async Task GetDocuments(bool isManager, int from, int? size)
        {
            var (workspaces, users, documents, tasks, dictionaryIds) = PrepareTaskTestData();

            var task = tasks.First();
            var workspace = workspaces.First();
            var user = users.Where(u => u.Roles.Contains(isManager ? UserRoleEnum.Manager : UserRoleEnum.Annotator)).First();

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(m => m.GetTaskAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(FromResult(task));

            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, string.Join(',', user.Roles))
            });

            var controller = new TaskController(taskServiceMock.Object, null, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            var result = await controller.GetDocuments(task.Workspace.Id, task.Id, from, size);

            var taskDocumentsTotalExpected = task.Annotators.Where(a => a.Annotator.Id.Equals(user.Id)).SelectMany(a => a.TaskDocuments);
            var taskDocumentsExpected = taskDocumentsTotalExpected.Skip(from).Take(size ?? int.MaxValue);

            Assert.NotNull(result);
            Assert.Equal(taskDocumentsTotalExpected.Count(), result.TotalCount);
            Assert.Equal(taskDocumentsExpected.Count(), result.Count);
            Assert.NotNull(result.Items);
            Assert.Equal(taskDocumentsExpected.Count(), result.Items.Count());
            Assert.All(result.Items, d =>
            {
                var taskDocumentExpected = taskDocumentsExpected.Where(td => td.Document.Id == d.Id).First();
                Assert.NotNull(d);
                Assert.Equal(taskDocumentExpected.Document.Id, d.Id);
                Assert.Equal(taskDocumentExpected.Document.Name, d.Name);
                Assert.Equal(taskDocumentExpected.Document.UploadedAt, d.UploadedAt);
                Assert.Equal(taskDocumentExpected.Status, d.Status);

                if (!isManager)
                    Assert.Equal(taskDocumentExpected.LatestResultSavedAt.ToString(), d.LatestResultSavedAt.ToString());
                else
                    Assert.Equal(task.Annotators.SelectMany(a => a.TaskDocuments.Where(td => td.Document.Id.Equals(d.Id))).Max(td => td.LatestResultSavedAt).ToString(), d.LatestResultSavedAt.ToString());
            });
        }

        #endregion

        private (IEnumerable<WorkspaceServiceModel>, IEnumerable<UserServiceModel>, IEnumerable<DocumentServiceModel>, IEnumerable<TaskServiceModel>, IEnumerable<string>) PrepareTaskTestData()
        {
            var workspaces = Enumerable.Range(1, 3).Select(i => new WorkspaceServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"Workspace {i}",
                Type = new WorkspaceTypeServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = $"Workspace type {i}"
                }
            }).ToList();

            var users = Enumerable.Range(1, 3).Select(i => new UserServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"User {i}",
                Roles = i == 1 ? new[] { UserRoleEnum.Manager } : new[] { UserRoleEnum.Annotator }
            }).ToList();

            var documents = workspaces.SelectMany(w => Enumerable.Range(1, 10).Select(i => new DocumentServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"Document {i} in workspace {w.Id}",
                UploadedAt = DateTime.UtcNow,
                WorkspaceId = w.Id
            })).ToList();

            var dictionaryIds = Enumerable.Range(1, 5).Select(m => ObjectId.GenerateNewId(m).ToString()).ToList();

            var tasks = workspaces.SelectMany(w => Enumerable.Range(1, 3).Select(i => new TaskServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"Task {i}",
                Status = TaskStatusEnum.Created,
                ActualDueAt = DateTime.UtcNow.AddDays(7),
                TaskType = i % 2 == 0 ? ObjectId.GenerateNewId().ToString() : null,
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString()
                },
                CreatedAt = DateTime.UtcNow,
                ExpectedDueAt = DateTime.UtcNow.AddDays(7),
                IsAutoApproved = true,
                IsAutoMerged = true,
                Overlap = 20,
                Workspace = w,
                Annotators = users.Select(u => new TaskAnnotatorServiceModel
                {
                    Annotator = u,
                    IsManager = u.Roles.Contains(UserRoleEnum.Manager),
                    TaskDocuments = documents.Where(d => d.WorkspaceId.Equals(w.Id)).Select(d => new TaskDocumentServiceModel
                    {
                        AnnotatorId = u.Id,
                        Status = TaskDocumentStatusEnum.Assigned,
                        Document = d,
                        LatestResultSavedAt = DateTime.UtcNow
                    })
                }).ToList(),
                DictionaryIds= dictionaryIds
            })).ToList();

            return (workspaces, users, documents, tasks, dictionaryIds);
        }
    }
}
