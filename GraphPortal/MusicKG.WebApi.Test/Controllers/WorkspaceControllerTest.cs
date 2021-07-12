using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Moq;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MusicKG.Service;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.Service.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MusicKG.WebApi.Test.Controllers
{
    public class WorkspaceControllerTest
    {
        private WorkspaceController systemUnderTest;

        private Mock<IWorkspaceService> workspaceServiceMock;

        public WorkspaceControllerTest()
        {
            this.workspaceServiceMock = new Mock<IWorkspaceService>();
            this.systemUnderTest = new WorkspaceController(this.workspaceServiceMock.Object);
        }

        [Fact]
        public async Task Create()
        {
            const string expectedWorkspaceName = "workspaceName";

            WorkspaceCreateBindingModel createBindingModel = new WorkspaceCreateBindingModel
            {
                Name = expectedWorkspaceName,
                Description = "",
                Language = LanguageEnum.Chinese,
                Type = ObjectId.GenerateNewId().ToString(),
                IsAutoMerging = false
            };

            WorkspaceServiceModel expectedServiceModel = new WorkspaceServiceModel
            {
                Name = expectedWorkspaceName,
                Description = "",
                Language = LanguageEnum.Chinese,
                Type = new WorkspaceTypeServiceModel { Id = ObjectId.GenerateNewId().ToString() },
                IsAutoMerging = false,
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "User1"
                }
            };

            workspaceServiceMock.Setup(x => x.CreateWorkspaceAsync(It.IsAny<WorkspaceCreateServiceModel>())).Returns(
                Task.FromResult(ObjectId.GenerateNewId().ToString()));

            await this.systemUnderTest.CreateWorkspace(createBindingModel);

            workspaceServiceMock.Verify(s => s.CreateWorkspaceAsync(It.IsAny<WorkspaceCreateServiceModel>()), Times.Once);
        }

        [Fact]
        public async Task Update()
        {
            const string updateWorkspaceName = "workspaceName";

            WorkspaceUpdateBindingModel updateBindingModel = new WorkspaceUpdateBindingModel
            {
                Name = updateWorkspaceName,
                Description = "",
                ReadOnlyUserIds = new List<string>() { ObjectId.GenerateNewId().ToString() }
            };

            WorkspaceServiceModel expectedServiceModel = new WorkspaceServiceModel
            {
                Name = updateWorkspaceName,
                Description = "",
                Type = new WorkspaceTypeServiceModel { Id = ObjectId.GenerateNewId().ToString() },
                Language = LanguageEnum.Chinese,
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "User1"
                },
                IsAutoMerging = false,
                ReadOnlyUsers = new List<UserServiceModel>
                {
                    new UserServiceModel{Id = ObjectId.GenerateNewId().ToString(),Name = "ReadOnly"}
                }
            };

            workspaceServiceMock.Setup(x => x.UpdateWorkspaceAsync(It.IsAny<string>(), It.IsAny<WorkspaceUpdateServiceModel>())).Returns(Task.FromResult(expectedServiceModel));

            await systemUnderTest.UpdateWorkspace("workspaceId", updateBindingModel);

            workspaceServiceMock.Verify(s => s.UpdateWorkspaceAsync(It.IsAny<string>(), It.IsAny<WorkspaceUpdateServiceModel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateWithEmptyName()
        {
            WorkspaceUpdateBindingModel updateBindingModel = new WorkspaceUpdateBindingModel
            {
                Name = "",
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.systemUnderTest.UpdateWorkspace(
                ObjectId.GenerateNewId().ToString(), updateBindingModel));
            Assert.Equal(MusicKGMessages.WorkspaceNameEmptyMessage, exception.Message);
        }

        [Fact]
        public async Task GetWorkspace()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            string workspaceName = "workspaceName";

            var expectedServiceModel = new WorkspaceServiceModel
            {
                Id = workspaceId,
                Name = workspaceName,
                Type = new WorkspaceTypeServiceModel { Id = ObjectId.GenerateNewId().ToString() },
                Language = LanguageEnum.Chinese,
                Description = "",
                IsAutoMerging = true,
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "User1"
                },
                ReadOnlyUsers = new List<UserServiceModel>
                {
                    new UserServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "ReadOnlyName"
                    }
                }
            };

            this.workspaceServiceMock.Setup(x => x.GetWorkspaceAsync(workspaceId)).Returns(Task.FromResult(expectedServiceModel));

            var actual = await this.systemUnderTest.GetWorkspace(workspaceId);

            Assert.Equal(expectedServiceModel.Id, actual.Id);
            Assert.Equal(expectedServiceModel.Name, actual.Name);
            Assert.Equal(expectedServiceModel.Description, actual.Description);
            Assert.Equal(expectedServiceModel.Language, actual.Language);
            Assert.Equal(expectedServiceModel.Type.Id, actual.Type.Id);
            Assert.Equal(expectedServiceModel.IsAutoMerging, actual.IsAutoMerging);
            Assert.Equal(expectedServiceModel.ReadOnlyUsers.First().Id, actual.ReadOnlyUsers.First().Id);
            Assert.Equal(expectedServiceModel.ReadOnlyUsers.First().Name, actual.ReadOnlyUsers.First().Name);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        public async Task GetWorkspaces(int from, int? size)
        {
            var workspaces = Enumerable.Range(1, 3).Select(i => new WorkspaceServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"TestName{i}",
                Description = $"Description{i}",
                Language = i % 2 == 0 ? LanguageEnum.English : LanguageEnum.Chinese,
                Type = new WorkspaceTypeServiceModel { Id = ObjectId.GenerateNewId().ToString() },
                IsAutoMerging = i % 2 == 0 ? false : true,
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = $"User{i}"
                }
            }).ToList();

            var createdBy = workspaces.First().CreatedBy.Id;

            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, createdBy),
                new Claim(ClaimTypes.Role, string.Join(',', new List<UserRoleEnum>{ UserRoleEnum.Manager }))
            });

            var controller = new WorkspaceController(this.workspaceServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            var workspacesExpectedTotal = workspaces.Where(w => string.IsNullOrEmpty(createdBy) || w.CreatedBy.Id.Equals(createdBy));
            var workspacesexpected = workspacesExpectedTotal.Skip(from).Take(size ?? int.MaxValue);

            this.workspaceServiceMock.Setup(x => x.GetWorkspacesAsync(It.IsAny<LanguageEnum?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(Task.FromResult(
                new Tuple<long, IEnumerable<WorkspaceServiceModel>>(workspacesExpectedTotal.Count(), workspacesexpected)));

            var result = await controller.GetWorkspaces(from, size);

            Assert.NotNull(result);
            Assert.Equal(result.Count, result.Items.Count());
            Assert.Equal(workspacesexpected.Count(), result.Count);
            Assert.Equal(workspacesExpectedTotal.Count(), result.TotalCount);
            Assert.Equal(from, result.From);
            Assert.All(result.Items, w =>
            {
                var workspaceExpected = workspacesexpected.First(we => we.Id.Equals(w.Id));
                this.AssertViewModel(workspaceExpected, w);
            });
        }

        private void AssertViewModel(WorkspaceServiceModel expected, WorkspaceViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Language, actual.Language);
            Assert.Equal(expected.Type.Id, actual.Type.Id);
            Assert.Equal(expected.IsAutoMerging, actual.IsAutoMerging);
        }
    }
}
