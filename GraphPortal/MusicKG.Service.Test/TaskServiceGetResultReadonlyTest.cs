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
    [Collection("TaskCollection")]
    public class TaskServiceGetResultReadonlyTest
    {
        private readonly TaskFixture taskFixture;
        private readonly IMusicKGContext context;

        public TaskServiceGetResultReadonlyTest(TaskFixture taskFixture)
        {
            this.taskFixture = taskFixture;
            context = taskFixture.Context;
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task GetTaskDocumentResult(bool isWithAnnotatorId, bool isWithResultType)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;

            var task = tasks.Where(t => t.ExpectedTaskStatus == TaskStatusEnum.Submitted).First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var results = await taskService.GetTaskDocumentResultAsync(
                task.WorkspaceId.ToString(),
                task.Id.ToString(),
                isWithAnnotatorId ? task.Annotators.Skip(1).Take(1).First().AnnotatorId.ToString() : null,
                task.Annotators.First().Documents.First().DocumentId.ToString(),
                isWithResultType ? TaskDocumentResultTypeEnum.ForModelTraining : (TaskDocumentResultTypeEnum?)null);

            Assert.NotNull(results);
            Assert.Equal(isWithResultType ? 1 : 2, results.Count());

            Assert.All(results, result =>
            {
                var userExpected = users.Where(u => u.Id == task.Annotators.First().AnnotatorId).First();
                var documentExpected = task.Annotators.First().Documents.First();
                Assert.NotNull(result);
                Assert.Equal(documentExpected.DocumentId.ToString(), result.DocumentId);
                Assert.Equal(documentExpected.Status, result.TaskDocumentStatus);
                Assert.Contains(result.ResultType, new[] { TaskDocumentResultTypeEnum.ForModelTraining, TaskDocumentResultTypeEnum.ForGraphMerging });

                var resultExpected = documentExpected.Results.Where(r => r.ResultType == result.ResultType).First();
                Assert.Equal(resultExpected.ResultDocumentId.ToString(), result.ResultDocumentId);
                Assert.Equal(resultExpected.CreatedAt.ToString(), result.CreatedAt.ToString());

                Assert.NotNull(result.CreatedBy);
                Assert.Equal(result.CreatedBy.Id, userExpected.Id.ToString());
                Assert.Equal(result.CreatedBy.Name, userExpected.Name);
                Assert.Equal(result.CreatedBy.Roles, userExpected.Roles);
            });
        }
    }
}
