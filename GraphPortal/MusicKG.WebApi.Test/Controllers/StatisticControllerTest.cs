using MongoDB.Bson;
using Moq;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using static MusicKG.Service.Helpers.ErrorHelper;

namespace MusicKG.WebApi.Test.Controllers
{
    public class StatisticControllerTest
    {
        [Fact]
        public async Task GetStatisiticsOverview()
        {
            var serviceMock = new Mock<IStatisticService>();

            IEnumerable<StatisticsOverviewServiceModel> serviceResult = new StatisticsOverviewServiceModel[]
            {
                new StatisticsOverviewServiceModel
                {
                    User = new UserServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "TestManager",
                        Roles = new List<UserRoleEnum> { UserRoleEnum.Manager }
                    },
                    Statistics = new StatisticsServiceModel
                    {
                        CreatedTaskCount = 1,
                        FinishedDocumentCount = 2,
                        NotFinishedDocumentCount = 3,
                        FinishedTaskCount = 5,
                        NotFinishedTaskCount = 7
                    }
                },
                new StatisticsOverviewServiceModel
                {
                    User = new UserServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "TestAnnotator1",
                        Roles = new List<UserRoleEnum> { UserRoleEnum.Annotator }
                    },
                    Statistics = new StatisticsServiceModel
                    {
                        CreatedTaskCount = 8,
                        FinishedDocumentCount = 9,
                        NotFinishedDocumentCount = 10,
                        FinishedTaskCount = 12,
                        NotFinishedTaskCount = 14
                    }
                },
                new StatisticsOverviewServiceModel
                {
                    User = new UserServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "TestAnnotator2",
                        Roles = new List<UserRoleEnum> { UserRoleEnum.Annotator }
                    },
                    Statistics = new StatisticsServiceModel
                    {
                        CreatedTaskCount = 15,
                        FinishedDocumentCount = 16,
                        NotFinishedDocumentCount = 17,
                        FinishedTaskCount = 19,
                        NotFinishedTaskCount = 21
                    }
                },
            };

            serviceMock.Setup(x => x.GetStatisticsOverviewAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(serviceResult));

            var controller = new StatisticController(serviceMock.Object);

            var result = await controller.GetStatisiticsOverview(DateTime.UtcNow.AddHours(-5), DateTime.UtcNow, null);

            serviceMock.Verify(x => x.GetStatisticsOverviewAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            Assert.NotNull(result);
            Assert.NotNull(result.Annotators);
            Assert.NotNull(result.Managers);

            var expectedManagers = serviceResult.Where(y => y.User.Roles.Contains(UserRoleEnum.Manager));
            var expectedAnnotators = serviceResult.Where(y => y.User.Roles.Contains(UserRoleEnum.Annotator));

            Assert.Equal(serviceResult.Sum(x => x.Statistics.FinishedTaskCount), result.AnnotatedTaskCount);
            Assert.Equal(serviceResult.Sum(x => x.Statistics.FinishedDocumentCount), result.AnnotatedDocumentCount);

            Assert.All(result.Managers, x =>
            {
                var manager = expectedManagers.First(y => y.User.Id == x.User.Id);
                this.AssertViewModel(x, manager);
            });

            Assert.All(result.Annotators, x =>
            {
                var annotator = expectedAnnotators.First(y => y.User.Id == x.User.Id);
                this.AssertViewModel(x, annotator);
            });
        }
        [Fact]
        public async Task GetStatisiticsDetails()
        {
            var serviceMock = new Mock<IStatisticService>();

            StatisticsDetailsServiceModel serviceResult = new StatisticsDetailsServiceModel
            {
                User = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "TestUser",
                    Roles = new List<UserRoleEnum> { UserRoleEnum.Manager }
                },
                TaskStatisticsDetails = new List<TaskStatisticsDetailsServiceModel>
                {
                    new TaskStatisticsDetailsServiceModel
                    {
                        TaskId = ObjectId.GenerateNewId().ToString(),
                        TaskName = "TestTask1",
                        CreatedAt = DateTime.Now,
                        ExpectedDueAt = DateTime.Now.AddDays(3),
                        RelatedUsers = new List<TaskAnnotatorServiceModel>
                        {
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestManager"
                                },
                                IsManager = true
                            },
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestAnnotator1"
                                },
                                IsManager = false
                            },
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestAnnotator2"
                                },
                                IsManager = false
                            }
                        },
                        Workspace = new WorkspaceServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Name = "TestWorkspace1",
                            Type = new WorkspaceTypeServiceModel
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                Name = "TestWorkspaceType1"
                            }
                        }
                    },
                    new TaskStatisticsDetailsServiceModel
                    {
                        TaskId = ObjectId.GenerateNewId().ToString(),
                        TaskName = "TestTask2",
                        CreatedAt = DateTime.Now,
                        ExpectedDueAt = DateTime.Now.AddDays(3),
                        RelatedUsers = new List<TaskAnnotatorServiceModel>
                        {
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestManager1"
                                },
                                IsManager = true
                            },
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestAnnotator11"
                                },
                                IsManager = false
                            },
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestAnnotator21"
                                },
                                IsManager = false
                            }
                        },
                        Workspace = new WorkspaceServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Name = "TestWorkspace2",
                            Type = new WorkspaceTypeServiceModel
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                Name = "TestWorkspaceType2"
                            }
                        }
                    },
                    new TaskStatisticsDetailsServiceModel
                    {
                        TaskId = ObjectId.GenerateNewId().ToString(),
                        TaskName = "TestTask3",
                        CreatedAt = DateTime.Now,
                        ExpectedDueAt = DateTime.Now.AddDays(3),
                        RelatedUsers = new List<TaskAnnotatorServiceModel>
                        {
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestManager31"
                                },
                                IsManager = true
                            },
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestAnnotator31"
                                },
                                IsManager = false
                            },
                            new TaskAnnotatorServiceModel
                            {
                                Annotator = new UserServiceModel
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Name = "TestAnnotator32"
                                },
                                IsManager = false
                            }
                        },
                        Workspace = new WorkspaceServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Name = "TestWorkspace3",
                            Type = new WorkspaceTypeServiceModel
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                Name = "TestWorkspaceType3"
                            }
                        }
                    }
                }
            };

            var userId = ObjectId.GenerateNewId().ToString();

            serviceMock.Setup(x => x.GetStatisticsDetailsAsync(userId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(serviceResult));

            var controller = new StatisticController(serviceMock.Object);

            var result = await controller.GetStatisiticsDetails(userId, DateTime.UtcNow.AddHours(-5), DateTime.UtcNow, null);

            serviceMock.Verify(x => x.GetStatisticsDetailsAsync(userId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            Assert.NotNull(result);
            Assert.NotNull(result.User);
            Assert.NotNull(result.StatisiticsDetails);

            Assert.Equal(serviceResult.User.Id, result.User.Id);
            Assert.Equal(serviceResult.User.Name, result.User.Name);
            Assert.Equal(serviceResult.User.Roles, result.User.Roles);
            Assert.Equal(serviceResult.TaskStatisticsDetails.Count(), result.StatisiticsDetails.Count());
            Assert.All(result.StatisiticsDetails, x =>
            {
                var expectedResult = serviceResult.TaskStatisticsDetails.First(t => t.TaskId == x.TaskId);
                Assert.Equal(expectedResult.TaskName, x.TaskName);
                Assert.Equal(expectedResult.CreatedAt, x.CreatedAt);
                Assert.Equal(expectedResult.ExpectedDueAt, x.ExpectedDueAt);
                Assert.Equal(expectedResult.FinishedDocumentCount, x.FinishedDocumentCount);
                Assert.Equal(expectedResult.Workspace.Id, x.Workspace.Id);
                Assert.Equal(expectedResult.Workspace.Name, x.Workspace.Name);
                Assert.Equal(expectedResult.Workspace.Type.Id, x.Workspace.Type.Id);
                Assert.Equal(expectedResult.Workspace.Type.Name, x.Workspace.Type.Name);
                Assert.All(x.Annotators, a =>
                {
                    var expectedAnnotator = expectedResult.RelatedUsers.First(ru => ru.Annotator.Id == a.AnnotatorId);
                    Assert.Equal(expectedAnnotator.Annotator.Name, a.AnnotatorName);
                    Assert.Equal(expectedAnnotator.IsManager, a.IsManager);
                });
            });
        }


        [Fact]
        public async Task GetStatisiticsOverviewWithInvalidDate()
        {
            var serviceMock = new Mock<IStatisticService>();

            IEnumerable<StatisticsOverviewServiceModel> serviceResult = new List<StatisticsOverviewServiceModel>();

            serviceMock.Setup(x => x.GetStatisticsOverviewAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(serviceResult));

            var controller = new StatisticController(serviceMock.Object);

            var exception = await Assert.ThrowsAsync<ErrorMessageException>(() => controller.GetStatisiticsOverview(DateTime.UtcNow.AddHours(5), DateTime.UtcNow, null));

            serviceMock.Verify(x => x.GetStatisticsOverviewAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);

            Assert.Equal(Service.Resources.MusicKGMessages.FromDateGreaterThanToDateMessage, exception.Message);
        }

        [Fact]
        public async Task GetStatisiticsDetailsWithInvalidDate()
        {
            var serviceMock = new Mock<IStatisticService>();

            StatisticsDetailsServiceModel serviceResult = new StatisticsDetailsServiceModel();

            serviceMock.Setup(x => x.GetStatisticsDetailsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(serviceResult));

            var controller = new StatisticController(serviceMock.Object);

            var exception = await Assert.ThrowsAsync<ErrorMessageException>(() => controller.GetStatisiticsDetails(It.IsAny<string>(), DateTime.UtcNow.AddHours(5), DateTime.UtcNow, null));

            serviceMock.Verify(x => x.GetStatisticsDetailsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);

            Assert.Equal(Service.Resources.MusicKGMessages.FromDateGreaterThanToDateMessage, exception.Message);
        }

        private void AssertViewModel(StatisiticsViewModel viewModel, StatisticsOverviewServiceModel serviceModel)
        {
            Assert.Equal(serviceModel.User.Name, viewModel.User.Name);
            Assert.Equal(serviceModel.User.Roles, viewModel.User.Roles);
            Assert.Equal(serviceModel.Statistics.CreatedTaskCount, viewModel.Statisitics.CreatedTaskCount);
            Assert.Equal(serviceModel.Statistics.FinishedDocumentCount, viewModel.Statisitics.FinishedDocumentCount);
            Assert.Equal(serviceModel.Statistics.FinishedTaskCount, viewModel.Statisitics.FinishedTaskCount);
            Assert.Equal(serviceModel.Statistics.NotFinishedDocumentCount, viewModel.Statisitics.NotFinishedDocumentCount);
            Assert.Equal(serviceModel.Statistics.NotFinishedTaskCount, viewModel.Statisitics.NotFinishedTaskCount);
        }
    }
}
