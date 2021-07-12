using MongoDB.Bson;
using Moq;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.BindingModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MusicKG.Service.Resources;
using MusicKG.Service.Helpers;

namespace MusicKG.WebApi.Test.Controllers
{
    public class WorkspaceTypeControllerTest
    {
        private readonly WorkspaceTypeController controllerUnderTest;

        private readonly Mock<IWorkspaceTypeService> workspaceTypeServiceMock;

        public WorkspaceTypeControllerTest()
        {
            this.workspaceTypeServiceMock = new Mock<IWorkspaceTypeService>();
            this.controllerUnderTest = new WorkspaceTypeController(this.workspaceTypeServiceMock.Object);
        }

        [Fact]
        public async Task Create()
        {
            var bindingModel = new WorkspaceTypeCreateBindingModel
            {
                Name = "WorkspaceType1",
                Status = WorkspaceTypeStatusEnum.Enabled,
                WorkflowId = ObjectId.GenerateNewId().ToString()
            };

            var serviceModel = new WorkspaceTypeServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = bindingModel.Name,
                WorkflowId = bindingModel.WorkflowId,
                WorkflowName = "WorkflowName1"
            };

            this.workspaceTypeServiceMock.Setup(x => x.CreateWorkspaceTypeAsync(
                It.IsAny<WorkspaceTypeCreateServiceModel>())).Returns(Task.FromResult(serviceModel));

            var viewModel = await this.controllerUnderTest.CreateWorkspaceType(bindingModel);

            Assert.NotNull(viewModel);
            Assert.Equal(serviceModel.Id, viewModel.Id);
            Assert.Equal(serviceModel.Name, viewModel.Name);
            Assert.Equal(serviceModel.WorkflowName, viewModel.WorkflowName);
        }

        [Fact]
        public async Task CreateWithEmptyName()
        {
            var bindingModel = new WorkspaceTypeCreateBindingModel
            {
                Name = "",
                Status = WorkspaceTypeStatusEnum.Enabled,
                WorkflowId = ObjectId.GenerateNewId().ToString()
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.CreateWorkspaceType(
                bindingModel));
            Assert.Equal(MusicKGMessages.WorkspaceTypeNameEmptyMessage, exception.Message);
        }

        [Fact]
        public async Task Update()
        {
            var bindingModel = new WorkspaceTypeUpdateBindingModel
            {
                Name = "WorkspaceType1",
                Status = WorkspaceTypeStatusEnum.Enabled
            };

            var serviceModel = new WorkspaceTypeServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = bindingModel.Name,
                WorkflowId = ObjectId.GenerateNewId().ToString(),
                WorkflowName = "WorkflowName1"
            };

            this.workspaceTypeServiceMock.Setup(x => x.UpdateWorkspaceTypeAsync(It.IsAny<string>(),
                It.IsAny<WorkspaceTypeUpdateServiceModel>())).Returns(Task.FromResult(serviceModel));

            var viewModel = await this.controllerUnderTest.UpdateWorkspaceType(ObjectId.GenerateNewId().ToString(), bindingModel);

            Assert.NotNull(viewModel);
            Assert.Equal(serviceModel.Id, viewModel.Id);
            Assert.Equal(serviceModel.Name, viewModel.Name);
            Assert.Equal(serviceModel.WorkflowName, viewModel.WorkflowName);
        }

        [Fact]
        public async Task UpdateWithEmptyName()
        {
            var bindingModel = new WorkspaceTypeUpdateBindingModel
            {
                Name = "",
                Status = WorkspaceTypeStatusEnum.Enabled,
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateWorkspaceType(
                ObjectId.GenerateNewId().ToString(), bindingModel));
            Assert.Equal(MusicKGMessages.WorkspaceTypeNameEmptyMessage, exception.Message);
        }

        [Fact]
        public async Task GetWorkspaceType()
        {
            var serviceModel = new WorkspaceTypeServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "WorkspaceType1",
                WorkflowId = ObjectId.GenerateNewId().ToString(),
                WorkflowName = "WorkflowName1"
            };

            this.workspaceTypeServiceMock.Setup(x => x.GetWorkspaceTypeAsync(It.IsAny<string>())).Returns(Task.FromResult(serviceModel));

            var viewModel = await this.controllerUnderTest.GetWorkspaceType(ObjectId.GenerateNewId().ToString());

            Assert.NotNull(viewModel);
            Assert.Equal(serviceModel.Id, viewModel.Id);
            Assert.Equal(serviceModel.Name, viewModel.Name);
            Assert.Equal(serviceModel.WorkflowName, viewModel.WorkflowName);
        }
        [Fact]
        public async Task GetWorkspaceTypes()
        {
            var serviceModels = new WorkspaceTypeServiceModel[]
            {
                new WorkspaceTypeServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "WorkspaceType1",
                    WorkflowId = ObjectId.GenerateNewId().ToString(),
                    WorkflowName = "WorkflowName1"
                },
                new WorkspaceTypeServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "WorkspaceType2",
                    WorkflowId = ObjectId.GenerateNewId().ToString(),
                    WorkflowName = "WorkflowName2"
                },
                new WorkspaceTypeServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "WorkspaceType3",
                    WorkflowId = ObjectId.GenerateNewId().ToString(),
                    WorkflowName = "WorkflowName3"
                },
                new WorkspaceTypeServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "WorkspaceType4",
                    WorkflowId = ObjectId.GenerateNewId().ToString(),
                    WorkflowName = "WorkflowName4"
                },
            };

            this.workspaceTypeServiceMock.Setup(x => x.GetWorkspaceTypesAsync(
                It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<WorkspaceTypeStatusEnum>())).Returns(Task.FromResult(
                new Tuple<long, IEnumerable<WorkspaceTypeServiceModel>>(serviceModels.Length, serviceModels)));

            var viewModels = await this.controllerUnderTest.GetWorkspaceTypes(0, null);

            Assert.NotNull(viewModels);
            Assert.Equal(serviceModels.Length, viewModels.TotalCount);
            Assert.Equal(serviceModels.Length, viewModels.Count);
            Assert.Equal(0, viewModels.From);

            var viewModelArray = viewModels.Items.ToArray();

            for (int i = 0; i < viewModels.Count; i++)
            {
                Assert.Equal(serviceModels[i].Id, viewModelArray[i].Id);
                Assert.Equal(serviceModels[i].Name, viewModelArray[i].Name);
                Assert.Equal(serviceModels[i].WorkflowName, viewModelArray[i].WorkflowName);
            }
        }
    }
}
