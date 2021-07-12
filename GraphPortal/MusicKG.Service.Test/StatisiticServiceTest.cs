using MusicKG.DataAccess;
using MusicKG.Service.Implementations;
using MusicKG.Service.Test.Fixtures;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Helpers;
using MusicKG.DataAccess.Models;
using MongoDB.Bson;
using static MusicKG.Service.Helpers.ErrorHelper;

namespace MusicKG.Service.Test
{
    [Collection("StatisiticsCollection")]
    public class StatisiticServiceTest
    {
        private readonly StatisiticsFixture statisiticFixture;
        private readonly IMusicKGContext context;

        public StatisiticServiceTest(StatisiticsFixture statisiticFixture)
        {
            this.statisiticFixture = statisiticFixture;
            context = statisiticFixture.Context;
        }

        public enum FromDateValues
        {
            GreaterThanEariestCreatedAt,

            EqualToEariestCreatedAt,

            LessThenEariestCreatedAt,

            GreaterThanLatestCreatedAt
        }

        public enum ToDateValues
        {
            GreaterThanLatestFinishedAt,

            EqualToLatestFinishedAt,

            LessThanLatestFinishedAt,

            LessThanEariestCreatedAt,

            LessThanLatestCreatedAt
        }

        [Theory]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1)]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.EqualToLatestFinishedAt, 0)]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.LessThanLatestFinishedAt, 1)]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.LessThanLatestCreatedAt, 2)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.EqualToLatestFinishedAt, 0)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.LessThanLatestFinishedAt, 1)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.LessThanLatestCreatedAt, 2)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.EqualToLatestFinishedAt, 0)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.LessThanLatestFinishedAt, 1)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.LessThanLatestCreatedAt, 2)]
        [InlineData(FromDateValues.GreaterThanLatestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.LessThanEariestCreatedAt, -1)]
        public async Task GetStatisticsOverview(FromDateValues fromDateCompareToMinValue, ToDateValues toDateCompareToMaxValue, int workspaceTypeIndex)
        {
            IStatisticService statisticsService = new StatisticService(context, null);
            DateTime fromDate, toDate;
            switch (fromDateCompareToMinValue)
            {
                case FromDateValues.EqualToEariestCreatedAt:
                    fromDate = StatisiticsDataHelper.EarliestCreatedAt;
                    break;
                case FromDateValues.GreaterThanEariestCreatedAt:
                    fromDate = StatisiticsDataHelper.EarliestCreatedAt.AddDays(1);
                    break;
                case FromDateValues.LessThenEariestCreatedAt:
                    fromDate = StatisiticsDataHelper.EarliestCreatedAt.AddDays(-1);
                    break;
                case FromDateValues.GreaterThanLatestCreatedAt:
                    fromDate = StatisiticsDataHelper.LatestCreatedAt.AddDays(1);
                    break;
                default:
                    fromDate = DateTime.MinValue.ToUniversalTime();
                    break;
            }

            switch (toDateCompareToMaxValue)
            {
                case ToDateValues.EqualToLatestFinishedAt:
                    toDate = StatisiticsDataHelper.LatestFinishedAt;
                    break;
                case ToDateValues.GreaterThanLatestFinishedAt:
                    toDate = StatisiticsDataHelper.LatestFinishedAt.AddDays(1);
                    break;
                case ToDateValues.LessThanEariestCreatedAt:
                    toDate = StatisiticsDataHelper.EarliestCreatedAt.AddDays(0.5);
                    break;
                case ToDateValues.LessThanLatestCreatedAt:
                    toDate = StatisiticsDataHelper.LatestCreatedAt.AddDays(-1);
                    break;
                case ToDateValues.LessThanLatestFinishedAt:
                    toDate = StatisiticsDataHelper.LatestFinishedAt.AddDays(-1);
                    break;
                default:
                    toDate = DateTime.MaxValue.ToUniversalTime();
                    break;
            }

            string workspaceTypeId = null;

            if (workspaceTypeIndex != -1)
            {
                workspaceTypeId = TaskDataHelper.Workspaces.ToArray()[workspaceTypeIndex].Type.ToString();
            }

            var result = await statisticsService.GetStatisticsOverviewAsync(fromDate, toDate, workspaceTypeId);

            var users = this.statisiticFixture.Data.Item1;

            Assert.Equal(users.Count(), result.Count());
            Assert.All(result, x =>
            {
                var user = users.First(u => u.Id.ToString() == x.User.Id);
                Assert.Equal(user.Name, x.User.Name);
                Assert.Equal(user.Roles, x.User.Roles);
                var expectedCreatedCount = user.Roles.Contains(UserRoleEnum.Manager) ? this.GetCreatedTaskCount(x.User, fromDate, toDate, workspaceTypeId) : 0;
                Assert.Equal(expectedCreatedCount, x.Statistics.CreatedTaskCount);
                var expectedFinishedCount = this.GetFinishedTaskCount(x.User, fromDate, toDate, workspaceTypeId);
                Assert.Equal(expectedFinishedCount, x.Statistics.FinishedTaskCount);
                var expectedNotFinishedCount = this.GetNotFinishedTaskCount(x.User, fromDate, toDate, workspaceTypeId);
                Assert.Equal(expectedNotFinishedCount, x.Statistics.NotFinishedTaskCount);
                var expectedFinishedDocumentCount = this.GetFinishedDocumentCount(x.User, fromDate, toDate, workspaceTypeId);
                Assert.Equal(expectedFinishedDocumentCount, x.Statistics.FinishedDocumentCount);
                var expectedNotFinishedDocumentCount = this.GetNotFinishedDocumentCount(x.User, fromDate, toDate, workspaceTypeId);
                Assert.Equal(expectedNotFinishedDocumentCount, x.Statistics.NotFinishedDocumentCount);
                var expectedFinishedItemCount = this.GetFinishedItemCount(x.User, fromDate, toDate, workspaceTypeId);
                var expectedNotFinishedItemCount = this.GetNotFinishedItemCount(x.User, fromDate, toDate, workspaceTypeId);
            });
        }

        [Theory]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1, 0)]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.EqualToLatestFinishedAt, 0, 1)]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.LessThanLatestFinishedAt, 1, 2)]
        [InlineData(FromDateValues.GreaterThanEariestCreatedAt, ToDateValues.LessThanLatestCreatedAt, 2, 3)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1, 0)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.EqualToLatestFinishedAt, 0, 1)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.LessThanLatestFinishedAt, 1, 2)]
        [InlineData(FromDateValues.EqualToEariestCreatedAt, ToDateValues.LessThanLatestCreatedAt, 2, 3)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1, 0)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.EqualToLatestFinishedAt, 0, 1)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.LessThanLatestFinishedAt, 1, 2)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.LessThanLatestCreatedAt, 2, 3)]
        [InlineData(FromDateValues.GreaterThanLatestCreatedAt, ToDateValues.GreaterThanLatestFinishedAt, -1, 0)]
        [InlineData(FromDateValues.LessThenEariestCreatedAt, ToDateValues.LessThanEariestCreatedAt, -1, 1)]
        public async Task GetStatisticsDetails(FromDateValues fromDateCompareToMinValue, ToDateValues toDateCompareToMaxValue, int workspaceTypeIndex, int userIndex)
        {
            IStatisticService statisticsService = new StatisticService(context, null);
            DateTime fromDate, toDate;
            switch (fromDateCompareToMinValue)
            {
                case FromDateValues.EqualToEariestCreatedAt:
                    fromDate = StatisiticsDataHelper.EarliestCreatedAt;
                    break;
                case FromDateValues.GreaterThanEariestCreatedAt:
                    fromDate = StatisiticsDataHelper.EarliestCreatedAt.AddDays(1);
                    break;
                case FromDateValues.LessThenEariestCreatedAt:
                    fromDate = StatisiticsDataHelper.EarliestCreatedAt.AddDays(-1);
                    break;
                case FromDateValues.GreaterThanLatestCreatedAt:
                    fromDate = StatisiticsDataHelper.LatestCreatedAt.AddDays(1);
                    break;
                default:
                    fromDate = DateTime.MinValue.ToUniversalTime();
                    break;
            }

            switch (toDateCompareToMaxValue)
            {
                case ToDateValues.EqualToLatestFinishedAt:
                    toDate = StatisiticsDataHelper.LatestFinishedAt;
                    break;
                case ToDateValues.GreaterThanLatestFinishedAt:
                    toDate = StatisiticsDataHelper.LatestFinishedAt.AddDays(1);
                    break;
                case ToDateValues.LessThanEariestCreatedAt:
                    toDate = StatisiticsDataHelper.EarliestCreatedAt.AddDays(0.5);
                    break;
                case ToDateValues.LessThanLatestCreatedAt:
                    toDate = StatisiticsDataHelper.LatestCreatedAt.AddDays(-1);
                    break;
                case ToDateValues.LessThanLatestFinishedAt:
                    toDate = StatisiticsDataHelper.LatestFinishedAt.AddDays(-1);
                    break;
                default:
                    toDate = DateTime.MaxValue.ToUniversalTime();
                    break;
            }

            string workspaceTypeId = null;

            var userId = userIndex == 3 ? ObjectId.GenerateNewId().ToString() : TaskDataHelper.Users.ToArray()[userIndex].Id.ToString();

            if (workspaceTypeIndex != -1)
            {
                workspaceTypeId = TaskDataHelper.Workspaces.ToArray()[workspaceTypeIndex].Type.ToString();
            }

            if (userIndex < 3)
            {
                var result = await statisticsService.GetStatisticsDetailsAsync(userId, fromDate, toDate, workspaceTypeId);

                var expectedUser = TaskDataHelper.Users.ToArray()[userIndex];

                Assert.NotNull(result);
                Assert.Equal(expectedUser.Name, result.User.Name);
                Assert.Equal(expectedUser.Roles, result.User.Roles);

                var expectedTasks = this.GetExpectedTasks(expectedUser.Id.ToString(), fromDate, toDate, workspaceTypeId);

                expectedTasks = expectedTasks.Where(x => x.Annotators?.Count(a => a.AnnotatorId == expectedUser.Id) > 0);

                Assert.Equal(expectedTasks.Count(), result.TaskStatisticsDetails.Count());

                Assert.All(result.TaskStatisticsDetails, x =>
                {
                    var expectedTask = expectedTasks.FirstOrDefault(t => t.Id.ToString() == x.TaskId);
                    var expectedWorkspace = TaskDataHelper.Workspaces.First(ws => ws.Id == expectedTask.WorkspaceId);
                    var expectedWorkspaceType = TaskDataHelper.WorkspaceTypes.First(wt => wt.Id == expectedWorkspace.Type);
                    Assert.NotNull(expectedTask);
                    Assert.Equal(expectedTask.Name, x.TaskName);
                    Assert.Equal(expectedTask.CreatedAt, x.CreatedAt);
                    Assert.Equal(expectedTask.ExpectedDueAt, x.ExpectedDueAt);
                    Assert.Equal(expectedTask.WorkspaceId.ToString(), x.Workspace.Id);
                    Assert.Equal(expectedWorkspace.Name, x.Workspace.Name);
                    Assert.Equal(expectedWorkspaceType.Name, x.Workspace.Type.Name);
                    Assert.All(x.RelatedUsers, ru =>
                    {
                        var tempUser = this.statisiticFixture.Data.Item1.First(u => u.Id.ToString() == ru.Annotator.Id);
                        Assert.Equal(tempUser.Name, ru.Annotator.Name);
                        Assert.Equal(tempUser.Roles.Contains(UserRoleEnum.Manager), ru.IsManager);
                    });
                    var expectedFinishedDocumentCount = expectedTask.Annotators.First(a => a.AnnotatorId == expectedUser.Id).Documents.Count(d => d.AnnotatedAt >= fromDate && d.AnnotatedAt <= toDate);
                    Assert.Equal(expectedFinishedDocumentCount, x.FinishedDocumentCount);
                });
            }
            else
            {
                var exception = await Assert.ThrowsAsync<ErrorMessageException>(() => statisticsService.GetStatisticsDetailsAsync(userId, fromDate, toDate, workspaceTypeId));
                Assert.Equal(Resources.MusicKGMessages.UserNotExistMessage, exception.Message);
            }
        }

        private long GetNotFinishedItemCount(UserServiceModel user, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var tasks = this.GetExpectedTasks(user.Id, fromDate, toDate, workspaceTypeId);
            var result = tasks == null ? 0 : tasks.Sum(x =>
            {
                var annotator = x.Annotators?.FirstOrDefault(a => a.AnnotatorId.ToString() == user.Id);
                var documents = annotator?.Documents;
                return annotator?.Documents?.Sum(d =>
                {
                    var document = this.statisiticFixture.Data.Item3.First(dd => dd.Id == d.DocumentId);
                    if (d.AnnotatedAt > toDate)
                    {
                        return document.ItemCount;
                    }
                    else
                    {
                        return 0;
                    }
                });
            });
            return result.HasValue ? result.Value : 0;
        }

        private long GetFinishedItemCount(UserServiceModel user, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var tasks = this.GetExpectedTasks(user.Id, fromDate, toDate, workspaceTypeId);
            var result = tasks == null ? 0 : tasks.Sum(x =>
            {
                var annotator = x.Annotators?.FirstOrDefault(a => a.AnnotatorId.ToString() == user.Id);
                var documents = annotator?.Documents;
                return annotator?.Documents?.Sum(d =>
                {
                    var document = this.statisiticFixture.Data.Item3.First(dd => dd.Id == d.DocumentId);
                    if (d.AnnotatedAt >= fromDate && d.AnnotatedAt <= toDate)
                    {
                        return document.ItemCount;
                    }
                    else
                    {
                        return 0;
                    }
                });
            });
            return result.HasValue ? result.Value : 0;
        }

        private long GetNotFinishedDocumentCount(UserServiceModel user, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var tasks = this.GetExpectedTasks(user.Id, fromDate, toDate, workspaceTypeId);
            var result = tasks == null ? 0 : tasks.Sum(x =>
            {
                var annotator = x.Annotators?.FirstOrDefault(a => a.AnnotatorId.ToString() == user.Id);
                return annotator?.Documents?.Count(d => d.AnnotatedAt > toDate);
            });
            return result.HasValue ? result.Value : 0;
        }

        private long GetFinishedDocumentCount(UserServiceModel user, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var tasks = this.GetExpectedTasks(user.Id, fromDate, toDate, workspaceTypeId);
            var result = tasks == null ? 0 : tasks.Sum(x =>
            {
                var annotator = x.Annotators?.FirstOrDefault(a => a.AnnotatorId.ToString() == user.Id);
                return annotator?.Documents?.Count(d => d.AnnotatedAt >= fromDate && d.AnnotatedAt <= toDate);
            });
            return result.HasValue ? result.Value : 0;
        }

        private long GetNotFinishedTaskCount(UserServiceModel user, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var tasks = this.GetExpectedTasks(user.Id, fromDate, toDate, workspaceTypeId);
            return tasks == null ? 0 : tasks.Count(x =>
            {
                var annotator = x.Annotators?.FirstOrDefault(a => a.AnnotatorId.ToString() == user.Id);
                var finishedDate = annotator?.Documents?.Max(d => d.AnnotatedAt);
                var result = annotator != null &&
                    finishedDate > toDate && x.CreatedAt <= toDate;
                return result;
            });
        }

        private long GetFinishedTaskCount(UserServiceModel user, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var tasks = this.GetExpectedTasks(user.Id, fromDate, toDate, workspaceTypeId);
            return tasks == null ? 0 : tasks.Count(x =>
            {
                var annotator = x.Annotators?.FirstOrDefault(a => a.AnnotatorId.ToString() == user.Id);
                var finishedDate = annotator?.Documents?.Max(d => d.AnnotatedAt);
                var result = annotator != null &&
                    finishedDate >= fromDate && finishedDate <= toDate;
                return result;
            });
        }

        private long GetCreatedTaskCount(UserServiceModel user, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var tasks = this.GetExpectedTasks(user.Id, fromDate, toDate, workspaceTypeId);
            return tasks == null ? 0 : tasks.Count(x =>
            {
                var result = x.Annotators?.Count(y => y.AnnotatorId.ToString() == user.Id) > 0 &&
                    x.CreatedAt >= fromDate && x.CreatedAt <= toDate;
                return result;
            });
        }

        private IEnumerable<TaskDataModel> GetExpectedTasks(string userId, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var workspaces = TaskDataHelper.Workspaces;
            return this.statisiticFixture.Data.Item2.Where(x =>
            {
                var workspaceType = workspaces.First(w => w.Id == x.WorkspaceId).Type.ToString();
                var finishedDate = x.Annotators.First(a => a.AnnotatorId.ToString() == userId).Documents.Max(d => d.AnnotatedAt);
                return (string.IsNullOrWhiteSpace(workspaceTypeId) || workspaceType == workspaceTypeId) &&
                    (x.CreatedAt <= toDate && finishedDate >= fromDate) && x.IsDeleted == false;
            }).ToList();
        }
    }
}
