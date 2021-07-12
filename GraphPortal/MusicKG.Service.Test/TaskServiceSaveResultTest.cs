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
    public class TaskServiceSaveResultTest : IDisposable, IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly string databaseName;

        public TaskServiceSaveResultTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            databaseName = RandomStringHelper.RandomString(10);
            context = new MusicKGContext(mongoFixture.Context.Client, databaseName);
        }

        [Fact]
        public async Task SaveTaskDocumentResultWithEmptyResuls()
        {
            var task = await PrepareTaskDocumentResultDataWithEmptyResults();

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.SaveTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                });
            
            var taskUpdated = await context.Tasks.Find(t => t.Id == task.Id).FirstOrDefaultAsync();

            Assert.NotNull(taskUpdated);
            Assert.Equal(task.Id, taskUpdated.Id);

            var annotatorsUpdated = taskUpdated.Annotators;
            Assert.NotNull(annotatorsUpdated);
            Assert.NotEmpty(annotatorsUpdated);

            var annotatorUpdated = annotatorsUpdated.First();
            Assert.NotNull(annotatorUpdated);

            var documentsUpdated = annotatorUpdated.Documents;
            Assert.NotNull(documentsUpdated);
            Assert.NotEmpty(documentsUpdated);

            var documentUpdated = documentsUpdated.First();
            Assert.NotNull(documentUpdated);
            Assert.Equal(TaskDocumentStatusEnum.Annotated, documentUpdated.Status);

            var resultsUpdated = documentUpdated.Results;
            Assert.NotNull(resultsUpdated);
            Assert.Single(resultsUpdated);

            var resultUpdated = resultsUpdated.First();
            Assert.NotNull(resultUpdated);
            Assert.Equal(resultDocumentId, resultUpdated.ResultDocumentId);
            Assert.Equal(TaskDocumentResultTypeEnum.ForModelTraining, resultUpdated.ResultType);
        }

        [Fact]
        public async Task SaveTaskDocumentResultWithNotExistingResultType()
        {
            var task = await PrepareTaskDocumentResultData();

            var existingResultDocumentId = task.Annotators.First().Documents.First().Results.First().ResultDocumentId;

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.SaveTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForGraphMerging,
                    Status = TaskDocumentStatusEnum.Annotated
                });

            var taskUpdated = await context.Tasks.Find(t => t.Id == task.Id).FirstOrDefaultAsync();

            Assert.NotNull(taskUpdated);
            Assert.Equal(task.Id, taskUpdated.Id);

            var annotatorsUpdated = taskUpdated.Annotators;
            Assert.NotNull(annotatorsUpdated);
            Assert.NotEmpty(annotatorsUpdated);

            var annotatorUpdated = annotatorsUpdated.First();
            Assert.NotNull(annotatorUpdated);

            var documentsUpdated = annotatorUpdated.Documents;
            Assert.NotNull(documentsUpdated);
            Assert.NotEmpty(documentsUpdated);

            var documentUpdated = documentsUpdated.First();
            Assert.NotNull(documentUpdated);
            Assert.Equal(TaskDocumentStatusEnum.Annotated, documentUpdated.Status);

            var resultsUpdated = documentUpdated.Results;
            Assert.NotNull(resultsUpdated);
            Assert.Equal(2, resultsUpdated.Count());

            var resultNotUpdated = resultsUpdated.First();
            Assert.NotNull(resultNotUpdated);
            Assert.Equal(existingResultDocumentId, resultNotUpdated.ResultDocumentId);
            Assert.Equal(TaskDocumentResultTypeEnum.ForModelTraining, resultNotUpdated.ResultType);

            var resultUpdated = resultsUpdated.Last();
            Assert.NotNull(resultUpdated);
            Assert.Equal(resultDocumentId, resultUpdated.ResultDocumentId);
            Assert.Equal(TaskDocumentResultTypeEnum.ForGraphMerging, resultUpdated.ResultType);
        }

        [Fact]
        public async Task UpdateExistingTaskDocumentResult()
        {
            var task = await PrepareTaskDocumentResultData();

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.SaveTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                });

            var taskUpdated = await context.Tasks.Find(t => t.Id == task.Id).FirstOrDefaultAsync();

            Assert.NotNull(taskUpdated);
            Assert.Equal(task.Id, taskUpdated.Id);

            var annotatorsUpdated = taskUpdated.Annotators;
            Assert.NotNull(annotatorsUpdated);
            Assert.NotEmpty(annotatorsUpdated);

            var annotatorUpdated = annotatorsUpdated.First();
            Assert.NotNull(annotatorUpdated);

            var documentsUpdated = annotatorUpdated.Documents;
            Assert.NotNull(documentsUpdated);
            Assert.NotEmpty(documentsUpdated);

            var documentUpdated = documentsUpdated.First();
            Assert.NotNull(documentUpdated);
            Assert.Equal(TaskDocumentStatusEnum.Annotated, documentUpdated.Status);

            var resultsUpdated = documentUpdated.Results;
            Assert.NotNull(resultsUpdated);
            Assert.Single(resultsUpdated);

            var resultUpdated = resultsUpdated.First();
            Assert.NotNull(resultUpdated);
            Assert.Equal(resultDocumentId, resultUpdated.ResultDocumentId);
            Assert.Equal(TaskDocumentResultTypeEnum.ForModelTraining, resultUpdated.ResultType);
        }

        [Fact]
        public async Task SaveTaskDocumentResultWithNotExistingWorkspaceId()
        {
            var task = await PrepareTaskDocumentResultData();

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => taskService.SaveTaskDocumentResultAsync(
                ObjectId.GenerateNewId().ToString(),
                task.Id.ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                }));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task SaveTaskDocumentResultWithNotExistingTaskId()
        {
            var task = await PrepareTaskDocumentResultData();

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => taskService.SaveTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                ObjectId.GenerateNewId().ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                }));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task SaveTaskDocumentResultToDeletedTask()
        {
            var task = await PrepareDeletedTaskDocumentResultData();

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => taskService.SaveTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                ObjectId.GenerateNewId().ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                }));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task SaveTaskDocumentResultWithNotExistingAnnotatorId()
        {
            var task = await PrepareTaskDocumentResultData();

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => taskService.SaveTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                ObjectId.GenerateNewId().ToString(),
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                }));

            Assert.Equal(MusicKGMessages.TaskAnnotatorOrDocumentNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task SaveTaskDocumentResultWithNotExistingDocumentId()
        {
            var task = await PrepareTaskDocumentResultData();

            var resultDocumentId = ObjectId.GenerateNewId();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => taskService.SaveTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                ObjectId.GenerateNewId().ToString(),
                new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId.ToString(),
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                }));

            Assert.Equal(MusicKGMessages.TaskAnnotatorOrDocumentNotExistMessage, exception.Message);
        }

        private async Task<TaskDataModel> PrepareTaskDocumentResultData()
        {
            var taskDataModel = new TaskDataModel()
            {
                Status = TaskStatusEnum.Created,
                WorkspaceId = ObjectId.GenerateNewId(),
                Annotators = Enumerable.Range(1, 3).Select(i => new TaskAnnotatorDataModel
                {
                    AnnotatorId = ObjectId.GenerateNewId(),
                    Documents = Enumerable.Range(1, 100).Select(j => new TaskDocumentDataModel
                    {
                        DocumentId = ObjectId.GenerateNewId(),
                        Status = TaskDocumentStatusEnum.Assigned,
                        Results = new List<TaskDocumentResultDataModel>
                        {
                            new TaskDocumentResultDataModel
                            {
                                ResultType = TaskDocumentResultTypeEnum.ForModelTraining
                            }
                        }
                    }).ToList()
                }).ToList()
            };

            await context.Tasks.InsertOneAsync(taskDataModel);

            return taskDataModel;
        }

        private async Task<TaskDataModel> PrepareTaskDocumentResultDataWithEmptyResults()
        {
            var taskDataModel = new TaskDataModel()
            {
                Status = TaskStatusEnum.Created,
                WorkspaceId = ObjectId.GenerateNewId(),
                Annotators = Enumerable.Range(1, 3).Select(i => new TaskAnnotatorDataModel
                {
                    AnnotatorId = ObjectId.GenerateNewId(),
                    Documents = Enumerable.Range(1, 100).Select(j => new TaskDocumentDataModel
                    {
                        DocumentId = ObjectId.GenerateNewId(),
                        Status = TaskDocumentStatusEnum.Assigned,
                        Results = new List<TaskDocumentResultDataModel>()
                    }).ToList()
                }).ToList()
            };

            await context.Tasks.InsertOneAsync(taskDataModel);

            return taskDataModel;
        }

        private async Task<TaskDataModel> PrepareDeletedTaskDocumentResultData()
        {
            var taskDataModel = new TaskDataModel()
            {
                Status = TaskStatusEnum.Created,
                IsDeleted = true,
                WorkspaceId = ObjectId.GenerateNewId(),
                Annotators = Enumerable.Range(1, 3).Select(i => new TaskAnnotatorDataModel
                {
                    AnnotatorId = ObjectId.GenerateNewId(),
                    Documents = Enumerable.Range(1, 100).Select(j => new TaskDocumentDataModel
                    {
                        DocumentId = ObjectId.GenerateNewId(),
                        Status = TaskDocumentStatusEnum.Assigned,
                        Results = new List<TaskDocumentResultDataModel>
                        {
                            new TaskDocumentResultDataModel
                            {
                                ResultType = TaskDocumentResultTypeEnum.ForModelTraining
                            }
                        }
                    }).ToList()
                }).ToList()
            };

            await context.Tasks.InsertOneAsync(taskDataModel);

            return taskDataModel;
        }

        public void Dispose()
        {
            context.Client.DropDatabase(databaseName);
        }
    }
}
