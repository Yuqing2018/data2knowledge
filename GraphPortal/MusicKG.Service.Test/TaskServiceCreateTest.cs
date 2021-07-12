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
    public partial class TaskServiceCreateTest : IClassFixture<TaskFixture>
    {
        private readonly TaskFixture taskFixture;
        private readonly IMusicKGContext context;

        public TaskServiceCreateTest(TaskFixture taskFixture)
        {
            this.taskFixture = taskFixture;
            context = taskFixture.Context;
        }

        [Theory]
        [InlineData(0, 3, 10, true)]
        [InlineData(25, 1, 10, true)]
        [InlineData(25, 3, 10, false)]
        [InlineData(100, 3, 10, false)]
        public async Task Create(int overlap, int annotatorCount, int documentCount, bool withTaskType)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;

            var workspace = workspaces.First();

            var workflowId = ObjectId.GenerateNewId();
            var birthStepId = ObjectId.GenerateNewId();
            var inspectorId = ObjectId.GenerateNewId();
            var acceptorId = ObjectId.GenerateNewId();

            var documentsToAdd = await PrepareDocumentsToCreateTask(workspace.Id, workflowId, birthStepId);

            var taskCreate = new TaskCreateServiceModel()
            {
                Name = RandomStringHelper.RandomString(10),
                TaskType = withTaskType ? ObjectId.GenerateNewId().ToString() : null,
                AnnotatorIds = users.Take(annotatorCount).Select(u => u.Id.ToString()).ToList(),
                CreateBy = users.First().Id.ToString(),
                DocumentIds = documentsToAdd.Take(documentCount).Select(d => d.Id.ToString()).ToList(),
                ExpectedDueAt = DateTime.UtcNow.AddHours(2),
                Overlap = overlap,
                WorkspaceId = workspace.Id.ToString(),
                IsAutoApproved = true,
                IsAutoMerged = true,
                DictionaryIds = dictonaries.Select(m => m.Id.ToString()).ToList(),
                InspectorIds = withTaskType ? null : new List<string> { inspectorId.ToString() },
                AcceptorIds = withTaskType ? null : new List<string> { acceptorId.ToString() }
            };

            var workflowStepServiceMock = new Mock<IWorkflowStepService>();
            workflowStepServiceMock.Setup(wf => wf
                .GetPreannotationWorkflowStepAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new WorkflowNextStepServiceModel
                {
                    Id = birthStepId.ToString(),
                    ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                    WorkflowId = workflowId.ToString()
                }));

            ITaskService taskService = new TaskService(null, context, null, workflowStepServiceMock.Object, null);

            var createdTaskId = await taskService.CreateTaskAsync(taskCreate);

            var createdTask = context.Tasks.Find(m => m.Id == new ObjectId(createdTaskId)).FirstOrDefault();

            Assert.NotNull(createdTask);
            Assert.Equal(taskCreate.CreateBy, createdTask.CreatedBy.ToString());
            Assert.Equal(taskCreate.Overlap, createdTask.Overlap);
            Assert.Equal(taskCreate.Name, createdTask.Name);
            Assert.Equal(taskCreate.TaskType, createdTask.TaskType?.ToString());
            Assert.Equal(taskCreate.WorkspaceId, createdTask.WorkspaceId.ToString());
            Assert.Equal(TaskStatusEnum.Created, createdTask.Status);
            if (!withTaskType)
            {
                Assert.Equal(taskCreate.InspectorIds.First(), createdTask.InspectorIds.First().ToString());
                Assert.Equal(taskCreate.AcceptorIds.First(), createdTask.AcceptorIds.First().ToString());
            }

            for (int i = 0; i < dictonaries.Count(); i++)
            {
                var expected = dictonaries.ToList()[i].Id.ToString();
                var actual = taskCreate.DictionaryIds[i].ToString();
                Assert.Equal(expected, actual);
            }

            var documentUpdated = await context.Documents.Find(
                Builders<DocumentDataModel>.Filter.In(d => d.Id, taskCreate.DocumentIds.Select(id => new ObjectId(id)))).ToListAsync();
            Assert.NotNull(documentUpdated);
            Assert.Equal(documentCount, documentUpdated.Count());
            Assert.All(documentUpdated, d =>
            {
                Assert.Contains(d.Id.ToString(), taskCreate.DocumentIds);
                Assert.Equal(DocumentStatusEnum.Assigned, d.Status);
            });

        }

        [Fact]
        public async Task CreateTaskWithDuplicatedName()
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;

            var workspace = workspaces.First();
            var task = tasks.First();

            var workflowId = ObjectId.GenerateNewId();
            var birthStepId = ObjectId.GenerateNewId();

            var documentsToAdd = await PrepareDocumentsToCreateTask(workspace.Id, workflowId, birthStepId);

            var workflowStepServiceMock = new Mock<IWorkflowStepService>();
            workflowStepServiceMock.Setup(wf => wf
                .GetPreannotationWorkflowStepAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new WorkflowNextStepServiceModel
                {
                    Id = birthStepId.ToString(),
                    ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                    WorkflowId = workflowId.ToString()
                }));

            var taskCreate = new TaskCreateServiceModel()
            {
                Name = task.Name,
                AnnotatorIds = users.Take(3).Select(u => u.Id.ToString()).ToList(),
                CreateBy = users.First().Id.ToString(),
                DocumentIds = documentsToAdd.Take(10).Select(d => d.Id.ToString()).ToList(),
                ExpectedDueAt = DateTime.UtcNow.AddHours(2),
                Overlap = 25,
                WorkspaceId = workspace.Id.ToString(),
                DictionaryIds = dictonaries.Select(m => m.Id.ToString()).ToList(),
                InspectorIds = users.Select(m => m.Id.ToString()).ToList(),
                AcceptorIds = users.Take(1).Select(m => m.Id.ToString()).ToList()
            };

            ITaskService taskService = new TaskService(null, context, null, workflowStepServiceMock.Object, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => taskService.CreateTaskAsync(taskCreate));

            Assert.Equal(MusicKGMessages.TaskNameExistMessage, exception.Message);
        }

        [Fact]
        public async Task CreateTaskWithNotExistingDocuments()
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;

            var workspace = workspaces.First();

            var workflowId = ObjectId.GenerateNewId();
            var birthStepId = ObjectId.GenerateNewId();

            var taskCreate = new TaskCreateServiceModel()
            {
                Name = RandomStringHelper.RandomString(10),
                AnnotatorIds = users.Take(3).Select(u => u.Id.ToString()).ToList(),
                CreateBy = users.First().Id.ToString(),
                DocumentIds = Enumerable.Range(1, 10).Select(i => ObjectId.GenerateNewId().ToString()).ToList(),
                ExpectedDueAt = DateTime.UtcNow.AddHours(2),
                Overlap = 25,
                WorkspaceId = workspace.Id.ToString()
            };

            var workflowStepServiceMock = new Mock<IWorkflowStepService>();
            workflowStepServiceMock.Setup(wf => wf
                .GetPreannotationWorkflowStepAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new WorkflowNextStepServiceModel
                {
                    Id = birthStepId.ToString(),
                    ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                    WorkflowId = workflowId.ToString()
                }));

            ITaskService taskService = new TaskService(null, context, null, workflowStepServiceMock.Object, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => taskService.CreateTaskAsync(taskCreate));

            Assert.Equal(MusicKGMessages.DocumentNotExistMessage, exception.Message);
        }

        private async Task<IEnumerable<DocumentDataModel>> PrepareDocumentsToCreateTask(ObjectId workspaceId, ObjectId workflowId, ObjectId birthStepId)
        {
            var documents = Enumerable.Range(1, 100).Select(i =>
            {
                return new DocumentDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    WorkspaceId = workspaceId,
                    WorkflowId = workflowId,
                    Name = $"document {i}",
                    Status = i % 2 == 0 ? DocumentStatusEnum.Preannotated : DocumentStatusEnum.Assigned,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = ObjectId.GenerateNewId(),
                    ContentType = "application/json",
                    BirthStep = new DocumentProcessStepDataModel
                    {
                        StepId = birthStepId
                    }
                };
            }).ToList();

            await context.Documents.InsertManyAsync(documents);

            return documents;
        }
    }
}
