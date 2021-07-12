using MongoDB.Bson;
using Moq;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Services;
using MusicKG.Workflow.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.Workflow.Test.Services
{
    public class WorkflowProcessServiceTest
    {
        private readonly IWorkflowProcessService serviceUnderTest;
        
        private Mock<IWorkflowStepProcessService> workflowStepProcessServiceMock;

        public WorkflowProcessServiceTest()
        {
            this.workflowStepProcessServiceMock = new Mock<IWorkflowStepProcessService>();
            this.serviceUnderTest = new WorkflowProcessService(this.workflowStepProcessServiceMock.Object, null);
            this.workflowStepProcessServiceMock.Setup(x => x.Start(It.IsAny<WorkflowStepServiceModel>())).Returns(Task.Delay(1));
        }

        [Fact]
        public async Task StartWorkflow()
        {
            var workflowId1 = ObjectId.GenerateNewId().ToString();
            await this.serviceUnderTest.Start(new Models.ServiceModels.WorkflowProcessorServiceModel
            {
                WorkflowId = workflowId1,
                AvailableSteps = new List<WorkflowStepServiceModel>
                {
                    new WorkflowStepServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        WorkflowId = workflowId1,
                        Name = "Step1",
                        ProcessorAssembly = "MusicKG.Workflow",
                        ProcessorClass = "MusicKG.Workflow.Services.Implementations.DocumentSplittingProcessor",
                        ResultDocumentStatus = MusicKG.DataAccess.Enums.DocumentStatusEnum.Preannotated
                    },
                    new WorkflowStepServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        WorkflowId = workflowId1,
                        Name = "Step2",
                        ProcessorAssembly = "MusicKG.Workflow",
                        ProcessorClass = "MusicKG.Workflow.Services.Implementations.DocumentSplittingProcessor",
                        ResultDocumentStatus = MusicKG.DataAccess.Enums.DocumentStatusEnum.Preannotated
                    }
                }
            });

            this.workflowStepProcessServiceMock.Verify(x => x.Start(It.IsAny<WorkflowStepServiceModel>()), Times.Exactly(2));
        }
    }
}
