using MongoDB.Bson;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using MongoDB.Driver;
using MusicKG.Workflow.Test.Fixtures;
using MusicKG.Workflow.Services;
using MusicKG.Workflow.Services.Implementations;
using MusicKG.Workflow.Models.ServiceModels;

namespace MusicKG.Workflow.Test.Services
{
    public class WorkflowServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        private readonly IWorkflowService serviceUnderTest;

        public WorkflowServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.serviceUnderTest = new WorkflowService(this.context);
        }

        #region Get

        #region Positive

        [Fact]
        public async Task GetWorkflows()
        {
            var expectedData = await this.PrepareDatas();

            var actualData = await this.serviceUnderTest.GetWorkflowsAsync(0, null);

            var actualDataArray = actualData.Item2.ToArray();

            Assert.NotNull(actualData);
            Assert.Equal(expectedData.Count, actualData.Item1);
            Assert.Equal(expectedData.Count, actualDataArray.Length);
            for (int i = 0; i < expectedData.Count; i++)
            {
                this.AssertServiceModel(expectedData[i], actualDataArray[i]);
            }
        }

        [Fact]
        public async Task GetWorkflow()
        {
            var expectedData = await this.PrepareDatas();

            var actualData = await this.serviceUnderTest.GetWorkflowAsync(expectedData.First().Id.ToString());

            Assert.NotNull(actualData);
            this.AssertServiceModel(expectedData.First(), actualData);
        }

        [Fact]
        public async Task GetWorkflowsPaginationWithEnoughItems()
        {
            int from = 1, size = 2;
            var expectedData = await this.PrepareDatas();

            var actualData = await this.serviceUnderTest.GetWorkflowsAsync(from, size);
            var actualDataArray = actualData.Item2.ToArray();

            Assert.NotNull(actualData);
            Assert.Equal(expectedData.Count, actualData.Item1);
            Assert.Equal(size, actualDataArray.Length);
            for (int i = 0; i < actualDataArray.Length; i++)
            {
                this.AssertServiceModel(expectedData[i + from], actualDataArray[i]);
            }
        }

        [Fact]
        public async Task GetWorkflowsPaginationWithoutEnoughItems()
        {
            var expectedData = await this.PrepareDatas();

            int from = 1, size = expectedData.Count;

            var actualData = await this.serviceUnderTest.GetWorkflowsAsync(from, size);
            var actualDataArray = actualData.Item2.ToArray();

            Assert.NotNull(actualData);
            Assert.Equal(expectedData.Count, actualData.Item1);
            Assert.Equal(size - from, actualDataArray.Length);
            for (int i = 0; i < actualDataArray.Length; i++)
            {
                this.AssertServiceModel(expectedData[i + from], actualDataArray[i]);
            }
        }

        #endregion

        #region Negative

        [Fact]
        public async Task GetWorkflowWithInvalidId()
        {
            await this.PrepareDatas();

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.GetWorkflowAsync(ObjectId.GenerateNewId().ToString()));
            Assert.Equal("Workflow does not exist.", exception.Message);
        }

        #endregion

        #endregion

        #region Create

        #region Positive

        [Fact]
        public async Task CreateWorkflow()
        {
            var serviceModel = new WorkflowServiceModel
            {
                Name = "Workflow Name",
                Steps = new WorkflowStepServiceModel[]
                {
                    new WorkflowStepServiceModel
                    {
                        Name = "Workflow Step1",
                        ProcessorAssembly = "Processor Assembly 1",
                        ProcessorClass = "Processor Class 1",
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed },
                                Steps = new List<ObjectId> { ObjectId.GenerateNewId() }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3,
                            },
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded }
                        }
                    },
                    new WorkflowStepServiceModel
                    {
                        Name = "Workflow Step2",
                        ProcessorAssembly = "Processor Assembly 2",
                        ProcessorClass = "Processor Class 2",
                        ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Failed },
                                Steps = new List<ObjectId> { ObjectId.GenerateNewId() }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3,
                            },
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Preprocessed }
                        }
                    }
                }
            };

            var workflow = await this.serviceUnderTest.CreateWorkflowAsync(serviceModel);

            Assert.NotNull(workflow);

            var dataModel = await this.context.Workflows.Find(x => x.Id == new ObjectId(workflow.Id)).FirstOrDefaultAsync();

            Assert.NotNull(dataModel);
            this.AssertDataModel(workflow, dataModel);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task CreateWorkflowWithExistingName()
        {
            var expectedData = await this.PrepareDatas();

            var serviceModel = new WorkflowServiceModel
            {
                Name = expectedData.First().Name,
                Steps = null
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.CreateWorkflowAsync(serviceModel));
            Assert.Equal("Workflow name already exist.", exception.Message);
        }

        #endregion

        #endregion

        private async Task<List<WorkflowDataModel>> PrepareDatas()
        {
            var dataModels = new List<WorkflowDataModel>();
            ObjectId documentUploadingId = ObjectId.GenerateNewId(), wordSegmentationId = ObjectId.GenerateNewId(),
                bootstrapAnnoatationId = ObjectId.GenerateNewId(), documentSplittingId;

            dataModels.Add(new WorkflowDataModel
            {
                Name = "Knowledge Extraction",
                Steps = new List<WorkflowStepDataModel>
                {
                    new WorkflowStepDataModel
                    {
                        StepId = documentUploadingId,
                        Name = "Document Uploading",
                        ProcessorAssembly = null,
                        ProcessorClass = null,
                        InputFilter = null,
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = wordSegmentationId,
                        Name = "Word Segmentation",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.WordSegmentationProcessor",
                        ResultDocumentStatus = DataAccess.Enums.DocumentStatusEnum.Preprocessed,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { documentUploadingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = bootstrapAnnoatationId,
                        Name = "Bootstrap Annotation",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.BootstrapAnnotationProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Preprocessed },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { wordSegmentationId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    }
                },
            });

            documentUploadingId = ObjectId.GenerateNewId();
            documentSplittingId = ObjectId.GenerateNewId();
            dataModels.Add(new WorkflowDataModel
            {
                Name = "Intent Recognization",
                Steps = new List<WorkflowStepDataModel>
                {
                    new WorkflowStepDataModel
                    {
                        StepId = documentUploadingId,
                        Name = "Document Uploading",
                        ProcessorAssembly = null,
                        ProcessorClass = null,
                        InputFilter = null,
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = documentSplittingId,
                        Name = "Document Splitting",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.DocumentSplittingProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { documentUploadingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    }
                }
            });

            documentUploadingId = ObjectId.GenerateNewId();
            documentSplittingId = ObjectId.GenerateNewId();
            dataModels.Add(new WorkflowDataModel
            {
                Name = "Word Segmentation",
                Steps = new List<WorkflowStepDataModel>
                {
                    new WorkflowStepDataModel
                    {
                        StepId = documentUploadingId,
                        Name = "Document Uploading",
                        ProcessorAssembly = null,
                        ProcessorClass = null,
                        InputFilter = null,
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = documentSplittingId,
                        Name = "Document Splitting",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.DocumentSplittingProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { documentUploadingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    }
                }
            });

            documentUploadingId = ObjectId.GenerateNewId();
            documentSplittingId = ObjectId.GenerateNewId();
            dataModels.Add(new WorkflowDataModel
            {
                Name = "Entity Recognization",
                Steps = new List<WorkflowStepDataModel>
                {
                    new WorkflowStepDataModel
                    {
                        StepId = documentUploadingId,
                        Name = "Document Uploading",
                        ProcessorAssembly = null,
                        ProcessorClass = null,
                        InputFilter = null,
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = documentSplittingId,
                        Name = "Document Splitting",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.DocumentSplittingProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { documentUploadingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    }
                }
            });

            documentUploadingId = ObjectId.GenerateNewId();
            documentSplittingId = ObjectId.GenerateNewId();
            dataModels.Add(new WorkflowDataModel
            {
                Name = "Similar Question Generation",
                Steps = new List<WorkflowStepDataModel>
                {
                    new WorkflowStepDataModel
                    {
                        StepId = documentUploadingId,
                        Name = "Document Uploading",
                        ProcessorAssembly = null,
                        ProcessorClass = null,
                        InputFilter = null,
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = documentSplittingId,
                        Name = "Document Splitting",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.DocumentSplittingProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Preannotated,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { documentUploadingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    }
                }
            });

            documentUploadingId = ObjectId.GenerateNewId();
            documentSplittingId = ObjectId.GenerateNewId();
            var dictionarySortingId = ObjectId.GenerateNewId();
            var dictiaonaryMergingId = ObjectId.GenerateNewId();
            dataModels.Add(new WorkflowDataModel
            {
                Name = "Dictionary Compiling",
                Steps = new List<WorkflowStepDataModel>
                {
                    new WorkflowStepDataModel
                    {
                        StepId = documentUploadingId,
                        Name = "Dictionary Documents Uploading",
                        ProcessorAssembly = null,
                        ProcessorClass = null,
                        InputFilter = null,
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = documentSplittingId,
                        Name = "Dictionary Document Splitting",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.DictionaryDocumentSplittingProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { documentUploadingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = dictionarySortingId,
                        Name = "Dictionary Document Sorting",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.DictionaryDocumentSortingProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { documentSplittingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    },
                    new WorkflowStepDataModel
                    {
                        StepId = dictiaonaryMergingId,
                        Name = "Dictionary Document Merging",
                        ProcessorAssembly = "MusicKG.Service.dll",
                        ProcessorClass = "MusicKG.Service.Workflow.Implementations.Processors.DictionaryDocumentMergingProcessor",
                        ResultDocumentStatus = DocumentStatusEnum.Uploaded,
                        InputFilter = new WorkflowStepInputFilterDataModel
                        {
                            Status = new List<DocumentStatusEnum> { DocumentStatusEnum.Uploaded },
                            BirthStep = new BirthStepFilterDataModel
                            {
                                Steps = new List<ObjectId> { dictionarySortingId },
                                Status = new List<DocumentProcessStatusEnum> { DocumentProcessStatusEnum.Succeed }
                            },
                            DeathStep = new DeathStepDataModel
                            {
                                MaxRetryTimes = 3
                            }
                        }
                    }
                }
            });

            await this.context.Workflows.InsertManyAsync(dataModels);
            return dataModels;
        }

        private void AssertServiceModel(WorkflowDataModel expected, WorkflowServiceModel actual)
        {
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Steps.Count, actual.Steps.Count());
            var actualSteps = actual.Steps.ToArray();
            for (int i = 0; i < expected.Steps.Count; i++)
            {
                var expectedStep = expected.Steps[i];
                var actualStep = actualSteps[i];
                Assert.Equal(expectedStep.StepId.ToString(), actualStep.Id);
                Assert.Equal(expectedStep.Name, actualStep.Name);
                Assert.Equal(expectedStep.ProcessorAssembly, actualStep.ProcessorAssembly);
                Assert.Equal(expectedStep.ProcessorClass, actualStep.ProcessorClass);
                Assert.Equal(expectedStep.ResultDocumentStatus, actualStep.ResultDocumentStatus);
                Assert.Equal(expectedStep.InputFilter.ToJson(), actualStep.InputFilter.ToJson());
            }
        }

        private void AssertDataModel(WorkflowServiceModel expected, WorkflowDataModel actual)
        {
            Assert.Equal(expected.Id, actual.Id.ToString());
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Steps.Count(), actual.Steps.Count);
            var expectedSteps = expected.Steps.ToArray();
            for (int i = 0; i < expectedSteps.Length; i++)
            {
                var expectedStep = expectedSteps[i];
                var actualStep = actual.Steps[i];
                Assert.Equal(expectedStep.Id, actualStep.StepId.ToString());
                Assert.Equal(expectedStep.Name, actualStep.Name);
                Assert.Equal(expectedStep.ProcessorAssembly, actualStep.ProcessorAssembly);
                Assert.Equal(expectedStep.ProcessorClass, actualStep.ProcessorClass);
                Assert.Equal(expectedStep.ResultDocumentStatus, actualStep.ResultDocumentStatus);
                Assert.Equal(expectedStep.InputFilter.ToJson(), actualStep.InputFilter.ToJson());
            }
        }

    }
}
