using MongoDB.Bson;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Services;
using MusicKG.Workflow.Services.Implementations;
using MusicKG.Workflow.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MongoDB.Driver;
using MusicKG.Service;
using Moq;
using MusicKG.Service.Models;

namespace MusicKG.Workflow.Test.Services
{
    public class WorkflowDocumentServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        private readonly Mock<IStorageService> storageServiceMock;

        private readonly IWorkflowDocumentService serviceUnderTest;

        private readonly ObjectId workflowId = ObjectId.GenerateNewId();
        private readonly ObjectId preprocessStepId = ObjectId.GenerateNewId();
        private readonly ObjectId preannotatedStepId = ObjectId.GenerateNewId();

        private const string ContentString = "TestData";

        public WorkflowDocumentServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.storageServiceMock = new Mock<IStorageService>();
            this.serviceUnderTest = new WorkflowDocumentService(this.context, this.storageServiceMock.Object, null);

            this.storageServiceMock.Setup(x => x.Read(It.IsAny<string>())).Returns(Task.FromResult(Encoding.UTF8.GetBytes(ContentString)));
            this.storageServiceMock.Setup(x => x.Create(It.IsAny<DocumentStoreServiceModel>())).Returns(Task.CompletedTask);
        }

        [Theory]
        [InlineData(DocumentStatusEnum.Uploaded, StepTypeEnum.Uploaded, DocumentProcessStatusEnum.Failed, 3, StepTypeEnum.Preprocess, true)]
        [InlineData(DocumentStatusEnum.Uploaded, StepTypeEnum.Uploaded, DocumentProcessStatusEnum.Failed, 3, StepTypeEnum.Preprocess, false)]
        [InlineData(DocumentStatusEnum.Preprocessed, StepTypeEnum.Preprocess, DocumentProcessStatusEnum.Failed, 3, StepTypeEnum.Preannotate, true)]
        [InlineData(DocumentStatusEnum.Preprocessed, StepTypeEnum.Preprocess, DocumentProcessStatusEnum.Failed, 3, StepTypeEnum.Preannotate, false)]
        public async Task FetchDocuments(
            DocumentStatusEnum status, 
            StepTypeEnum birthStep, 
            DocumentProcessStatusEnum deadStatus, 
            int maxFailTimes,
            StepTypeEnum deadStep,
            bool enoughItemsToFetch)
        {
            string workflowIdUnderTest = ObjectId.GenerateNewId().ToString();
            string preprocessedStepId = ObjectId.GenerateNewId().ToString();
            string preannotatoeStepId = ObjectId.GenerateNewId().ToString();

            var initData = await this.PrepareDocuments(workflowIdUnderTest, preprocessedStepId, preannotatoeStepId);

            var expectedData = initData.Where(document =>
            {
                bool nextStepFilter = document.NextStep == null || (
                        document.NextStep.Status == deadStatus &&
                        document.NextStep.StepId == (deadStep == StepTypeEnum.Preprocess ? new ObjectId(preprocessedStepId) : new ObjectId(preannotatoeStepId)) &&
                        document.NextStep.Times < maxFailTimes);
                return document.Status == status &&
                    document.WorkflowId == new ObjectId(workflowIdUnderTest) &&
                    document.BirthStep.StepId == (birthStep == StepTypeEnum.Uploaded ? ObjectId.Empty : new ObjectId(preprocessedStepId)) &&
                    nextStepFilter;
            })?.ToList();

            var filter = new WorkflowStepInputFilterServiceModel
            {
                Status = new List<DocumentStatusEnum> { status },
                WorkflowId = workflowIdUnderTest,
                BirthStep = new BirthStepFilterDataModel
                {
                    Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed },
                    Steps = new List<ObjectId> { birthStep == StepTypeEnum.Uploaded ? ObjectId.Empty : new ObjectId(preprocessedStepId) },
                },
                DeathStep = new DeathStepDataModel
                {
                    Status = new List<DocumentProcessStatusEnum> { deadStatus },
                    MaxRetryTimes = maxFailTimes,
                    Steps = new List<ObjectId> { deadStep == StepTypeEnum.Preprocess ? new ObjectId(preprocessedStepId) : new ObjectId(preannotatoeStepId) }
                }
            };

            var result = await this.serviceUnderTest.FetchDocumentsAsync(filter, enoughItemsToFetch ? (expectedData.Count > 1 ? expectedData.Count - 1 : 1) : expectedData.Count + 1);

            Assert.NotNull(result);
            if (enoughItemsToFetch)
            {
                expectedData = expectedData.Take((expectedData.Count > 1 ? expectedData.Count - 1 : 1)).ToList();
            }

            Assert.Equal(expectedData.Count, result.Count);
            Assert.Equal(expectedData.Select(x => x.Id.ToString()), result);
        }

        [Theory]
        [InlineData(true, DocumentProcessStatusEnum.Succeed)]
        [InlineData(true, DocumentProcessStatusEnum.Failed)]
        [InlineData(false, DocumentProcessStatusEnum.Succeed)]
        [InlineData(false, DocumentProcessStatusEnum.Failed)]
        public async Task UpdateParentDocument(bool containsNextStep, DocumentProcessStatusEnum processStatus)
        {
            var expectedDocument = await this.PrepareSingleDocument(containsNextStep);
            string stepId = ObjectId.GenerateNewId().ToString();
            string errorMessage = "Internal server error.";

            var serviceModel = new DocumentProcessorServiceModel
            {
                DocumentId = expectedDocument.Id.ToString(),
                StartTime = DateTime.UtcNow.AddHours(-2),
                EndTime = DateTime.UtcNow.AddHours(-1),
                Status = processStatus,
                Message = processStatus == DocumentProcessStatusEnum.Succeed ? string.Empty : errorMessage
            };

            await this.serviceUnderTest.UpdateDocumentAsync(stepId, serviceModel);

            var document = await this.context.Documents.Find(x => x.Id == expectedDocument.Id).FirstOrDefaultAsync();

            Assert.NotNull(document);
            Assert.Equal(stepId, document.NextStep.StepId.ToString());
            Assert.Equal(processStatus, document.NextStep.Status);
            Assert.Equal(containsNextStep ? expectedDocument.NextStep.Times + 1 : 1, document.NextStep.Times);
            Assert.NotNull(document.NextStep.Histories);
            if (!containsNextStep)
            {
                Assert.Single(document.NextStep.Histories);
            }
            else
            {
                var lastHistory = document.NextStep.Histories.Last();
                Assert.Equal(expectedDocument.NextStep.Histories.Count + 1, document.NextStep.Histories.Count);
                Assert.Equal(serviceModel.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), lastHistory.StartedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                Assert.Equal(serviceModel.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), lastHistory.FinishedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                Assert.Equal(serviceModel.Status, lastHistory.Status);
                Assert.Equal(serviceModel.Message, lastHistory.Message);
            }
        }

        [Fact]
        public async Task GetDocumentContent()
        {
            var preparedData = await this.PrepareSingleDocument(false);

            var result = await this.serviceUnderTest.GetDocumentContentAsync(preparedData.Id.ToString());

            Assert.Equal(preparedData.Id.ToString(), result.Id);
            Assert.Equal(preparedData.Name, result.Name);
            Assert.Equal(preparedData.Tags, result.Tags);
            Assert.Equal(preparedData.WorkflowId.ToString(), result.WorkflowId);
            Assert.Equal(preparedData.WorkspaceId.ToString(), result.WorkspaceId);
            Assert.Equal(Encoding.UTF8.GetBytes(ContentString), result.Content);
            Assert.Equal(preparedData.ContentType, result.ContentType);
        }

        [Fact]
        public async Task CreateDocument()
        {
            var expectedData = new WorkflowDocumentCreateServiceModel
            {
                ParentId = ObjectId.GenerateNewId().ToString(),
                Status = DocumentStatusEnum.Preprocessed,
                Content = Encoding.UTF8.GetBytes(ContentString),
                BirthStep = ObjectId.GenerateNewId().ToString(),
                ParentName = "ParentDocumentName",
                ContentType = "text/plain",
                ParentTags = new List<string> { "Tags From Parent" },
                ParentWorkflowId = ObjectId.GenerateNewId().ToString(),
                ParentWorkspaceId = ObjectId.GenerateNewId().ToString()
            };

            var result = await this.serviceUnderTest.CreateDocumentAsync(expectedData);

            Assert.NotNull(result);

            var actualData = await this.context.Documents.Find(x => x.Id == new ObjectId(result)).FirstOrDefaultAsync();

            Assert.NotNull(actualData);
            Assert.Equal(result, actualData.Id.ToString());
            Assert.Equal(expectedData.ParentName, actualData.Name);
            Assert.Equal(expectedData.ParentTags, actualData.Tags);
            Assert.Equal(expectedData.ParentWorkflowId, actualData.WorkflowId.ToString());
            Assert.Equal(expectedData.ParentWorkspaceId, actualData.WorkspaceId.ToString());
            Assert.Equal(expectedData.BirthStep, actualData.BirthStep.StepId.ToString());
            Assert.Equal(expectedData.Status, actualData.Status);
            Assert.Equal(HashHelper.GetMD5Hash(expectedData.Content), actualData.ContentMd5);
            Assert.Equal(expectedData.ContentType, actualData.ContentType);
        }

        private async Task<DocumentDataModel> PrepareSingleDocument(bool containsNextStep)
        {
            var document = new DocumentDataModel
            {
                Name = "DocumentUploaded_DifferentWorkflow",
                Status = DocumentStatusEnum.Uploaded,
                WorkflowId = ObjectId.GenerateNewId(),
                BirthStep = new DocumentProcessStepDataModel
                {
                    StepId = ObjectId.Empty,
                    Status = DocumentProcessStatusEnum.Succeed
                },
                NextStep = containsNextStep ? new DocumentProcessStepDataModel
                {
                    StepId = ObjectId.GenerateNewId(),
                    Status = DocumentProcessStatusEnum.Failed,
                    Times = 1,
                    Histories = new List<DocumentProcessHistoryDataModel>
                    {
                        new DocumentProcessHistoryDataModel
                        {
                            Status = DocumentProcessStatusEnum.Failed,
                            Message = "Internal server error.",
                            StartedAt = DateTime.UtcNow.AddHours(-4),
                            FinishedAt = DateTime.UtcNow.AddHours(-3)
                        }
                    }
                } : null
            };

            await this.context.Documents.InsertOneAsync(document);

            return document;
        }

        private async Task<List<DocumentDataModel>> PrepareDocuments(
            string workflowId, 
            string preprocessStepId, 
            string preannotateStepId)
        {
            var documentsUploaded = Enumerable.Range(1, 20).Select(i => new DocumentDataModel
            {
                Name = $"Document Uploaded #{i}",
                Status = DocumentStatusEnum.Uploaded,
                WorkflowId = i % 2 == 0 ? ObjectId.GenerateNewId() : new ObjectId(workflowId),
                BirthStep = new DocumentProcessStepDataModel
                {
                    StepId = ObjectId.Empty,
                    Status = DocumentProcessStatusEnum.Succeed
                },
                NextStep = i % 2 == 0 ? null : this.CreateStepDataModel(preprocessStepId)
            });

            var documentPreprocessed = Enumerable.Range(1, 20).Select(i => new DocumentDataModel
            {
                Name = $"Document Preprocessed #{i}",
                Status = DocumentStatusEnum.Preprocessed,
                WorkflowId = i % 2 == 0 ? ObjectId.GenerateNewId() : new ObjectId(workflowId),
                BirthStep = new DocumentProcessStepDataModel
                {
                    StepId = new ObjectId(preprocessStepId),
                    Status = DocumentProcessStatusEnum.Succeed
                },
                NextStep = i % 2 == 0 ? null : this.CreateStepDataModel(preannotateStepId)
            });

            var documentPreannotated = Enumerable.Range(1, 20).Select(i => new DocumentDataModel
            {
                Name = $"Document Preannotated #{i}",
                Status = DocumentStatusEnum.Preannotated,
                WorkflowId = i % 2 == 0 ? ObjectId.GenerateNewId() : new ObjectId(workflowId),
                BirthStep = new DocumentProcessStepDataModel
                {
                    StepId = new ObjectId(preannotateStepId),
                    Status = DocumentProcessStatusEnum.Succeed
                },
                NextStep = null
            });

            var documents = new List<DocumentDataModel>();
            documents.AddRange(documentsUploaded);
            documents.AddRange(documentPreprocessed);
            documents.AddRange(documentPreannotated);

            await this.context.Documents.InsertManyAsync(documents);

            return documents;
        }

        private DocumentProcessStepDataModel CreateStepDataModel(string stepId)
        {
            DocumentProcessStatusEnum status = (DocumentProcessStatusEnum)(new Random().Next(0, 2));
            int times = new Random().Next(1, 3);
            return new DocumentProcessStepDataModel
            {
                Status = status,
                StepId = new ObjectId(stepId),
                Times = times,
                Histories = Enumerable.Range(1, times).Select(x => new DocumentProcessHistoryDataModel
                {
                    Status = DocumentProcessStatusEnum.Failed,
                    Message = "Server Internal Error",
                    StartedAt = DateTime.UtcNow.AddHours(-x),
                    FinishedAt = DateTime.UtcNow.AddHours(-x + 1)
                }).ToList()
            };
        }

        public enum StepTypeEnum
        {
            Uploaded,

            Preprocess,

            Preannotate,
        }
    }
}
