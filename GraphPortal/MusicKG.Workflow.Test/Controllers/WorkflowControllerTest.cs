using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MusicKG.Workflow.Controllers;
using MongoDB.Bson;
using MusicKG.Workflow.Services;
using MusicKG.Workflow.Models.ServiceModels;

namespace MusicKG.Workflow.Test.Controllers
{
    public class WorkflowControllerTest
    {
        private readonly Mock<IWorkflowService> workflowManagementServiceMock;

        private readonly WorkflowController controllerUnderTest;

        public WorkflowControllerTest()
        {
            this.workflowManagementServiceMock = new Mock<IWorkflowService>();
            this.controllerUnderTest = new WorkflowController(this.workflowManagementServiceMock.Object);
        }

        [Fact]
        public async Task GetWorkflows()
        {
            var serviceModels = new WorkflowServiceModel[]
            {
                new WorkflowServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Workflow1"
                },
                new WorkflowServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Workflow2"
                },
                new WorkflowServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Workflow3"
                },
                new WorkflowServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Workflow4"
                },
            };

            this.workflowManagementServiceMock.Setup(x => x.GetWorkflowsAsync(
                It.IsAny<int>(), It.IsAny<int?>())).Returns(Task.FromResult(
                    new Tuple<long, IEnumerable<WorkflowServiceModel>>(
                        serviceModels.Length, serviceModels)));

            var viewModels = await this.controllerUnderTest.GetWorkflows(0, null);
            var viewModelArray = viewModels.Items.ToArray();

            Assert.NotNull(viewModels);
            Assert.Equal(serviceModels.Length, viewModels.TotalCount);
            Assert.Equal(serviceModels.Length, viewModels.Count);
            Assert.Equal(0, viewModels.From);

            for (int i = 0; i < viewModels.Count; i++)
            {
                Assert.Equal(serviceModels[i].Id, viewModelArray[i].Id);
                Assert.Equal(serviceModels[i].Name, viewModelArray[i].Name);
            }
        }
    }
}
