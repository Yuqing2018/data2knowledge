using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Helpers;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Services;
using MusicKG.Workflow.Services.Implementations.Processors;
using MusicKG.Workflow.Settings;
using MusicKG.Workflow.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq.Protected;
using System.Threading;
using MusicKG.Service;
using MusicKG.Service.Implementations;
using MusicKG.Service.Settings;
using MusicKG.Workflow.Services.Implementations;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MusicKG.Workflow.Test.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MusicKG.Workflow.Test.Services.Processors
{
    public class KEPreprocessProcessorTest
    {
        [Theory]
        [InlineData(ProcessorType.KEPreprocessProcessor)]
        [InlineData(ProcessorType.KEPreannotateProcessor)]
        [InlineData(ProcessorType.KEPreannotatedResultMergeProcessor)]
        public async Task Process(ProcessorType processorType)
        {
            string expectedDocumentId = ObjectId.GenerateNewId().ToString();
            
            Mock<IWorkflowDocumentService> workflowDocumentServiceMock = new Mock<IWorkflowDocumentService>();
            Mock<IHttpClientFactory> httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var processor = this.GetProcessor(processorType, workflowDocumentServiceMock.Object, 
                httpClientFactoryMock.Object, out string content, out string contentType);

            workflowDocumentServiceMock.Setup(x => x.GetDocumentContentAsync(It.IsAny<string>())).Returns(Task.FromResult(new DocumentContentServiceModel
            {
                WorkflowId = ObjectId.Empty.ToString(),
                WorkspaceId = ObjectId.Empty.ToString(),
                Content = Encoding.UTF8.GetBytes(content),
                ContentType = contentType
            }));
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(HttpClientHelper.GetMockClient());
            workflowDocumentServiceMock.Setup(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>())).Returns(Task.FromResult(It.IsAny<string>()));

            var result = await processor.ProcessAsync(expectedDocumentId, ObjectId.GenerateNewId().ToString(), DataAccess.Enums.DocumentStatusEnum.Preprocessed);

            workflowDocumentServiceMock.Verify(x => x.GetDocumentContentAsync(expectedDocumentId), Times.Once);
            httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once);
            workflowDocumentServiceMock.Verify(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(DocumentProcessStatusEnum.Succeed, result.Status);
            Assert.Equal(expectedDocumentId, result.DocumentId);
        }

        [Theory]
        [InlineData(ProcessorType.KEPreprocessProcessor)]
        [InlineData(ProcessorType.KEPreannotateProcessor)]
        [InlineData(ProcessorType.KEPreannotatedResultMergeProcessor)]
        public async Task ProcessFailedWhenGetDocumentContent(ProcessorType processorType)
        {
            string expectedDocumentId = ObjectId.GenerateNewId().ToString();

            string expectedErrorMessage = "Server Internal Error.";

            Mock<IWorkflowDocumentService> workflowDocumentServiceMock = new Mock<IWorkflowDocumentService>();
            Mock<IHttpClientFactory> httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var processor = this.GetProcessor(processorType, workflowDocumentServiceMock.Object,
                httpClientFactoryMock.Object, out string content, out string contentType);

            workflowDocumentServiceMock.Setup(x => x.GetDocumentContentAsync(It.IsAny<string>())).Throws(new Exception(expectedErrorMessage));
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(HttpClientHelper.GetMockClient());
            workflowDocumentServiceMock.Setup(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>())).Returns(Task.FromResult(It.IsAny<string>()));

            var result = await processor.ProcessAsync(expectedDocumentId, ObjectId.Empty.ToString(), DocumentStatusEnum.Preprocessed);

            workflowDocumentServiceMock.Verify(x => x.GetDocumentContentAsync(expectedDocumentId), Times.Once);
            httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Never);
            workflowDocumentServiceMock.Verify(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>()), Times.Never);

            Assert.NotNull(result);
            Assert.Equal(DocumentProcessStatusEnum.Failed, result.Status);
            Assert.Equal(expectedErrorMessage, result.Message);
        }

        [Theory]
        [InlineData(ProcessorType.KEPreprocessProcessor)]
        [InlineData(ProcessorType.KEPreannotateProcessor)]
        [InlineData(ProcessorType.KEPreannotatedResultMergeProcessor)]
        public async Task ProcessFailedWhenInvokeWebApi(ProcessorType processorType)
        {
            string expectedDocumentId = ObjectId.GenerateNewId().ToString();
            
            string expectedErrorMessage = "400";

            Mock<IWorkflowDocumentService> workflowDocumentServiceMock = new Mock<IWorkflowDocumentService>();
            Mock<IHttpClientFactory> httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var processor = this.GetProcessor(processorType, workflowDocumentServiceMock.Object,
                httpClientFactoryMock.Object, out string content, out string contentType);

            workflowDocumentServiceMock.Setup(x => x.GetDocumentContentAsync(It.IsAny<string>())).Returns(Task.FromResult(new DocumentContentServiceModel
            {
                WorkflowId = ObjectId.Empty.ToString(),
                WorkspaceId = ObjectId.Empty.ToString(),
                Content = Encoding.UTF8.GetBytes(content),
                ContentType = contentType
            }));
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(HttpClientHelper.GetMockClient(false));
            workflowDocumentServiceMock.Setup(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>())).Returns(Task.FromResult(It.IsAny<string>()));

            var result = await processor.ProcessAsync(expectedDocumentId, ObjectId.Empty.ToString(), DocumentStatusEnum.Preprocessed);

            workflowDocumentServiceMock.Verify(x => x.GetDocumentContentAsync(expectedDocumentId), Times.Once);
            httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once);
            workflowDocumentServiceMock.Verify(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>()), Times.Never);

            Assert.NotNull(result);
            Assert.Equal(DocumentProcessStatusEnum.Failed, result.Status);
            Assert.Contains(expectedErrorMessage, result.Message);
        }

        [Theory]
        [InlineData(ProcessorType.KEPreprocessProcessor)]
        [InlineData(ProcessorType.KEPreannotateProcessor)]
        [InlineData(ProcessorType.KEPreannotatedResultMergeProcessor)]
        public async Task ProcessFailedWhenCreateNewDocument(ProcessorType processorType)
        {
            string expectedDocumentId = ObjectId.GenerateNewId().ToString();
            
            string expectedErrorMessage = "Create document failed.";

            Mock<IWorkflowDocumentService> workflowDocumentServiceMock = new Mock<IWorkflowDocumentService>();
            Mock<IHttpClientFactory> httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var processor = this.GetProcessor(processorType, workflowDocumentServiceMock.Object,
                httpClientFactoryMock.Object, out string content, out string contentType);

            workflowDocumentServiceMock.Setup(x => x.GetDocumentContentAsync(It.IsAny<string>())).Returns(Task.FromResult(new DocumentContentServiceModel
            {
                WorkflowId = ObjectId.Empty.ToString(),
                WorkspaceId = ObjectId.Empty.ToString(),
                Content = Encoding.UTF8.GetBytes(content),
                ContentType = contentType
            }));
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(HttpClientHelper.GetMockClient());
            workflowDocumentServiceMock.Setup(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>())).Throws(new Exception(expectedErrorMessage));

            var result = await processor.ProcessAsync(expectedDocumentId, ObjectId.Empty.ToString(), DocumentStatusEnum.Preprocessed);

            workflowDocumentServiceMock.Verify(x => x.GetDocumentContentAsync(expectedDocumentId), Times.Once);
            httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once);
            workflowDocumentServiceMock.Verify(x => x.CreateDocumentAsync(It.IsAny<WorkflowDocumentCreateServiceModel>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(DocumentProcessStatusEnum.Failed, result.Status);
            Assert.Equal(expectedErrorMessage, result.Message);
        }

        public enum ProcessorType
        {
            KEPreprocessProcessor,
            KEPreannotateProcessor,
            KEPreannotatedResultMergeProcessor
        }

        private IProcessorProvider GetProcessor(ProcessorType processorType,
            IWorkflowDocumentService workflowDocumentService,
            IHttpClientFactory httpClientFactory, out string content, out string contentType)
        {
            IProcessorProvider processor;
            KEProcessorsSettings settings = new KEProcessorsSettings();
            settings.ProcessorSettings.Add(processorType.ToString(), new KEProcessorSettings
            {
                ProcessorName = processorType.ToString(),
                RequestUrl = "http://10.127.197.119:35002"
            });

            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            Mock<IConfigurationSection> configurationSectionKEProcessorsMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionPreprocessMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionPreannotateMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionMergeMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionPreprocessNameMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionPreannotateNameMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionMergeNameMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionPreprocessUrlMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionPreannotateUrlMock = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> configurationSectionMergeUrlMock = new Mock<IConfigurationSection>();

            serviceProviderMock.Setup(x => x.GetService(typeof(IWorkflowDocumentService))).Returns(workflowDocumentService);
            serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientFactory))).Returns(httpClientFactory);

            configurationMock.Setup(x => x.GetSection("KEProcessors")).Returns(configurationSectionKEProcessorsMock.Object);

            configurationSectionKEProcessorsMock.Setup(x => x.GetChildren()).Returns(new List<IConfigurationSection>
            {
                configurationSectionPreprocessMock.Object,
                configurationSectionPreannotateMock.Object,
                configurationSectionMergeMock.Object
            });

            configurationSectionPreprocessNameMock.Setup(x => x.Value).Returns("KEPreprocessProcessor");
            configurationSectionPreprocessUrlMock.Setup(x => x.Value).Returns("http://10.127.197.119:35002");
            configurationSectionPreprocessMock.Setup(x => x.GetSection("ProcessorName")).Returns(configurationSectionPreprocessNameMock.Object);
            configurationSectionPreprocessMock.Setup(x => x.GetSection("RequestUrl")).Returns(configurationSectionPreprocessUrlMock.Object);

            configurationSectionPreannotateNameMock.Setup(x => x.Value).Returns("KEPreannotateProcessor");
            configurationSectionPreannotateUrlMock.Setup(x => x.Value).Returns("http://10.127.197.119:35002");
            configurationSectionPreannotateMock.Setup(x => x.GetSection("ProcessorName")).Returns(configurationSectionPreannotateNameMock.Object);
            configurationSectionPreannotateMock.Setup(x => x.GetSection("RequestUrl")).Returns(configurationSectionPreannotateUrlMock.Object);

            configurationSectionMergeNameMock.Setup(x => x.Value).Returns("KEPreannotatedResultMergeProcessor");
            configurationSectionMergeUrlMock.Setup(x => x.Value).Returns("http://10.127.197.119:35002");
            configurationSectionMergeMock.Setup(x => x.GetSection("ProcessorName")).Returns(configurationSectionMergeNameMock.Object);
            configurationSectionMergeMock.Setup(x => x.GetSection("RequestUrl")).Returns(configurationSectionMergeUrlMock.Object);

            switch (processorType)
            {
                case ProcessorType.KEPreprocessProcessor:
                default:
                    processor = new KEPreprocessProcessor();
                    content = "Test document content.";
                    contentType = "text/plain";
                    break;
                case ProcessorType.KEPreannotateProcessor:
                    processor = new KEPreannotateProcessor();
                    content = "{'Name':'a', 'Value':b}";
                    contentType = "application/json";
                    break;
                case ProcessorType.KEPreannotatedResultMergeProcessor:
                    processor = new KEPreannotatedResultMergeProcessor();
                    content = "{'Name':'a', 'Value':b}";
                    contentType = "application/json";
                    break;
            }

            processor.Initialize(serviceProviderMock.Object, configurationMock.Object, null);

            return processor;
        }
    }
}
