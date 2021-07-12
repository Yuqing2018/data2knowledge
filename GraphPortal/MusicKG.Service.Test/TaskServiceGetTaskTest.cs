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
    public class TaskServiceGetTaskTest
    {
        private readonly TaskFixture taskFixture;
        private readonly IMusicKGContext context;

        public TaskServiceGetTaskTest(TaskFixture taskFixture)
        {
            this.taskFixture = taskFixture;
            context = taskFixture.Context;
        }

        [Theory]
        [MemberData(nameof(MethodDataForGetTask))]
        public async Task GetTask(int workspaceIndex, int taskIndex, int? annotatorIndex, bool isInCludingDocuments)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;

            var workspace = workspaces.Skip(workspaceIndex).First();
            var task = tasks.Where(t => t.WorkspaceId == workspace.Id).Skip(taskIndex).First();
            var user = annotatorIndex.HasValue ? users.Skip(annotatorIndex.Value).First() : null;

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var taskReturned = await taskService.GetTaskAsync(workspace.Id.ToString(), task.Id.ToString(), user?.Id.ToString(), isInCludingDocuments);

            Assert.Equal(task.Overlap, taskReturned.Overlap);
            Assert.Equal(task.Name, taskReturned.Name);
            Assert.Equal(task.TaskType?.ToString(), taskReturned.TaskType);
            Assert.Equal(task.Id.ToString(), taskReturned.Id);
            Assert.Equal(taskStatuses[taskIndex][0], taskReturned.Status);
            Assert.Equal(task.InspectorIds.First().ToString(), taskReturned.Inspectors.First().Id);
            Assert.Equal(task.AcceptorIds.First().ToString(), taskReturned.Acceptors.First().Id);

            Assert.NotNull(taskReturned.Workspace);
            var workspaceExpected = workspaces.Where(w => w.Id.ToString().Equals(taskReturned.Workspace.Id)).FirstOrDefault();
            Assert.NotNull(workspaceExpected);
            Assert.Equal(workspaceExpected.Id.ToString(), taskReturned.Workspace.Id);
            Assert.Equal(workspaceExpected.Name, taskReturned.Workspace.Name);
            Assert.Equal(workspaceExpected.Type.ToString(), taskReturned.Workspace.Type.Id);

            for (int i = 0; i < dictonaries.Count(); i++)
            {
                var expected = dictonaries.ToList()[i].Id.ToString();
                var actual = task.DictionaryIds[i].ToString();
                Assert.Equal(expected, actual);
            }

            if (user != null)
                Assert.Single(taskReturned.Annotators);
            else
                Assert.Equal(users.Count(), taskReturned.Annotators.Count());

            Assert.All(taskReturned.Annotators, a =>
            {
                var userExpected = users.Where(u => u.Id.ToString().Equals(a.Annotator.Id)).FirstOrDefault();
                Assert.NotNull(userExpected);
                Assert.Equal(userExpected.Name, a.Annotator.Name);
                Assert.Equal(userExpected.Roles, a.Annotator.Roles);

                if (!isInCludingDocuments)
                    Assert.Null(a.TaskDocuments);
                else
                {
                    var taskDocumentsExpected = task.Annotators.First(an => an.AnnotatorId.ToString().Equals(a.Annotator.Id)).Documents;
                    Assert.Equal(taskDocumentsExpected.Count(), a.TaskDocuments.Count());
                    Assert.All(a.TaskDocuments, d =>
                    {
                        var taskDocumentExpected = taskDocumentsExpected.First(tde => tde.DocumentId.ToString().Equals(d.Document.Id));
                        Assert.Equal(taskDocumentExpected.Status, d.Status);
                        Assert.Equal(a.Annotator.Id, d.AnnotatorId);
                        Assert.Equal(taskDocumentExpected.Results.Max(r => r.CreatedAt.ToString()), d.LatestResultSavedAt.ToString());
                    });
                }
            });
        }

        [Fact]
        public async Task GetTaskWithInvalidId()
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;
            var task = tasks.First();

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                taskService.GetTaskAsync(task.WorkspaceId.ToString(), ObjectId.GenerateNewId().ToString()));

            Assert.Equal(MusicKGMessages.TaskNotExistMessage, exception.Message);
        }

        public static IEnumerable<object[]> MethodDataForGetTask()
        {
            return TaskDataHelper.Workspaces.Take(1).SelectMany((w, i) =>
                TaskDataHelper.TaskStatuses.SelectMany((s, j) =>
                    TaskDataHelper.Users.Append(null).SelectMany((u, k) =>
                        new[] {
                            new object[] { i, j, u == null ? (int?)null : k, true },
                            new object[] { i, j, u == null ? (int?)null : k, false }
                        })));
        }
    }
}
