using MongoDB.Bson;
using Moq;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Helpers;

namespace MusicKG.WebApi.Test.Controllers
{
    public class DocumentControllerTest
    {
        private readonly DocumentController controllerUnderTest;

        private readonly Mock<IDocumentService> documentServiceMock;

        public DocumentControllerTest()
        {
            this.documentServiceMock = new Mock<IDocumentService>();
            this.controllerUnderTest = new DocumentController(this.documentServiceMock.Object);
        }

        [Fact]
        public async Task GetDocuments()
        {
            var expectedDocuments = new DocumentServiceModel[]
            {
                new DocumentServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "DocumentName1",
                    ContentType = "text/plain",
                    Status = DocumentStatusEnum.Uploaded,
                    Tags = new List<string> { "DocumentTag1" },
                    WorkspaceId = ObjectId.GenerateNewId().ToString(),
                    UploadedAt = DateTime.Parse("2018-11-1"),
                    UploadedBy = new UserServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "UploadUser1"
                    }
                },
                new DocumentServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "DocumentName2",
                    ContentType = "text/plain",
                    Status = DocumentStatusEnum.Uploaded,
                    Tags = new List<string> { "DocumentTag2" },
                    WorkspaceId = ObjectId.GenerateNewId().ToString(),
                    UploadedAt = DateTime.Parse("2018-11-2"),
                    UploadedBy = new UserServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "UploadUser2"
                    }
                }
            };

            this.documentServiceMock.Setup(x => x.GetDocumentsAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                It.IsAny<List<DocumentStatusEnum>>(),
                It.IsAny<int>(), It.IsAny<int?>())).Returns(Task.FromResult(
                    new Tuple<long, IEnumerable<DocumentServiceModel>>(expectedDocuments.Length, expectedDocuments)));

            var controllerResult = await this.controllerUnderTest.GetDocuments(ObjectId.GenerateNewId().ToString(), 0, null, null, null, null, null, null);

            var actualDocuments = controllerResult.Items.ToArray();

            Assert.Equal(expectedDocuments.Length, actualDocuments.Length);
            for (int i = 0; i < expectedDocuments.Length; i++)
            {
                this.AssertViewModel(expectedDocuments[i], actualDocuments[i]);
            }
        }

        [Fact]
        public async Task GetDocument()
        {
            var expectedDocument = new DocumentServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "DocumentName",
                ContentType = "text/plain",
                Status = DocumentStatusEnum.Uploaded,
                Tags = new List<string> { "DocumentTag" },
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                UploadedAt = DateTime.Parse("2018-11-1"),
                UploadedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "UploadUser"
                }
            };

            this.documentServiceMock.Setup(x => x.GetDocumentAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(expectedDocument));

            var actualDocument = await this.controllerUnderTest.GetDocument(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

            this.AssertViewModel(expectedDocument, actualDocument);
        }

        [Fact]
        public async Task UploadDocument()
        {
            var file = new Mock<IFormFile>();
            var fileName = "TestFile.json";
            var contentType = "application/json";
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(RandomStringHelper.RandomString(100))))
            {
                stream.Position = 0;
                file.Setup(f => f.OpenReadStream()).Returns(stream);
                file.Setup(f => f.FileName).Returns(fileName);
                file.Setup(f => f.Length).Returns(stream.Length);
                file.Setup(x => x.ContentType).Returns(contentType);

                var expectedDocumentId = ObjectId.GenerateNewId().ToString();

                this.documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<string>(),
                    It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(expectedDocumentId));

                var actualDocument = await this.controllerUnderTest.UploadDocument(ObjectId.GenerateNewId().ToString(),
                    new List<string> { "TestTag" }, new List<long> { 6 }, new List<IFormFile> { file.Object });

                this.documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>()), Times.Once);

                Assert.Equal(expectedDocumentId, actualDocument.FirstOrDefault());
            }
        }

        [Fact]
        public async Task UploadDocumentWithEmptyName()
        {
            var file = new Mock<IFormFile>();
            var fileName = "";
            var contentType = "application/json";
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(RandomStringHelper.RandomString(100))))
            {
                stream.Position = 0;
                file.Setup(f => f.OpenReadStream()).Returns(stream);
                file.Setup(f => f.FileName).Returns(fileName);
                file.Setup(f => f.Length).Returns(stream.Length);
                file.Setup(x => x.ContentType).Returns(contentType);

                var expectedDocumentId = ObjectId.GenerateNewId().ToString();

                this.documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<string>(),
                    It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(expectedDocumentId));

                var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UploadDocument(ObjectId.GenerateNewId().ToString(),
                    new List<string> { "TestTag" }, new List<long> { 10 }, new List<IFormFile> { file.Object }));

                this.documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>()), Times.Never);

                Assert.Equal(Service.Resources.MusicKGMessages.DocumentNameEmptyMessage, exception.Message);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UploadDocumentWithDifferentCountOfItemsAndFiles(bool useItemCount)
        {
            var file = new Mock<IFormFile>();
            var fileName = "TestFile";
            var contentType = "application/json";
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(RandomStringHelper.RandomString(100))))
            {
                stream.Position = 0;
                file.Setup(f => f.OpenReadStream()).Returns(stream);
                file.Setup(f => f.FileName).Returns(fileName);
                file.Setup(f => f.Length).Returns(stream.Length);
                file.Setup(x => x.ContentType).Returns(contentType);

                var expectedDocumentId = ObjectId.GenerateNewId().ToString();

                this.documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<string>(),
                    It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(expectedDocumentId));

                if (useItemCount)
                {
                    var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UploadDocument(ObjectId.GenerateNewId().ToString(),
                        new List<string> { "TestTag" }, new List<long> { 10 }, new List<IFormFile> { file.Object, file.Object }));

                    Assert.Equal(Service.Resources.MusicKGMessages.DocumentItemCountWrongMessage, exception.Message);
                    this.documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>()), Times.Never);
                }
                else
                {
                    var result = await this.controllerUnderTest.UploadDocument(ObjectId.GenerateNewId().ToString(),
                        new List<string> { "TestTag" }, null, new List<IFormFile> { file.Object });

                    Assert.Single(result);
                    Assert.All(result, x =>
                    {
                        Assert.Equal(expectedDocumentId, x);
                    });

                    this.documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>()), Times.Once);
                }
            }
        }

        [Fact]
        public async Task UploadDocumentPartialSucceed()
        {
            var uploadDocuments = this.PrepareUploadedFiles();

            var expectedDocumentId = ObjectId.GenerateNewId().ToString();

            var expectedErrorMessage = "Test Error.";

            this.documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<string>(),
                    It.IsAny<DocumentUploadServiceModel>())).Throws(new Exception(expectedErrorMessage));

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UploadDocument(ObjectId.GenerateNewId().ToString(),
                    new List<string> { "TestTag" }, uploadDocuments.Select(x => x.Item3).ToList(), uploadDocuments.Select(x => x.Item2).ToList()));

            foreach (var stream in uploadDocuments.Select(x => x.Item1))
            {
                stream.Close();
                stream.Dispose();
            }

            this.documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>()), Times.Exactly(3));

            var message = new StringBuilder();
            message.AppendLine(string.Format(Service.Resources.MusicKGMessages.DocumentUploadFailedMessage, uploadDocuments.Count, uploadDocuments.Count));
            foreach (var failedDocument in uploadDocuments)
            {
                message.AppendLine(string.Format(Service.Resources.MusicKGMessages.DocumentUploadFailedDetailMessage, failedDocument.Item2.FileName, expectedErrorMessage));
            }

            Assert.Equal(message.ToString(), exception.Message);
        }

        private List<(MemoryStream, IFormFile, long)> PrepareUploadedFiles()
        {
            return Enumerable.Range(0, 3).Select(x =>
            {
                var file = new Mock<IFormFile>();
                var fileName = $"Upload Document {x}";
                var contentType = "application/json";
                var stream = new MemoryStream(Encoding.Default.GetBytes(RandomStringHelper.RandomString(100)));
                stream.Position = 0;
                file.Setup(f => f.OpenReadStream()).Returns(stream);
                file.Setup(f => f.FileName).Returns(fileName);
                file.Setup(f => f.Length).Returns(stream.Length);
                file.Setup(f => f.ContentType).Returns(contentType);
                return (stream, file.Object, Convert.ToInt64(x * 10));
            }).ToList();
        }

        [Fact]
        public async Task UpdateDocumentWithEmptyName()
        {
            this.documentServiceMock.Setup(x => x.UpdateDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DocumentUpdateServiceModel>())).Returns(Task.CompletedTask);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateDocument(ObjectId.GenerateNewId().ToString(),
                    ObjectId.GenerateNewId().ToString(), new DocumentUpdateBindingModel
                    {
                        Name = string.Empty
                    }));

            this.documentServiceMock.Verify(x => x.UpdateDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DocumentUpdateServiceModel>()), Times.Never);

            Assert.Equal(Service.Resources.MusicKGMessages.DocumentNameEmptyMessage, exception.Message);
        }

        private void AssertViewModel(DocumentServiceModel expected, DocumentViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Status, actual.Status);
            Assert.Equal(expected.Tags, actual.Tags);
            Assert.Equal(expected.UploadedAt, actual.UploadedAt);
            Assert.Equal(expected.UploadedBy.Id, actual.UploadedBy.Id);
            Assert.Equal(expected.UploadedBy.Name, actual.UploadedBy.Name);
            Assert.Equal(expected.WorkspaceId, actual.WorkspaceId);
        }
    }
}
