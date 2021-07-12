using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MusicKG.Service.Test.Fixtures;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Helpers;
using System.Collections;
using MusicKG.Service.Resources;
using MusicKG.Service.SynchronizedWorkflow;
using MusicKG.Service.Constants;

namespace MusicKG.Service.Test
{
    public class DocumentServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        private readonly IDocumentService serviceUnderTest;

        private readonly Mock<IStorageService> fileStorageProviderMock;
        private readonly Mock<IWorkspaceService> workspaceServiceMock;
        private readonly Mock<IWorkflowStepService> workflowServiceMock;
        private readonly Mock<ISyncWorkflowStepProcessService> syncWorkflowStepProcessServiceMock;

        private const string ExpectedUploader = "TestUploader";

        public DocumentServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.fileStorageProviderMock = new Mock<IStorageService>();
            this.workspaceServiceMock = new Mock<IWorkspaceService>();
            this.workflowServiceMock = new Mock<IWorkflowStepService>();
            this.syncWorkflowStepProcessServiceMock = new Mock<ISyncWorkflowStepProcessService>();
            this.serviceUnderTest = new DocumentService(this.fileStorageProviderMock.Object, this.workspaceServiceMock.Object,
                this.context, this.workflowServiceMock.Object, syncWorkflowStepProcessServiceMock.Object);
        }

        #region Get

        #region Positive

        [Theory]
        [ClassData(typeof(ClassDataForGetDocuments))]
        public async Task GetDocuments(string keyword, string tag, IEnumerable<DocumentStatusEnum> statuses, DateTime? fromUploadedAt, DateTime? toUploadedAt, int from, int? size)
        {
            var (workflows, workspaceTypes, workspaces, documents) = await DocumentDataHelper.PrepareDocumentsData(context);

            var workspace = workspaces.First();
            var workflow = workflows.First(wf => wf.Id == workspaceTypes.First(w => w.Id == workspace.Type).WorkflowId);
            var uploadStep = workflow.Steps.Where(step => step.ResultDocumentStatus == DocumentStatusEnum.Preannotated).First();
            var documentsExpectedTotal = documents
                .Where(d => d.WorkspaceId == workspace.Id
                    && (string.IsNullOrWhiteSpace(keyword) || d.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
                    && (statuses.Count() == 0 || statuses.Contains(d.Status))
                    && (fromUploadedAt == null || d.UploadedAt.CompareTo(fromUploadedAt.Value) >= 0)
                    && (toUploadedAt == null || d.UploadedAt.CompareTo(toUploadedAt.Value) <= 0)
                    && d.BirthStep.StepId == uploadStep.StepId
                    && d.IsDeleted == false
                    && (string.IsNullOrWhiteSpace(tag) || d.Tags.Count(x => x.Contains(tag, StringComparison.CurrentCultureIgnoreCase)) > 0));

            var documentsExpected = documentsExpectedTotal.OrderByDescending(d => d.UploadedAt).Skip(from).Take(size ?? int.MaxValue);

            workspaceServiceMock.Setup(x => x.GetWorkspaceAsync(It.IsAny<string>())).Returns(Task.FromResult(new WorkspaceServiceModel
            {
                Type = new WorkspaceTypeServiceModel
                {
                    Id = workspace.Type.ToString(),
                    WorkflowId = workflow.Id.ToString()
                }
            }));

            workflowServiceMock.Setup(wf => wf
                .GetPreannotationWorkflowStepAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new WorkflowNextStepServiceModel
                {
                    Id = uploadStep.StepId.ToString(),
                    Name = uploadStep.Name,
                    ResultDocumentStatus = uploadStep.ResultDocumentStatus
                }));

            var (totalCount, documentsReturned) = await this.serviceUnderTest.GetDocumentsAsync(workspace.Id.ToString(), keyword, tag, fromUploadedAt, toUploadedAt, statuses, from, size);

            Assert.NotNull(documentsReturned);
            Assert.Equal(documentsExpectedTotal.Count(), totalCount);
            Assert.Equal(documentsExpected.Count(), documentsReturned.Count());

            Assert.Equal(documentsReturned.Select(t => t.UploadedAt.ToString()), documentsExpected.Select(t => t.UploadedAt.ToString()));

            Assert.All(documentsReturned, d =>
            {
                var documentExpected = documentsExpected.Where(de => de.Id == new ObjectId(d.Id)).First();
                AssertServiceModel(documentExpected, d);
            });
        }

        [Fact]
        public async Task GetDocument()
        {
            var (workflows, workspaceTypes, workspaces, documents) = await DocumentDataHelper.PrepareDocumentsData(context);

            var expectedDocument = documents.First();

            var actualResult = await this.serviceUnderTest.GetDocumentAsync(expectedDocument.WorkspaceId.ToString(),
                expectedDocument.Id.ToString());

            Assert.NotNull(actualResult);
            this.AssertServiceModel(expectedDocument, actualResult);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task GetDocumentWithInvalidWorkspaceId()
        {
            var (workflows, workspaceTypes, workspaces, documents) = await DocumentDataHelper.PrepareDocumentsData(context);

            var existedDocument = documents.First();

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.GetDocumentAsync(ObjectId.GenerateNewId().ToString(), existedDocument.Id.ToString()));
            Assert.Equal(MusicKGMessages.DocumentNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task GetDocumentWithInvalidDocumentId()
        {
            var (workflows, workspaceTypes, workspaces, documents) = await DocumentDataHelper.PrepareDocumentsData(context);

            var existedDocument = documents.First();

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.GetDocumentAsync(existedDocument.WorkspaceId.ToString(),
                ObjectId.GenerateNewId().ToString()));
            Assert.Equal(MusicKGMessages.DocumentNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Upload

        #region Positive

        [Theory]
        [InlineData(DocumentStatusEnum.Assigned, null)]
        [InlineData(DocumentStatusEnum.Preannotated, null)]
        [InlineData(DocumentStatusEnum.Preprocessed, null)]
        [InlineData(DocumentStatusEnum.Uploaded, null)]
        public async Task Upload(DocumentStatusEnum status, string parentDocumentId)
        {
            string expectedWorkspaceId = ObjectId.GenerateNewId().ToString();
            var expectedDocument = new DocumentUploadServiceModel
            {
                Name = "TestDocument",
                Content = Encoding.Default.GetBytes(RandomStringHelper.RandomString(32)),
                ContentType = "text/plain",
                Tags = new List<string> { "TestTag" },
                ItemCount = new Random().Next(0, 100),
                UploadedBy = ObjectId.GenerateNewId().ToString(),
                ParentDocumentId = parentDocumentId
            };

            this.workspaceServiceMock.Setup(x => x.GetWorkspaceAsync(It.IsAny<string>())).Returns(Task.FromResult(new WorkspaceServiceModel
            {
                Type = new WorkspaceTypeServiceModel { Id = ObjectId.GenerateNewId().ToString(), WorkflowId = ObjectId.GenerateNewId().ToString() }
            }));

            var workflowStep = new WorkflowNextStepServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "step",
                ResultDocumentStatus = status
            };
            this.workflowServiceMock.Setup(w => w.GetNextWorkflowStepAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(workflowStep));

            this.fileStorageProviderMock.Setup(x => x.Create(It.IsAny<DocumentStoreServiceModel>())).Returns(Task.CompletedTask);

            this.syncWorkflowStepProcessServiceMock.Setup(x => x.ProcessAsync(It.IsAny<string>(), workflowStep, expectedDocument.Content, expectedDocument.ContentType))
                .Returns(Task.FromResult((expectedDocument.Content, expectedDocument.ContentType, expectedDocument.ItemCount)));

            var documentId = await this.serviceUnderTest.UploadDocumentAsync(expectedWorkspaceId, expectedDocument);

            var result = this.context.Documents.Find(x => true).ToList();

            this.fileStorageProviderMock.Verify(x => x.Create(It.IsAny<DocumentStoreServiceModel>()), Times.Once);

            Assert.NotNull(result);
            Assert.Single(result);
            var actualDocumentDataModel = result.First();
            Assert.Equal(expectedDocument.Name, actualDocumentDataModel.Name);
            Assert.Equal(expectedDocument.ContentType, actualDocumentDataModel.ContentType);
            Assert.Equal(expectedDocument.UploadedBy, actualDocumentDataModel.UploadedBy.ToString());
            Assert.Equal(expectedDocument.Tags, actualDocumentDataModel.Tags);
            Assert.Equal(expectedDocument.ItemCount, actualDocumentDataModel.ItemCount);
            Assert.Equal(status, actualDocumentDataModel.Status);
            Assert.Equal(HashHelper.GetMD5Hash(expectedDocument.Content), actualDocumentDataModel.ContentMd5);
            Assert.Equal(workflowStep.Id, actualDocumentDataModel.BirthStep.StepId.ToString());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task UploadWithPreprocess(int stepCount)
        {
            var workflow = new WorkflowDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "MyWorkflow",
                Steps = Enumerable.Range(0, stepCount).Select(i => new WorkflowStepDataModel
                {
                    StepId = ObjectId.GenerateNewId(),
                    Name = $"step{i}",
                    ResultDocumentStatus = (DocumentStatusEnum)(i + 1),
                    AutoDoNext = stepCount == i + 1 ? false : true
                }).ToList(),
            };

            var workspaceType = new WorkspaceTypeDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "MyWorkspaceType",
                Status = WorkspaceTypeStatusEnum.Enabled,
                WorkflowId = workflow.Id
            };

            var user = new UserDataModel()
            {
                Id = ObjectId.GenerateNewId(),
                Name = "CreateUser"
            };

            var workspace = new WorkspaceDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "MyWorkspace",
                Type = workspaceType.Id,
                CreatedBy = user.Id
            };

            var rawContent = Encoding.UTF8.GetBytes("RawContent");
            var rawContentType = "text/palin";

            var contents = workflow.Steps.Select((step, i) =>
            {
                return (content: Encoding.UTF8.GetBytes($"Step_{step.Name}_Content"), contentType: "text/plain", itemCount: i);
            }).ToList();

            await context.Workflows.InsertOneAsync(workflow);
            await context.WorkspaceTypes.InsertOneAsync(workspaceType);
            await context.Users.InsertOneAsync(user);
            await context.Workspaces.InsertOneAsync(workspace);

            for (int i = 0; i < stepCount; i++)
            {
                if (i == 0)
                {
                    syncWorkflowStepProcessServiceMock.Setup(mock => mock.ProcessAsync(workspace.Id.ToString(), It.IsAny<WorkflowNextStepServiceModel>(), rawContent, rawContentType))
                        .Returns(Task.FromResult((contents[i].content, contents[i].contentType, Convert.ToInt64(contents[i].itemCount))));
                }
                else
                {
                    syncWorkflowStepProcessServiceMock.Setup(mock => mock.ProcessAsync(workspace.Id.ToString(), It.IsAny<WorkflowNextStepServiceModel>(), contents[i - 1].content, contents[i - 1].contentType))
                        .Returns(Task.FromResult((contents[i].content, contents[i].contentType, Convert.ToInt64(contents[i].itemCount))));
                }
            }

            this.fileStorageProviderMock.Setup(x => x.Create(It.IsAny<DocumentStoreServiceModel>())).Returns(Task.CompletedTask);

            var service = new DocumentService(fileStorageProviderMock.Object, new WorkspaceService(context, null), context, new WorkflowStepService(context, null), syncWorkflowStepProcessServiceMock.Object);

            var rawDocument = new DocumentUploadServiceModel
            {
                Name = "DocumentName",
                ParentDocumentId = null,
                Tags = new List<string> { "Test" },
                UploadedBy = ObjectId.GenerateNewId().ToString(),
                Content = rawContent,
                ContentType = rawContentType
            };

            var actualDocumentId = await service.UploadDocumentAsync(workspace.Id.ToString(), rawDocument);



            var actualDocument = await context.Documents.Find(d => d.Id == new ObjectId(actualDocumentId) && d.WorkspaceId == workspace.Id).FirstOrDefaultAsync();

            AssertDocument(actualDocument, rawDocument, workflow.Steps.Last(), contents.Last());
            
            if (stepCount == 1)
            {
                syncWorkflowStepProcessServiceMock.Verify(x => x.ProcessAsync(workspace.Id.ToString(), It.IsAny<WorkflowNextStepServiceModel>(), rawContent, rawContentType), Times.Once);

                Assert.Null(actualDocument.ParentId);
            }
            else
            {
                var document = actualDocument;
                for (int i = stepCount - 2; i >= 0; i--)
                {
                    syncWorkflowStepProcessServiceMock.Verify(x => x.ProcessAsync(workspace.Id.ToString(), It.IsAny<WorkflowNextStepServiceModel>(), contents[i].content, contents[i].contentType), Times.Once);

                    var parent = await context.Documents.Find(d => d.Id == document.ParentId && d.WorkspaceId == workspace.Id).FirstOrDefaultAsync();
                    Assert.NotNull(parent);
                    AssertDocument(parent, rawDocument, workflow.Steps[i], contents[i]);
                    document = parent;
                }
                Assert.Null(document.ParentId);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UploadDocumentResult(bool withPreprocess)
        {
            ObjectId preannotatedStepId = ObjectId.GenerateNewId();
            WorkflowDataModel workflow;
            if (withPreprocess)
            {
                workflow = new WorkflowDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "WorkflowWithPreprocess",
                    Steps = new List<WorkflowStepDataModel>
                    {
                        new WorkflowStepDataModel
                        {
                            StepId = ObjectId.GenerateNewId(),
                            Name = "Uploading",
                            AutoDoNext = true,
                            ResultDocumentStatus = DocumentStatusEnum.Uploaded
                        },
                        new WorkflowStepDataModel
                        {
                            StepId = preannotatedStepId,
                            Name = "Preannotated",
                            AutoDoNext = false,
                            ResultDocumentStatus = DocumentStatusEnum.Preannotated
                        },
                        new WorkflowStepDataModel
                        {
                            StepId = ObjectId.GenerateNewId(),
                            Name = "Annotation",
                            AutoDoNext = false,
                            ResultDocumentStatus = DocumentStatusEnum.Uploaded
                        }
                    }
                };
            }
            else
            {
                workflow = new WorkflowDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "WorkflowWithoutPreprocess",
                    Steps = new List<WorkflowStepDataModel>
                    {
                        new WorkflowStepDataModel
                        {
                            StepId = preannotatedStepId,
                            Name = "Uploading",
                            AutoDoNext = false,
                            ResultDocumentStatus = DocumentStatusEnum.Preannotated
                        },
                        new WorkflowStepDataModel
                        {
                            StepId = ObjectId.GenerateNewId(),
                            Name = "Annotation",
                            AutoDoNext = false,
                            ResultDocumentStatus = DocumentStatusEnum.Uploaded
                        }
                    }
                };
            }

            var workspaceType = new WorkspaceTypeDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "MyWorkspaceType",
                Status = WorkspaceTypeStatusEnum.Enabled,
                WorkflowId = workflow.Id
            };

            var user = new UserDataModel()
            {
                Id = ObjectId.GenerateNewId(),
                Name = "CreateUser"
            };

            var workspace = new WorkspaceDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "MyWorkspace",
                Type = workspaceType.Id,
                CreatedBy = user.Id
            };

            var rawDocument = new DocumentDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "RawDocumentName",
                WorkspaceId = workspace.Id,
                WorkflowId = workflow.Id,
                Status = DocumentStatusEnum.Preannotated,
                BirthStep = new DocumentProcessStepDataModel
                {
                    StepId = preannotatedStepId,
                    Status = DocumentProcessStatusEnum.Succeed
                }
            };

            await context.Workflows.InsertOneAsync(workflow);
            await context.WorkspaceTypes.InsertOneAsync(workspaceType);
            await context.Users.InsertOneAsync(user);
            await context.Workspaces.InsertOneAsync(workspace);
            await context.Documents.InsertOneAsync(rawDocument);
            
            var expectedResultDocument = new DocumentUploadServiceModel
            {
                Name = RandomStringHelper.RandomString(10),
                ContentType = HttpContentTypes.ApplicationJson,
                Content = Encoding.UTF8.GetBytes("Annotation Result."),
                UploadedBy = ObjectId.GenerateNewId().ToString(),
                ParentDocumentId = rawDocument.Id.ToString()
            };

            syncWorkflowStepProcessServiceMock.Setup(mock => mock.ProcessAsync(workspace.Id.ToString(), It.IsAny<WorkflowNextStepServiceModel>(), expectedResultDocument.Content, expectedResultDocument.ContentType))
                        .Returns(Task.FromResult((expectedResultDocument.Content, expectedResultDocument.ContentType, -1L)));

            var service = new DocumentService(fileStorageProviderMock.Object, new WorkspaceService(context, null), context, new WorkflowStepService(context, null), syncWorkflowStepProcessServiceMock.Object);

            var actualDocumentId = await service.UploadDocumentAsync(workspace.Id.ToString(), expectedResultDocument);

            var actualDocument = await context.Documents.Find(d => d.Id == new ObjectId(actualDocumentId) && d.WorkspaceId == workspace.Id).FirstOrDefaultAsync();

            Assert.Equal(expectedResultDocument.Name, actualDocument.Name);
            Assert.Equal(expectedResultDocument.ContentType, actualDocument.ContentType);
            Assert.Equal(HashHelper.GetMD5Hash(expectedResultDocument.Content), actualDocument.ContentMd5);
            Assert.Equal(expectedResultDocument.UploadedBy, actualDocument.UploadedBy.ToString());
            Assert.Equal(rawDocument.Id, actualDocument.ParentId);
            Assert.Equal(DocumentStatusEnum.Uploaded, actualDocument.Status);
            Assert.Equal(rawDocument.ItemCount, actualDocument.ItemCount);
        }

        private void AssertDocument(
            DocumentDataModel actualDocument,
            DocumentUploadServiceModel rawDocument,
            WorkflowStepDataModel step, 
            (byte[] content, string contentType, int itemCount) expectedContent)
        {
            Assert.Equal(rawDocument.Name, actualDocument.Name);
            Assert.Equal(rawDocument.Tags, actualDocument.Tags);
            Assert.Equal(rawDocument.UploadedBy, actualDocument.UploadedBy.ToString());
            Assert.Equal(step.StepId, actualDocument.BirthStep.StepId);
            Assert.Equal(DocumentProcessStatusEnum.Succeed, actualDocument.BirthStep.Status);
            Assert.Equal(HashHelper.GetMD5Hash(expectedContent.content), actualDocument.ContentMd5);
            Assert.Equal(expectedContent.contentType, actualDocument.ContentType);
            Assert.Equal(expectedContent.itemCount, actualDocument.ItemCount);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task UploadWithNotExistingWorkflowStep()
        {
            var preparedData = await PrepareDatas(ObjectId.GenerateNewId().ToString(), DateTime.UtcNow);

            var document = await context.Documents.Find(d => true).FirstAsync();

            this.workspaceServiceMock.Setup(x => x.GetWorkspaceAsync(It.IsAny<string>())).Returns(Task.FromResult(new WorkspaceServiceModel
            {
                Type = new WorkspaceTypeServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkflowId = ObjectId.GenerateNewId().ToString()
                }
            }));

            this.workflowServiceMock.Setup(w => w.GetNextWorkflowStepAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new ErrorHelper.ErrorMessageException(MusicKGMessages.WorkflowStepGetFailedMessage));

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.UploadDocumentAsync(document.WorkspaceId.ToString(), new DocumentUploadServiceModel
            {
                Name = "TestDocument",
                Content = Encoding.Default.GetBytes(RandomStringHelper.RandomString(32)),
                ContentType = "text/plain",
                Tags = new List<string> { "TestTag" },
                UploadedBy = ObjectId.GenerateNewId().ToString(),
                ParentDocumentId = document.Id.ToString()
            }));
            Assert.Equal(MusicKGMessages.WorkflowStepGetFailedMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Update

        #region Positive

        [Fact]
        public async Task UpdateDocument()
        {
            const string expectedName = "UpdatedName";
            const string expectedTag = "UpdatedTag";
            const DocumentStatusEnum expectedStatus = DocumentStatusEnum.Preprocessed;

            DateTime minUploadedAt = DateTime.Parse("2018-11-1");
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatas(workspaceId, minUploadedAt);
            var documentBeforeUpdated = preparedData.Item2.First();

            var updateServiceModel = new DocumentUpdateServiceModel
            {
                Name = expectedName,
                IsNameAssigned = true,
                Tags = new List<string> { expectedTag },
                IsTagsAssigned = true,
                Status = expectedStatus,
                IsStatusAssigned = true
            };

            await this.serviceUnderTest.UpdateDocumentAsync(workspaceId, documentBeforeUpdated.Id.ToString(), updateServiceModel);

            var updatedDocument = await this.context.Documents.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Id == documentBeforeUpdated.Id).FirstOrDefaultAsync();

            Assert.NotNull(updatedDocument);
            Assert.Equal(expectedName, updatedDocument.Name);
            Assert.Equal(expectedTag, updatedDocument.Tags.First());
            Assert.Equal(expectedStatus, updatedDocument.Status);
        }

        [Fact]
        public async Task UpdateDocumentNameOnly()
        {
            const string expectedName = "UpdatedName";

            DateTime minUploadedAt = DateTime.Parse("2018-11-1");
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatas(workspaceId, minUploadedAt);
            var documentBeforeUpdated = preparedData.Item2.First();

            var updateServiceModel = new DocumentUpdateServiceModel
            {
                Name = expectedName,
                IsNameAssigned = true,
                IsTagsAssigned = false,
                IsStatusAssigned = false
            };

            await this.serviceUnderTest.UpdateDocumentAsync(workspaceId, documentBeforeUpdated.Id.ToString(), updateServiceModel);

            var updatedDocument = await this.context.Documents.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Id == documentBeforeUpdated.Id).FirstOrDefaultAsync();

            Assert.NotNull(updatedDocument);
            Assert.Equal(expectedName, updatedDocument.Name);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task UpdateDocumentWithInvalidWorkspaceId()
        {
            DateTime minUploadedAt = DateTime.Parse("2018-11-1");
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatas(workspaceId, minUploadedAt);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.UpdateDocumentAsync(ObjectId.GenerateNewId().ToString(), preparedData.Item2.First().Id.ToString(), new DocumentUpdateServiceModel()));
            Assert.Equal(MusicKGMessages.DocumentNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateDocumentWithInvalidDocumentId()
        {
            DateTime minUploadedAt = DateTime.Parse("2018-11-1");
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatas(workspaceId, minUploadedAt);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.UpdateDocumentAsync(workspaceId, ObjectId.GenerateNewId().ToString(), new DocumentUpdateServiceModel()));
            Assert.Equal(MusicKGMessages.DocumentNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Delete

        #region Positive

        [Fact]
        public async Task DeleteDocument()
        {
            DateTime minUploadedAt = DateTime.Parse("2018-11-1");
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatas(workspaceId, minUploadedAt);

            var documentIdToBeDeleted = preparedData.Item2.First().Id.ToString();

            await this.serviceUnderTest.DeleteDocumentAsync(workspaceId, documentIdToBeDeleted);

            var actualDocuments = await this.context.Documents.Find(x => x.Id == new ObjectId(documentIdToBeDeleted)).ToListAsync();

            Assert.NotNull(actualDocuments);
            Assert.True(actualDocuments.First().IsDeleted);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task DeleteDocumentWithInvalidWorkspaceId()
        {
            DateTime minUploadedAt = DateTime.Parse("2018-11-1");
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatas(workspaceId, minUploadedAt);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.DeleteDocumentAsync(ObjectId.GenerateNewId().ToString(), preparedData.Item2.First().Id.ToString()));
            Assert.Equal(MusicKGMessages.DocumentNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteDocumentWithInvalidDocumentId()
        {
            DateTime minUploadedAt = DateTime.Parse("2018-11-1");
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatas(workspaceId, minUploadedAt);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.DeleteDocumentAsync(workspaceId, ObjectId.GenerateNewId().ToString()));
            Assert.Equal(MusicKGMessages.DocumentNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        private async Task<Tuple<DateTime, List<DocumentDataModel>>> PrepareDatas(string workspaceId, DateTime minUploadedAt)
        {
            List<DocumentDataModel> result = new List<DocumentDataModel>();
            result.Add(this.CreateDocumentDataModel(workspaceId, "TestDocumentName1", new List<string> { "TestTag1" }, DocumentStatusEnum.Uploaded, minUploadedAt, ObjectId.GenerateNewId().ToString()));
            result.Add(this.CreateDocumentDataModel(workspaceId, "TestDocumentName2", new List<string> { "TestTag2" }, DocumentStatusEnum.Preprocessed, minUploadedAt.AddDays(1), ObjectId.GenerateNewId().ToString()));
            result.Add(this.CreateDocumentDataModel(workspaceId, "TestDocumentName3", new List<string> { "TestTag3" }, DocumentStatusEnum.Preannotated, minUploadedAt.AddDays(2), ObjectId.GenerateNewId().ToString()));
            result.Add(this.CreateDocumentDataModel(workspaceId, "TestDocumentName4", new List<string> { "TestTag4" }, DocumentStatusEnum.Preannotated, minUploadedAt.AddDays(3), ObjectId.GenerateNewId().ToString()));
            result.Add(this.CreateDocumentDataModel(workspaceId, "TestDocumentName5", new List<string> { "TestTag5" }, DocumentStatusEnum.Assigned, minUploadedAt.AddDays(4), ObjectId.GenerateNewId().ToString()));

            await this.context.Documents.InsertManyAsync(result);

            return new Tuple<DateTime, List<DocumentDataModel>>(minUploadedAt.AddDays(5), result);
        }

        private DocumentDataModel CreateDocumentDataModel(string workspaceId, string name, List<string> tags,
            DocumentStatusEnum status, DateTime uploadedAt, string uploadedBy)
        {
            return new DocumentDataModel
            {
                Name = name,
                Tags = tags,
                WorkspaceId = new ObjectId(workspaceId),
                Status = status,
                UploadedAt = uploadedAt,
                UploadedBy = new ObjectId(uploadedBy)
            };
        }

        private void AssertServiceModel(DocumentDataModel expected, DocumentServiceModel actual)
        {
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Status, actual.Status);
            Assert.Equal(expected.Tags, actual.Tags);
            Assert.Equal(expected.UploadedAt.ToString(), actual.UploadedAt.ToString());
            Assert.Equal(expected.UploadedBy.ToString(), actual.UploadedBy.Id);
        }

        public class ClassDataForGetDocuments : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null, null, new DocumentStatusEnum[] { }, null, null, 0, null };
                yield return new object[] { null, null, new DocumentStatusEnum[] { }, null, null, 0, 1 };
                yield return new object[] { null, null, new DocumentStatusEnum[] { }, null, null, 1, 1 };
                yield return new object[] { "", null, new DocumentStatusEnum[] { }, null, null, 0, null };
                yield return new object[] { "Document", null, new DocumentStatusEnum[] { }, null, null, 0, null };
                yield return new object[] { "document", null, new DocumentStatusEnum[] { }, null, null, 0, null };
                yield return new object[] { null, "", new DocumentStatusEnum[] { }, null, null, 0, null };
                yield return new object[] { null, "Document Tag", new DocumentStatusEnum[] { }, null, null, 0, null };
                yield return new object[] { null, "document tag", new DocumentStatusEnum[] { }, null, null, 0, null };
                yield return new object[] { null, null, new DocumentStatusEnum[] { }, DateTime.UtcNow.AddDays(-1), null, 0, null };
                yield return new object[] { null, null, new DocumentStatusEnum[] { }, null, DateTime.UtcNow.AddDays(1), 0, null };
                yield return new object[] { null, null, new DocumentStatusEnum[] { }, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), 0, null };
                yield return new object[] { "document", "Document", new DocumentStatusEnum[] { DocumentStatusEnum.Uploaded }, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), 0, null };

                var statuses = Enum.GetNames(typeof(DocumentStatusEnum));

                for (int i = 0; i < Math.Pow(2, statuses.Length); i++)
                {
                    List<DocumentStatusEnum> subset = new List<DocumentStatusEnum>();
                    uint rs = 0;

                    while (rs < statuses.Length)
                    {
                        if ((i & (1u << (int)rs)) > 0)
                            subset.Add(Enum.Parse<DocumentStatusEnum>(statuses[(int)rs]));

                        rs++;
                    }

                    yield return new object[] { null, null, subset, null, null, 0, null };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
