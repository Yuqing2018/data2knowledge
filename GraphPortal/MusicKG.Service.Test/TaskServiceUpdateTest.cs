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
    public partial class TaskServiceUpdateTest : IDisposable, IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly string databaseName;

        public TaskServiceUpdateTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            databaseName = RandomStringHelper.RandomString(10);
            context = new MusicKGContext(mongoFixture.Context.Client, databaseName);
        }

        #region Positive

        [Theory]
        [InlineData(true, "UpdatedTaskName", false)]
        [InlineData(true, "UpdatedTaskName", true)]
        [InlineData(false, null, true)]
        [InlineData(false, null, false)]
        public async Task UpdateWithName(bool isNameAssigned, string name, bool isExpectedDueAtAssigned)
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            var update = new TaskUpdateServiceModel()
            {
                IsNameAssigned = isNameAssigned,
                Name = name,
                ExpectedDueAt = isExpectedDueAtAssigned ? DateTime.UtcNow.AddDays(7) : (DateTime?) null
            };

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.UpdateTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString(), update);

            var updatedTask = await context.Tasks.Find(t => t.Id == task.Id).FirstAsync();

            Assert.NotNull(updatedTask);
            if (isNameAssigned)
                Assert.Equal(update.Name, updatedTask.Name);
            else
                Assert.Equal(task.Name, updatedTask.Name);

            if (isExpectedDueAtAssigned)
                Assert.Equal(update.ExpectedDueAt.ToString(), updatedTask.ExpectedDueAt.ToString());
            else
                Assert.Equal(task.ExpectedDueAt.ToString(), updatedTask.ExpectedDueAt.ToString());
        }

        #endregion

        #region Negative

        [Fact]
        public async Task UpdateWithInvalidId()
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.UpdateTaskAsync(task.WorkspaceId.ToString(), ObjectId.GenerateNewId().ToString(), new TaskUpdateServiceModel()));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateWithInvalidWorkspaceId()
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.UpdateTaskAsync(ObjectId.GenerateNewId().ToString(), task.Id.ToString(),
                    new TaskUpdateServiceModel()));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateWithInvalidExpectedDueAt()
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var update=new TaskUpdateServiceModel()
            {
                ExpectedDueAt = task.CreatedAt.AddDays(-1)
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.UpdateTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString(),
                    update));

            Assert.Equal(MusicKGMessages.TaskExpectedDueAtWrongMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateWithDuplicatedName()
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var update = new TaskUpdateServiceModel()
            {
                Name = tasks.Last().Name,
                IsNameAssigned = true
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.UpdateTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString(),
                    update));

            Assert.Equal(MusicKGMessages.TaskNameExistMessage, exception.Message);
        }

        #endregion

        public void Dispose()
        {
            context.Client.DropDatabase(databaseName);
        }
    }
}
