using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Services;
using MusicKG.Workflow.Services.Implementations;
using MusicKG.Workflow.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.Workflow.Test.Services
{
    public class WorkflowHostTest
    {
        [Fact]
        public async Task RunWithAllWorkflows()
        {
            var workflowServiceMock = new Mock<IWorkflowService>();
            var workflowProcessServiceMock = new Mock<IWorkflowProcessService>();

            var allWorkflows = new Tuple<long, IEnumerable<WorkflowServiceModel>>(2, new List<WorkflowServiceModel>
            {
                new WorkflowServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "TestWorkflow1",
                    Steps = new List<WorkflowStepServiceModel>
                    {
                        new WorkflowStepServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        },
                        new WorkflowStepServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        },
                        new WorkflowStepServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        }
                    }
                },
                new WorkflowServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "TestWorkflow2",
                    Steps = new List<WorkflowStepServiceModel>
                    {
                        new WorkflowStepServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        },
                        new WorkflowStepServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        },
                        new WorkflowStepServiceModel
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        }
                    }
                }
            });

            workflowServiceMock.Setup(x => x.GetWorkflowsAsync(0, null)).Returns(Task.FromResult(allWorkflows));

            var host = new WorkflowDaemon(Options.Create(new SupportedWorkflowSettings()), workflowServiceMock.Object, workflowProcessServiceMock.Object, null);

            await host.Run();

            workflowServiceMock.Verify(x => x.GetWorkflowsAsync(0, null), Times.Once);

            workflowServiceMock.Verify(x => x.GetWorkflowAsync(It.IsAny<string>()), Times.Never);

            workflowProcessServiceMock.Verify(x => x.Start(It.IsAny<WorkflowProcessorServiceModel>()), Times.Exactly(2));
        }

        [Fact]
        public async Task RunWithConfiguredWorkflows()
        {
            var workflowServiceMock = new Mock<IWorkflowService>();
            var workflowProcessServiceMock = new Mock<IWorkflowProcessService>();

            var configuredWorkflows = Enumerable.Range(1, 3).Select(x => new WorkflowServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"TestWorkflow{x}",
                Steps = new List<WorkflowStepServiceModel>
                {
                    new WorkflowStepServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString()
                    },
                    new WorkflowStepServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString()
                    }
                }
            }).ToList();

            var workflowConfiguration = new SupportedWorkflowSettings();
            workflowConfiguration.SupportedWorkflows.AddRange(configuredWorkflows.Select(x => new WorkflowSettings
            {
                WorkflowId = x.Id,
                SupportedStepIds = x.Steps.Select(y => y.Id).ToList()
            }));

            workflowServiceMock.Setup(x => x.GetWorkflowAsync(configuredWorkflows[0].Id)).Returns(Task.FromResult(configuredWorkflows[0]));
            workflowServiceMock.Setup(x => x.GetWorkflowAsync(configuredWorkflows[1].Id)).Returns(Task.FromResult(configuredWorkflows[1]));
            workflowServiceMock.Setup(x => x.GetWorkflowAsync(configuredWorkflows[2].Id)).Returns(Task.FromResult(configuredWorkflows[2]));

            var host = new WorkflowDaemon(Options.Create(workflowConfiguration), workflowServiceMock.Object, workflowProcessServiceMock.Object, null);

            await host.Run();

            workflowServiceMock.Verify(x => x.GetWorkflowsAsync(0, null), Times.Never);

            workflowServiceMock.Verify(x => x.GetWorkflowAsync(It.IsAny<string>()), Times.Exactly(3));

            workflowProcessServiceMock.Verify(x => x.Start(It.IsAny<WorkflowProcessorServiceModel>()), Times.Exactly(3));
        }

        [Fact]
        public async Task RunWithNoWorkflow()
        {
            var workflowServiceMock = new Mock<IWorkflowService>();
            var workflowProcessServiceMock = new Mock<IWorkflowProcessService>();

            var allWorkflows = new Tuple<long, IEnumerable<WorkflowServiceModel>>(0, null);

            workflowServiceMock.Setup(x => x.GetWorkflowsAsync(0, null)).Returns(Task.FromResult(allWorkflows));

            var host = new WorkflowDaemon(Options.Create(new SupportedWorkflowSettings()), workflowServiceMock.Object, workflowProcessServiceMock.Object, null);

            await host.Run();

            workflowServiceMock.Verify(x => x.GetWorkflowsAsync(0, null), Times.Once);

            workflowServiceMock.Verify(x => x.GetWorkflowAsync(It.IsAny<string>()), Times.Never);

            workflowProcessServiceMock.Verify(x => x.Start(It.IsAny<WorkflowProcessorServiceModel>()), Times.Never);
        }
    }
}
