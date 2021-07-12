using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Moq;
using MusicKG.DataAccess.Models;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MusicKG.Service.Resources;
using MusicKG.Service.Helpers;

namespace MusicKG.WebApi.Test.Controllers
{
    public class OntologyEntityControllerTest
    {
        private OntologyEntityController controllerUnderTest;
        private Mock<IOntologyService> ontologyEntityServiceMock;

        public OntologyEntityControllerTest()
        {
            this.ontologyEntityServiceMock = new Mock<IOntologyService>();
            this.controllerUnderTest = new OntologyEntityController(this.ontologyEntityServiceMock.Object);
        }

        [Fact]
        public async Task GetAllEntities()
        {
            var testWorkspaceId = ObjectId.GenerateNewId().ToString();

            IEnumerable<OntologyEntityListItemServiceModel> rowdatas = new List<OntologyEntityListItemServiceModel>()
            {
                new OntologyEntityListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyEntity1",
                    Color = "#111111"
                },
                new OntologyEntityListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyEntity2",
                    Color = "#222222"
                },
                new OntologyEntityListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyEntity3",
                    Color = "#333333"
                },
                new OntologyEntityListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyEntity4",
                    Color = "#444444"
                }
            };

            long count = rowdatas.ToList().Count;

            var serviceModelTuple = new Tuple<long, IEnumerable<OntologyEntityListItemServiceModel>>(count, rowdatas);

            ontologyEntityServiceMock.Setup(m => m.GetOntologyEntitiesAsync(testWorkspaceId, 0, null)).Returns(Task.FromResult(serviceModelTuple));

            var actualDatas = await controllerUnderTest.GetEntities(testWorkspaceId, 0, null);

            VerifyResult(serviceModelTuple.Item2.ToArray(), actualDatas.Items.ToArray());
        }

        [Fact]
        public async Task GetEntity()
        {
            var expectedEntity = new OntologyEntityServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "EntityName",
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Properties = new List<OntologyEntityPropertyServiceModel>(),
                Color = "#111111"
            };

            ontologyEntityServiceMock.Setup(x => x.GetOntologyEntityAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(expectedEntity));

            var actualEntity = await controllerUnderTest.GetEntity(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

            Assert.Equal(expectedEntity.Id, actualEntity.Id);
            Assert.Equal(expectedEntity.Name, actualEntity.Name);
            Assert.Equal(expectedEntity.Description, actualEntity.Description);
            Assert.Equal(expectedEntity.Color, actualEntity.Color);
        }

        [Fact]
        public async Task Create()
        {
            OntologyEntityCreateBindingModel bindingModel = new OntologyEntityCreateBindingModel
            {
                Name = "Entity1",
                Description = "Entity1 description",
                Color = "#111111",
            };

            OntologyEntityServiceModel serviceModel = new OntologyEntityServiceModel
            {
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Entity1",
                Properties = new List<OntologyEntityPropertyServiceModel>(),
                Description = "Entity1 description",
                Color = "#111111",
            };

            ontologyEntityServiceMock.Setup(m => m.CreateOntologyEntityAsync(It.IsAny<OntologyEntityCreateServiceModel>())).Returns(Task.FromResult(serviceModel));

            var result = await controllerUnderTest.CreateEntity(ObjectId.GenerateNewId().ToString(), bindingModel);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.NotEmpty(result.Id);
            Assert.Equal(bindingModel.Name, result.Name);
        }
        
        [Fact]
        public async Task Update()
        {
            var expectedEntity = new OntologyEntityServiceModel
            {
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "UpdateName1",
                Description = "UpdateDescription",
                Color = "#DDAAEE",
                Properties = new List<OntologyEntityPropertyServiceModel>(){
                    new OntologyEntityPropertyServiceModel()
                    {
                        Name = "Property1",
                        Description = "PropertyDescription1",
                        Type = ObjectId.GenerateNewId().ToString(),
                    },
                    new OntologyEntityPropertyServiceModel()
                    {
                        Name = "Property2",
                        Description = "PropertyDescription2",
                        Type = ObjectId.GenerateNewId().ToString(),
                    }
                },
            };

            this.ontologyEntityServiceMock.Setup(x => x.UpdateOntologyEntityAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyEntityUpdateServiceModel>())).Returns(Task.FromResult(
                    expectedEntity));

            var actualEntity = await this.controllerUnderTest.UpdateEntity(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), new OntologyEntityUpdateBindingModel() { Properties = new List<OntologyPropertyBindingModel>() });

            this.AssertViewModel(expectedEntity, actualEntity);
        }

        [Fact]
        public async Task UpdateEntityWithEmtpyName()
        {
            this.ontologyEntityServiceMock.Setup(x => x.UpdateOntologyEntityAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyEntityUpdateServiceModel>())).Returns(Task.FromResult(
                    new OntologyEntityServiceModel()));
            
            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateEntity(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), new OntologyEntityUpdateBindingModel() { Name = string.Empty }));

            this.ontologyEntityServiceMock.Verify(x => x.UpdateOntologyEntityAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyEntityUpdateServiceModel>()), Times.Never);

            Assert.Equal(MusicKGMessages.OntologyEntityNameEmptyMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateEntityWithDuplicateProperties()
        {
            var bindingModel = new OntologyEntityUpdateBindingModel
            {
                Properties = new List<OntologyPropertyBindingModel>
                {
                    new OntologyPropertyBindingModel
                    {
                        Name = "TestProperty",
                        Type = ObjectId.GenerateNewId().ToString(),
                        Description = "TestPropertyDescription"
                    },
                    new OntologyPropertyBindingModel
                    {
                        Name = "TestProperty",
                        Type = ObjectId.GenerateNewId().ToString(),
                        Description = "TestPropertyDescription1"
                    },
                }
            };

            this.ontologyEntityServiceMock.Setup(x => x.UpdateOntologyEntityAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyEntityUpdateServiceModel>())).Returns(Task.FromResult(
                    new OntologyEntityServiceModel()));

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateEntity(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), bindingModel));

            this.ontologyEntityServiceMock.Verify(x => x.UpdateOntologyEntityAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyEntityUpdateServiceModel>()), Times.Never);

            Assert.Equal(MusicKGMessages.OntologyEntityDuplicatePropertyNameMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateEntityWithInvalidPropertyName()
        {
            var bindingModel = new OntologyEntityUpdateBindingModel
            {
                Properties = new List<OntologyPropertyBindingModel>
                {
                    new OntologyPropertyBindingModel
                    {
                        Name = "Name123",
                    },
                    new OntologyPropertyBindingModel
                    {
                        Name = "Value_Test",
                    },
                    new OntologyPropertyBindingModel
                    {
                        Name = "Value.Test"
                    }
                }
            };

            this.ontologyEntityServiceMock.Setup(x => x.UpdateOntologyEntityAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyEntityUpdateServiceModel>())).Returns(Task.FromResult(
                    new OntologyEntityServiceModel()));

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateEntity(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), bindingModel));

            this.ontologyEntityServiceMock.Verify(x => x.UpdateOntologyEntityAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyEntityUpdateServiceModel>()), Times.Never);

            Assert.Equal(MusicKGMessages.OntologyEntityPropertyNameWrongMessage, exception.Message);
        }

        [Fact]
        public async Task Upload()
        {
            var file = new Mock<IFormFile>();
            var contentType = "text/plain";
            string workspaceId = ObjectId.GenerateNewId().ToString();

            OntologyUploadServiceModel serviceModel = new OntologyUploadServiceModel
            {
                Entities = new List<OntologyEntityServiceModel>()
                {
                    new OntologyEntityServiceModel()
                    {
                        Name = "EntityName1",
                        Description="entity description",
                        Properties = new List<OntologyEntityPropertyServiceModel>(),
                        Color = "#11ff11"
                    },
                    new OntologyEntityServiceModel()
                    {
                        Name = "EntityName2",
                        Description="entity description",
                        Properties = new List<OntologyEntityPropertyServiceModel>(),
                        Color = "#227711"
                    },
                },

                Relations = new List<OntologyRelationDownloadServiceModel>()
                {
                    new OntologyRelationDownloadServiceModel
                    {
                        Name = "Relation1",
                        Properties = new List<OntologyRelationPropertyServiceModel>(),
                        Description = "Relation1 description",
                        FirstEntityName = "uploadEntity1",
                        SecondEntityName = "uploadEntity2",
                    }
                }
            };

            using (var stream = new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(serviceModel))))
            {
                stream.Position = 0;
                file.Setup(f => f.OpenReadStream()).Returns(stream);
                file.Setup(x => x.ContentType).Returns(contentType);

                var returnedServiceModel = new OntologyUploadServiceModel()
                {
                    Entities = serviceModel.Entities,
                    Relations = serviceModel.Relations,
                };

                returnedServiceModel.Entities?.ForEach(item => { item.Id = ObjectId.GenerateNewId().ToString(); });

                returnedServiceModel.Relations?.ForEach(item =>
                {
                    item.Id = ObjectId.GenerateNewId().ToString();
                    item.FirstEntityId = ObjectId.GenerateNewId().ToString();
                    item.SecondEntityId = ObjectId.GenerateNewId().ToString();
                });

                ontologyEntityServiceMock.Setup(m => m.UploadOntologyAsync(workspaceId, It.IsAny<OntologyUploadServiceModel>()))
                    .Returns(Task.FromResult(returnedServiceModel));


                var viewModel = await this.controllerUnderTest.UploadOntology(workspaceId, file.Object);

                Assert.NotNull(viewModel);
                Assert.NotNull(viewModel.Entities);
                Assert.Equal(serviceModel.Entities.Count, viewModel.Entities.Count);
                Assert.False(viewModel.Entities.Exists(x => String.IsNullOrEmpty(x.Id)));

                Assert.NotNull(viewModel.Relations);
                Assert.Equal(serviceModel.Relations.Count, viewModel.Relations.Count);
                Assert.False(viewModel.Relations.Exists(x => String.IsNullOrEmpty(x.Id)));
            }
        }

        private void AssertViewModel(OntologyEntityServiceModel expected, OntologyEntityViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Color, actual.Color);
            AssertPropertyServiceModel(expected.Properties.ToArray(), actual.Properties.ToArray());
        }

        private void AssertViewModel(OntologyEntityDataModel expected, OntologyEntityListItemServiceModel actual)
        {
            Assert.Equal(expected.WorkspaceId.ToString(), actual.WorkspaceId);
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Color, actual.Color);
        }

        private void AssertPropertyServiceModel(OntologyEntityPropertyServiceModel[] expected, OntologyPropertyViewModel[] actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Type, actual[i].Type);
                Assert.Equal(expected[i].Description.ToString(), actual[i].Description);
            }
        }

        private void VerifyResult(OntologyEntityListItemServiceModel[] rawData, OntologyEntityListItemViewModel[] actualData)
        {
            Assert.NotNull(actualData);
            Assert.Equal(rawData.Length, actualData.Length);

            for (int i = 0; i < rawData.Count(); i++)
            {
                Assert.Equal(rawData[i].Id, actualData[i].Id);
                Assert.Equal(rawData[i].Name, actualData[i].Name);
            }
        }
    }
}
