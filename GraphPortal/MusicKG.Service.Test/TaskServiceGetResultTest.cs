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
    public class TaskServiceGetResultTest : IDisposable, IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly string databaseName;

        public TaskServiceGetResultTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            databaseName = RandomStringHelper.RandomString(10);
            context = new MusicKGContext(mongoFixture.Context.Client, databaseName);
        }

        [Fact]
        public async Task GetDeletedTaskDocumentResult()
        {
            var task = await PrepareDeletedTaskDocumentResultDataToGet();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var results = await taskService.GetTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                task.Annotators.First().AnnotatorId.ToString(),
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                TaskDocumentResultTypeEnum.ForModelTraining);

            Assert.NotNull(results);
            Assert.Empty(results);
        }

        private async Task<TaskDataModel> PrepareDeletedTaskDocumentResultDataToGet()
        {
            var task = new TaskDataModel
            {
                Status = TaskStatusEnum.Created,
                IsDeleted = true,
                WorkspaceId = ObjectId.GenerateNewId(),
                CreatedAt = DateTime.UtcNow,
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
            };

            await context.Tasks.InsertOneAsync(task);

            await context.Users.InsertManyAsync(task.Annotators.Select(a => new UserDataModel
            {
                Id = a.AnnotatorId,
                Name = a.AnnotatorId.ToString()
            }));

            return task;
        }

        public void Dispose()
        {
            context.Client.DropDatabase(databaseName);
        }
    }
}
