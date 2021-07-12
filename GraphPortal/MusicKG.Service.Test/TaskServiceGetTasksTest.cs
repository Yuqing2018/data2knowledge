using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Moq;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Fixtures;
using MusicKG.Service.Test.Helpers;
using Xunit;

namespace MusicKG.Service.Test
{
    [Collection("TaskCollection")]
    public class TaskServiceGetTasksTest
    {
        private readonly TaskFixture taskFixture;
        private readonly IMusicKGContext context;

        public TaskServiceGetTasksTest(TaskFixture taskFixture)
        {
            this.taskFixture = taskFixture;
            context = taskFixture.Context;
        }

        [Theory]
        [ClassData(typeof(ClassDataForGetTasks))]
        public async Task GetAllTasks(int? workspaceIdIndex, int? annotatorIdIndex, IEnumerable<TaskStatusEnum> statuses, int from, int? size, string taskType)
        {
            var (workspaceTypes, workspaces, users, tasks, documents, dictonaries, taskStatuses) = taskFixture.Data;

            ITaskService taskService = new TaskService(null, context, null, null, null);

            var workspaceId = workspaceIdIndex.HasValue ? workspaces.Skip(workspaceIdIndex.Value).First().Id.ToString() : null;
            var annotatorId = annotatorIdIndex.HasValue ? users.Skip(annotatorIdIndex.Value).First().Id.ToString() : null;

            var (totalCount, tasksReturned) = await taskService.GetTasksAsync(workspaceId, annotatorId, null, statuses, from, size, taskType);
            Assert.NotNull(tasksReturned);

            var tasksExpectedTotal = tasks
                .Where(t => (t.WorkspaceId.ToString().Equals(workspaceId) || workspaceId == null) 
                    && (statuses == null || statuses.Count() == 0 || statuses.Contains(t.ExpectedTaskStatus))
                    && (annotatorId == null || t.Annotators.Where(a => a.AnnotatorId.ToString().Equals(annotatorId)).First().Documents.Count() > 0))
                .OrderByDescending(t => t.CreatedAt);

            if (taskType != null)
            {
                tasksExpectedTotal = tasksExpectedTotal.Where(t => t.TaskType.ToString() == taskType).OrderByDescending(t => t.CreatedAt);
            }

            Assert.Equal(tasksExpectedTotal.Count(), totalCount);

            var tasksExpected = tasksExpectedTotal.Skip(from).Take(size ?? int.MaxValue);
            Assert.Equal(tasksExpected.Count(), tasksReturned.Count());

            Assert.Equal(tasksReturned.Select(t => t.CreatedAt.ToString()), tasksExpected.Select(t => t.CreatedAt.ToString()));

            Assert.All(tasksReturned, t =>
            {
                var taskExpected = (tasksExpectedTotal.Count() != tasksExpected.Count() ? tasksExpectedTotal : tasksExpected).Where(ta => ta.Id.ToString().Equals(t.Id)).First();
                Assert.Equal(taskExpected.Overlap, t.Overlap);
                Assert.Equal(taskExpected.Name, t.Name);
                Assert.Equal(taskExpected.Id.ToString(), t.Id);
                Assert.Equal(taskExpected.ExpectedTaskStatus, t.Status);
                if (taskType != null)
                {
                    Assert.Equal(taskType, t.TaskType);
                }
                else
                {
                    Assert.Equal(taskExpected.TaskType?.ToString(), t.TaskType);
                }
                Assert.NotNull(t.Workspace);
                var workspaceExpected = workspaces.Where(w => w.Id.ToString().Equals(t.Workspace.Id)).FirstOrDefault();
                Assert.NotNull(workspaceExpected);
                Assert.Equal(workspaceExpected.Id.ToString(), t.Workspace.Id);
                Assert.Equal(workspaceExpected.Name, t.Workspace.Name);
                Assert.Equal(workspaceExpected.Type.ToString(), t.Workspace.Type.Id);

                var workspceTypeExpected = workspaceTypes.First(wt => wt.Id == workspaceExpected.Type);
                Assert.Equal(workspceTypeExpected.Name, t.Workspace.Type.Name);

                Assert.All(t.Annotators, a =>
                {
                    var userExpected = users.Where(u => u.Id.ToString().Equals(a.Annotator.Id)).FirstOrDefault();
                    Assert.NotNull(userExpected);
                    Assert.Equal(userExpected.Name, a.Annotator.Name);
                    Assert.Equal(userExpected.Roles, a.Annotator.Roles);
                });
            });
        }

        public class ClassDataForGetTasks : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null, null, null, 0, null, null };
                yield return new object[] { null, null, null, 1, null, null };
                yield return new object[] { null, null, null, 1, 1, null };
                yield return new object[] { null, null, null, 10, 5, null };
                yield return new object[] { null, null, null, 100, 1, null };
                yield return new object[] { 0, null, null, 0, null, null };
                yield return new object[] { 0, null, null, 1, null, null };
                yield return new object[] { 0, null, null, 1, 1, null };
                yield return new object[] { null, 0, null, 0, null, null };
                yield return new object[] { null, 0, null, 1, null, null };
                yield return new object[] { null, 0, null, 1, 1, null };
                yield return new object[] { 0, 0, null, 0, null, null };
                yield return new object[] { 0, 0, null, 1, null, null };
                yield return new object[] { 0, 0, null, 1, 1, null };

                var statuses = Enum.GetNames(typeof(TaskStatusEnum));
                
                for (int i = 0; i < Math.Pow(2, statuses.Length); i++)
                {
                    List<TaskStatusEnum> subset = new List<TaskStatusEnum>();
                    uint rs = 0;

                    while (rs < statuses.Length)
                    {
                        if ((i & (1u << (int)rs)) > 0)
                            subset.Add(Enum.Parse<TaskStatusEnum>(statuses[(int)rs]));

                        rs++;
                    }

                    yield return new object[] { 0, 0, subset, 1, 1, null };
                }

                foreach (var taskType in TaskDataHelper.TaskTypes)
                {
                    yield return new object[] { 0, 0, null, 1, 1, taskType.ToString() };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
