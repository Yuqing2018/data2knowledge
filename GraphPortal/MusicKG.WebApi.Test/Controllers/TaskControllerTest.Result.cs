using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.BindingModels;
using System.Collections.Generic;
using System.Linq;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.DataAccess.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MusicKG.Service.Resources;
using MusicKG.Service.Helpers;
using System.Text;

namespace MusicKG.WebApi.Test.Controllers
{
    public partial class TaskControllerTest
    {
        #region Save task document result

        [Fact]
        public async Task SaveResult()
        {
            var documentServiceMock = new Mock<IDocumentService>();
            documentServiceMock.Setup(d => d.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(ObjectId.GenerateNewId().ToString()));

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(t => t.SaveTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>())).Returns(Task.CompletedTask);

            var currentUserId = ObjectId.GenerateNewId().ToString();
            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, currentUserId),
                new Claim(ClaimTypes.Role, string.Join(',', new List<UserRoleEnum>{ UserRoleEnum.Manager }))
            });

            var controller = new TaskController(taskServiceMock.Object, documentServiceMock.Object, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            await controller.SaveResult(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), new TaskResultCreateBindingModel
            {
                Result = "test result",
                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                Status = TaskDocumentStatusEnum.Annotated
            });

            taskServiceMock.Verify(t => t.SaveTaskDocumentResultAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>()), Times.Once);
        }

        [Fact]
        public async Task SaveResultWithMissingUserId()
        {
            var documentServiceMock = new Mock<IDocumentService>();
            documentServiceMock.Setup(d => d.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(ObjectId.GenerateNewId().ToString()));

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(t => t.SaveTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>())).Returns(Task.CompletedTask);

            var controller = new TaskController(taskServiceMock.Object, documentServiceMock.Object, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = null
                }
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => controller.SaveResult(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), new TaskResultCreateBindingModel
            {
                Result = "test result",
                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                Status = TaskDocumentStatusEnum.Annotated
            }));
            Assert.Equal(MusicKGMessages.UserIdWrongMessage, exception.Message);

            taskServiceMock.Verify(t => t.SaveTaskDocumentResultAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>()), Times.Never);
        }

        [Fact]
        public async Task SaveResultWithInvalidUserId()
        {
            var documentServiceMock = new Mock<IDocumentService>();
            documentServiceMock.Setup(d => d.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(ObjectId.GenerateNewId().ToString()));

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(t => t.SaveTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>())).Returns(Task.CompletedTask);

            var currentUserId = "";
            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, currentUserId),
                new Claim(ClaimTypes.Role, string.Join(',', new List<UserRoleEnum>{ UserRoleEnum.Manager }))
            });

            var controller = new TaskController(taskServiceMock.Object, documentServiceMock.Object, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => controller.SaveResult(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), new TaskResultCreateBindingModel
            {
                Result = "test result",
                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                Status = TaskDocumentStatusEnum.Annotated
            }));
            Assert.Equal(MusicKGMessages.UserIdWrongMessage, exception.Message);

            taskServiceMock.Verify(t => t.SaveTaskDocumentResultAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>()), Times.Never);
        }

        #endregion

        #region Get task document result

        [Fact]
        public async Task GetResults()
        {
            var taskDocumentResult = new TaskDocumentResultServiceModel
            {
                DocumentId = ObjectId.GenerateNewId().ToString(),
                ResultDocumentId = ObjectId.GenerateNewId().ToString(),
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "testuser"
                },
                CreatedAt = DateTime.UtcNow,
                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                TaskDocumentStatus = TaskDocumentStatusEnum.Annotated
            };

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(t => t.GetTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultTypeEnum>())).Returns(Task.FromResult(
                    Enumerable.Range(1, 1).Select(i => taskDocumentResult)));

            var currentUserId = ObjectId.GenerateNewId().ToString();

            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, currentUserId),
                new Claim(ClaimTypes.Role, string.Join(',', new List<UserRoleEnum>{ UserRoleEnum.Manager }))
            });

            var controller = new TaskController(taskServiceMock.Object, null, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            var results = await controller.GetResults(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), TaskDocumentResultTypeEnum.ForModelTraining);

            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.NotNull(result);
            Assert.Equal(taskDocumentResult.DocumentId, result.DocumentId);
            Assert.Equal(taskDocumentResult.ResultDocumentId, result.ResultDocumentId);
            Assert.Equal(taskDocumentResult.ResultType, result.ResultType);
            Assert.Equal(taskDocumentResult.TaskDocumentStatus, result.TaskDocumentStatus);
            Assert.NotNull(taskDocumentResult.CreatedBy);
            Assert.Equal(taskDocumentResult.CreatedBy.Id, result.AnnotatedBy.Id);
            Assert.Equal(taskDocumentResult.CreatedBy.Name, result.AnnotatedBy.Name);
            Assert.Equal(taskDocumentResult.CreatedBy.Roles, result.AnnotatedBy.Roles);
            Assert.Equal(taskDocumentResult.CreatedAt.ToString(), result.AnnotatedAt.ToString());
        }

        [Fact]
        public async Task GetResultsWithManagerRole()
        {
            var taskDocumentResult = new TaskDocumentResultServiceModel
            {
                DocumentId = ObjectId.GenerateNewId().ToString(),
                ResultDocumentId = ObjectId.GenerateNewId().ToString(),
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "testuser"
                },
                CreatedAt = DateTime.UtcNow,
                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                TaskDocumentStatus = TaskDocumentStatusEnum.Annotated
            };

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(t => t.GetTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultTypeEnum>())).Returns(Task.FromResult(
                    Enumerable.Range(1, 1).Select(i => taskDocumentResult)));

            var currentUserId = ObjectId.GenerateNewId().ToString();

            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, currentUserId),
                new Claim(ClaimTypes.Role, string.Join(',', new List<UserRoleEnum>{ UserRoleEnum.Manager }))
            });

            var controller = new TaskController(taskServiceMock.Object, null, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            var results = await controller.GetResults(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), TaskDocumentResultTypeEnum.ForModelTraining);

            taskServiceMock.Verify(t => t.GetTaskDocumentResultAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultTypeEnum>()), Times.Once);

            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.NotNull(result);
            Assert.Equal(taskDocumentResult.DocumentId, result.DocumentId);
            Assert.Equal(taskDocumentResult.ResultDocumentId, result.ResultDocumentId);
            Assert.Equal(taskDocumentResult.ResultType, result.ResultType);
            Assert.Equal(taskDocumentResult.TaskDocumentStatus, result.TaskDocumentStatus);
            Assert.NotNull(taskDocumentResult.CreatedBy);
            Assert.Equal(taskDocumentResult.CreatedBy.Id, result.AnnotatedBy.Id);
            Assert.Equal(taskDocumentResult.CreatedBy.Name, result.AnnotatedBy.Name);
            Assert.Equal(taskDocumentResult.CreatedBy.Roles, result.AnnotatedBy.Roles);
            Assert.Equal(taskDocumentResult.CreatedAt.ToString(), result.AnnotatedAt.ToString());
        }

        [Fact]
        public async Task GetResultsWithMissingUserId()
        {
            var taskDocumentResult = new TaskDocumentResultServiceModel
            {
                DocumentId = ObjectId.GenerateNewId().ToString(),
                ResultDocumentId = ObjectId.GenerateNewId().ToString(),
                CreatedBy = new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "testuser"
                },
                CreatedAt = DateTime.UtcNow,
                ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                TaskDocumentStatus = TaskDocumentStatusEnum.Annotated
            };

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(t => t.GetTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultTypeEnum>())).Returns(Task.FromResult(
                    Enumerable.Range(1, 1).Select(i => taskDocumentResult)));

            var currentUserId = ObjectId.GenerateNewId().ToString();

            var controller = new TaskController(taskServiceMock.Object, null, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(
                () => controller.GetResults(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), TaskDocumentResultTypeEnum.ForModelTraining));

            Assert.Equal(MusicKGMessages.UserIdWrongMessage, exception.Message);
        }

        #endregion

        #region ApproveAll task

        [Fact]
        public async Task ApproveAllTest()
        {
            var documentServiceMock = new Mock<IDocumentService>();
            documentServiceMock.Setup(d => d.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<DocumentUploadServiceModel>())).Returns(Task.FromResult(ObjectId.GenerateNewId().ToString()));
            documentServiceMock.Setup(d => d.GetDocumentContentAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(Encoding.Default.GetBytes(ObjectId.GenerateNewId().ToString())));

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(t => t.SaveTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>())).Returns(Task.CompletedTask);

            var task = new TaskServiceModel()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Annotators = new List<TaskAnnotatorServiceModel>() { new TaskAnnotatorServiceModel()
                {
                    TaskDocuments= new List<TaskDocumentServiceModel>(){
                     new TaskDocumentServiceModel()
                    {
                        Document = new DocumentServiceModel()
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        }
                    },
                     new TaskDocumentServiceModel()
                     {
                        Document = new DocumentServiceModel()
                        {
                            Id = ObjectId.GenerateNewId().ToString()
                        }
                     }
                    }
                } }
            };
            taskServiceMock.Setup(t => t.GetTaskAsync(
                It.IsAny<string>(), It.IsAny<string>(), null, true)).Returns(Task.FromResult(task));

            var taskDocuments = new List<TaskDocumentResultServiceModel>
            {
                new TaskDocumentResultServiceModel()
                {
                    ResultDocumentId = ObjectId.GenerateNewId().ToString(),
                    TaskDocumentStatus = TaskDocumentStatusEnum.Annotated
                }
            };
            taskServiceMock.Setup(t => t.GetTaskDocumentResultAsync(
                It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<string>(), null)).Returns(Task.FromResult(taskDocuments.AsEnumerable()));

            var currentUserId = ObjectId.GenerateNewId().ToString();
            var httpContextMock = new Mock<HttpContext>();
            var principalMock = new Mock<ClaimsPrincipal>();
            httpContextMock.SetupGet(c => c.User).Returns(principalMock.Object);
            principalMock.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, currentUserId),
                new Claim(ClaimTypes.Role, string.Join(',', new List<UserRoleEnum>{ UserRoleEnum.Manager }))
            });

            var controller = new TaskController(taskServiceMock.Object, documentServiceMock.Object, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            await controller.ApproveAll(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

            taskServiceMock.Verify(t => t.SaveTaskDocumentResultAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TaskDocumentResultSaveServiceModel>()), Times.Exactly(2));
        }

        #endregion
    }
}
