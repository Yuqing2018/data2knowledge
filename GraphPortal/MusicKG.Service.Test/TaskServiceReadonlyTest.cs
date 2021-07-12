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
    public partial class TaskServiceReadonlyTest
    {
        private readonly TaskFixture taskFixture;
        private readonly IMusicKGContext context;

        public TaskServiceReadonlyTest(TaskFixture taskFixture)
        {
            this.taskFixture = taskFixture;
            context = taskFixture.Context;
        }

        #region ApproveTask

        #region Negative

        [Theory]
        [InlineData(TaskStatusEnum.Created)]
        [InlineData(TaskStatusEnum.Rejected)]
        [InlineData(TaskStatusEnum.ConflictResolved)]
        [InlineData(TaskStatusEnum.KnowledgeMerged)]
        public async Task ApproveTaskInInvalidStatus(TaskStatusEnum status)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;
            var task = tasks.Where(t => t.ExpectedTaskStatus == status).First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.ApproveTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString()));

            Assert.Equal(MusicKGMessages.NoTaskCanBeApprovedMessage, exception.Message);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task ApproveNotExistingTask(bool isValidWorkspaceId, bool isValidTaskId)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;
            var task = tasks.Where(t => t.ExpectedTaskStatus == TaskStatusEnum.Submitted).First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.ApproveTaskAsync(
                    isValidWorkspaceId ? task.WorkspaceId.ToString() : ObjectId.GenerateNewId().ToString(),
                    isValidTaskId ? task.Id.ToString() : ObjectId.GenerateNewId().ToString()));

            Assert.Equal(MusicKGMessages.NoTaskCanBeApprovedMessage, exception.Message);
        }

        #endregion

        #endregion

        #region RejectTask

        #region Negative

        [Theory]
        [InlineData(TaskStatusEnum.Created)]
        [InlineData(TaskStatusEnum.Rejected)]
        [InlineData(TaskStatusEnum.ConflictResolved)]
        [InlineData(TaskStatusEnum.KnowledgeMerged)]
        public async Task RejecteTaskInInvalidStatus(TaskStatusEnum status)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;
            var task = tasks.Where(t => t.ExpectedTaskStatus == status).First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.RejectTaskAsync(task.WorkspaceId.ToString(), task.Id.ToString()));

            Assert.Equal(MusicKGMessages.NoTaskCanBeRejectMessage, exception.Message);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task RejecteNotExistingTask(bool isValidWorkspaceId, bool isValidTaskId)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;
            var task = tasks.Where(t => t.ExpectedTaskStatus == TaskStatusEnum.Submitted).First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.RejectTaskAsync(
                    isValidWorkspaceId ? task.WorkspaceId.ToString() : ObjectId.GenerateNewId().ToString(),
                    isValidTaskId ? task.Id.ToString() : ObjectId.GenerateNewId().ToString()));

            Assert.Equal(MusicKGMessages.NoTaskCanBeRejectMessage, exception.Message);
        }

        #endregion

        #endregion
    }
}
