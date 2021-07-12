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
    public partial class TaskServiceDeleteTest : IDisposable, IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly string databaseName;

        public TaskServiceDeleteTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            databaseName = RandomStringHelper.RandomString(10);
            context = new MusicKGContext(mongoFixture.Context.Client, databaseName);
        }

        #region Positive

        [Fact]
        public async Task Delete()
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            await taskService.DeleteTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString());

            var actualTask = await context.Tasks.Find(m => m.Id == task.Id).FirstOrDefaultAsync();

            Assert.True(actualTask.IsDeleted);
            Assert.NotEqual(task.Name, actualTask.Name);
        }


        #endregion

        #region Negative

        [Fact]
        public async Task DeleteWithInvalidId()
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.DeleteTaskAsync(task.WorkspaceId.ToString(), ObjectId.GenerateNewId().ToString()));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteWithInvalidWorkspaceId()
        {
            var (workspaces, users, tasks, documents) = await TaskServiceTest.PrepareTasksData(context);
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.DeleteTaskAsync(ObjectId.GenerateNewId().ToString(), task.Id.ToString()));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        #endregion

        public void Dispose()
        {
            context.Client.DropDatabase(databaseName);
        }
    }
}
