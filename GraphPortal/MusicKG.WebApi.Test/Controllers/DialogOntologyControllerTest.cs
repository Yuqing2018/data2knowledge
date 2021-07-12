using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using MusicKG.Service;
using MusicKG.Service.Models;
using MongoDB.Bson;
using MusicKG.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using MusicKG.Service.Helpers;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using static MusicKG.Service.Helpers.ErrorHelper;

namespace MusicKG.WebApi.Test.Controllers
{
    public class DialogOntologyControllerTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetDialogOntologyTest(bool ontogloyExists)
        {
            var serviceMock = new Mock<IDialogOntologyService>();
            var serviceReturn = ontogloyExists ? new DialogOntologyServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                DialogEntityDocumentId = ObjectId.GenerateNewId().ToString(),
                DialogIntentDocumentId = ObjectId.GenerateNewId().ToString()
            } : null;

            serviceMock.Setup(x => x.GetDialogOntologyAsync(It.IsAny<string>())).Returns(Task.FromResult(serviceReturn));

            var controller = new DialogOntologyController(serviceMock.Object, null);

            var actualOntology = await controller.GetDialogOntology(ObjectId.GenerateNewId().ToString());

            Assert.NotNull(actualOntology);
            if (ontogloyExists)
            {
                Assert.Equal(serviceReturn.DialogEntityDocumentId, actualOntology.EntityDocumentId);
                Assert.Equal(serviceReturn.DialogIntentDocumentId, actualOntology.IntentDocumentId);
            }
            else
            {
                Assert.Null(actualOntology.IntentDocumentId);
                Assert.Null(actualOntology.EntityDocumentId);
            }
        }

        [Theory]
        [InlineData(DialogOntologyExistStatus.AllExist, UploadOptions.All)]
        [InlineData(DialogOntologyExistStatus.AllExist, UploadOptions.Entity)]
        [InlineData(DialogOntologyExistStatus.AllExist, UploadOptions.Intent)]
        [InlineData(DialogOntologyExistStatus.EntityExist, UploadOptions.All)]
        [InlineData(DialogOntologyExistStatus.EntityExist, UploadOptions.Entity)]
        [InlineData(DialogOntologyExistStatus.EntityExist, UploadOptions.Intent)]
        [InlineData(DialogOntologyExistStatus.IntentExist, UploadOptions.All)]
        [InlineData(DialogOntologyExistStatus.IntentExist, UploadOptions.Entity)]
        [InlineData(DialogOntologyExistStatus.IntentExist, UploadOptions.Intent)]
        [InlineData(DialogOntologyExistStatus.NotExist, UploadOptions.All)]
        [InlineData(DialogOntologyExistStatus.NotExist, UploadOptions.Entity)]
        [InlineData(DialogOntologyExistStatus.NotExist, UploadOptions.Intent)]
        public async Task UploadOntologyTest(DialogOntologyExistStatus currentStatus, UploadOptions uploadOptions)
        {
            string entityDocumentId = ObjectId.GenerateNewId().ToString();
            string intentDocumentId = ObjectId.GenerateNewId().ToString();
            string entityFileContent = RandomStringHelper.RandomString(100);
            string intentFileContent = RandomStringHelper.RandomString(100);
            Mock<IDialogOntologyService> service = new Mock<IDialogOntologyService>();
            Mock<IDocumentService> documentService = new Mock<IDocumentService>();
            Mock<IFormFile> entityFile = uploadOptions == UploadOptions.All || uploadOptions == UploadOptions.Entity ? CreateMockFile(entityFileContent) : null;
            Mock<IFormFile> intentFile = uploadOptions == UploadOptions.All || uploadOptions == UploadOptions.Intent ? CreateMockFile(intentFileContent) : null;
            string workspaceId = ObjectId.GenerateNewId().ToString();

            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ObjectId.GenerateNewId().ToString()),
                new Claim(ClaimTypes.Role, string.Join(',', "Administrator"))
            });

            DialogOntologyServiceModel currentDialogOntology = null;

            switch (currentStatus)
            {
                case DialogOntologyExistStatus.AllExist:
                    currentDialogOntology = new DialogOntologyServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        DialogEntityDocumentId = ObjectId.GenerateNewId().ToString(),
                        DialogIntentDocumentId = ObjectId.GenerateNewId().ToString()
                    };
                    break;
                case DialogOntologyExistStatus.EntityExist:
                    currentDialogOntology = new DialogOntologyServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        DialogEntityDocumentId = ObjectId.GenerateNewId().ToString(),
                        DialogIntentDocumentId = null
                    };
                    break;
                case DialogOntologyExistStatus.IntentExist:
                    currentDialogOntology = new DialogOntologyServiceModel
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        DialogEntityDocumentId = null,
                        DialogIntentDocumentId = ObjectId.GenerateNewId().ToString()
                    };
                    break;
                default:
                    break;
            }

            service.Setup(s => s.GetDialogOntologyAsync(workspaceId)).Returns(Task.FromResult(currentDialogOntology));
            documentService.Setup(x => x.UploadDocumentAsync(workspaceId, It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(ObjectId.GenerateNewId().ToString()));

            var controller = new DialogOntologyController(service.Object, documentService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            if (currentStatus == DialogOntologyExistStatus.AllExist && uploadOptions == UploadOptions.All ||
                currentStatus == DialogOntologyExistStatus.AllExist && uploadOptions == UploadOptions.Entity ||
                currentStatus == DialogOntologyExistStatus.EntityExist && uploadOptions == UploadOptions.All ||
                currentStatus == DialogOntologyExistStatus.EntityExist && uploadOptions == UploadOptions.Entity)
            {
                var exception = await Assert.ThrowsAsync<ErrorMessageException>(() => controller.UploadOntology(workspaceId, entityFile?.Object, intentFile?.Object));
                Assert.Equal(Service.Resources.MusicKGMessages.DialogOntologyEntityDuplicateMessage, exception.Message);
            }
            else if (currentStatus == DialogOntologyExistStatus.AllExist && uploadOptions == UploadOptions.Intent ||
                currentStatus == DialogOntologyExistStatus.IntentExist && uploadOptions == UploadOptions.All ||
                currentStatus == DialogOntologyExistStatus.IntentExist && uploadOptions == UploadOptions.Intent)
            {
                var exception = await Assert.ThrowsAsync<ErrorMessageException>(() => controller.UploadOntology(workspaceId, entityFile?.Object, intentFile?.Object));
                Assert.Equal(Service.Resources.MusicKGMessages.DialogOntologyIntentDuplicateMessage, exception.Message);
            }
            else
            {
                await controller.UploadOntology(workspaceId, entityFile?.Object, intentFile?.Object);

                service.Verify(x => x.GetDialogOntologyAsync(workspaceId), Times.Once);

                service.Verify(x => x.UploadDialogOntologyAsync(workspaceId, It.IsAny<DialogOntologyCreateServiceModel>()), Times.Once);

                switch (uploadOptions)
                {
                    case UploadOptions.All:
                        documentService.Verify(x => x.UploadDocumentAsync(workspaceId, It.IsAny<DocumentUploadServiceModel>()), Times.Exactly(2));
                        break;
                    case UploadOptions.Entity:
                    case UploadOptions.Intent:
                        documentService.Verify(x => x.UploadDocumentAsync(workspaceId, It.IsAny<DocumentUploadServiceModel>()), Times.Once);
                        break;
                    default:
                        break;
                }
            }
        }

        private Mock<IFormFile> CreateMockFile(string content)
        {
            var file = new Mock<IFormFile>();
            var fileName = "TestFile.json";
            var contentType = "application/json";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            file.Setup(f => f.OpenReadStream()).Returns(stream);
            file.Setup(f => f.FileName).Returns(fileName);
            file.Setup(f => f.Length).Returns(stream.Length);
            file.Setup(x => x.ContentType).Returns(contentType);
            return file;
        }

        public enum UploadOptions
        {
            Entity,

            Intent,

            All
        }

        public enum DialogOntologyExistStatus
        {
            NotExist,

            EntityExist,

            IntentExist,

            AllExist
        }
    }
}
