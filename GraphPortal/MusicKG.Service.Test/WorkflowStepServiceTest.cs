using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Moq;
using System.Linq;
using MusicKG.Service.Test.Fixtures;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Test.Helpers;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    public class WorkflowStepServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private IMusicKGContext context;

        public WorkflowStepServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetNextWorkflowStep(int? parentStepIndex)
        {
            var (workflows, workspaceTypes, workspaces, documents) = await DocumentDataHelper.PrepareDocumentsData(context);

            var workflow = workflows.First();
            var workspaceType = workspaceTypes.Where(wt => wt.WorkflowId == workflow.Id).First();
            var workspace = workspaces.Where(w => w.Type == workspaceType.Id).First();
            var documentsExpected = documents.Where(d => d.WorkflowId == workflow.Id);

            var workflowService = new WorkflowStepService(context, null);

            var step = await workflowService.GetNextWorkflowStepAsync(workspace.Id.ToString(), parentStepIndex.HasValue ? documentsExpected.ElementAt(parentStepIndex.Value).Id.ToString() : null);

            Assert.NotNull(step);
            var stepExpected = workflow.Steps.ElementAtOrDefault(parentStepIndex.HasValue ? parentStepIndex.Value + 1 : 0);
            Assert.Equal(stepExpected.StepId.ToString(), step.Id);
            Assert.Equal(stepExpected.Name, step.Name);
            Assert.Equal(stepExpected.ResultDocumentStatus, step.ResultDocumentStatus);
            Assert.Equal(workflow.Id.ToString(), step.WorkflowId);
        }

        [Fact]
        public async Task GetPreannotationWorkflowStep()
        {
            var (workflows, workspaceTypes, workspaces, documents) = await DocumentDataHelper.PrepareDocumentsData(context);

            var workflow = workflows.First();
            var workspaceType = workspaceTypes.Where(wt => wt.WorkflowId == workflow.Id).First();
            var workspace = workspaces.Where(w => w.Type == workspaceType.Id).First();

            var stepExpected = workflow.Steps.FirstOrDefault(s => s.ResultDocumentStatus == DocumentStatusEnum.Preannotated);

            var workflowService = new WorkflowStepService(context, null);

            var step = await workflowService.GetPreannotationWorkflowStepAsync(workspace.Id.ToString());

            Assert.NotNull(step);
            Assert.Equal(stepExpected.StepId.ToString(), step.Id);
            Assert.Equal(stepExpected.Name, step.Name);
            Assert.Equal(stepExpected.ResultDocumentStatus, step.ResultDocumentStatus);
            Assert.Equal(workflow.Id.ToString(), step.WorkflowId);
        }

        [Fact]
        public async Task GetNextWorkflowStepWithInvalidParentStep()
        {
            var (workflows, workspaceTypes, workspaces, documents) = await DocumentDataHelper.PrepareDocumentsData(context);

            var workflow = workflows.First();
            var workspaceType = workspaceTypes.Where(wt => wt.WorkflowId == workflow.Id).First();
            var workspace = workspaces.Where(w => w.Type == workspaceType.Id).First();
            var documentsExpected = documents.Where(d => d.WorkflowId == workflow.Id);

            var workflowService = new WorkflowStepService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => workflowService.GetNextWorkflowStepAsync(workflow.Id.ToString(), documentsExpected.Last().Id.ToString()));

            Assert.Equal(MusicKGMessages.WorkflowStepGetFailedMessage, exception.Message);
        }
    }
}
