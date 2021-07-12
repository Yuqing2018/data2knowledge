using MongoDB.Bson;
using Moq;
using MusicKG.Service;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.WebApi.Test.Controllers
{
    public class OntologyRelationControllerTest
    {
        private OntologyRelationController controllerUnderTest;
        private Mock<IOntologyService> ontologyRelationServiceMock;

        public OntologyRelationControllerTest()
        {
            this.ontologyRelationServiceMock = new Mock<IOntologyService>();
            this.controllerUnderTest = new OntologyRelationController(this.ontologyRelationServiceMock.Object);
        }

        [Fact]
        public async Task GetAllRelations()
        {
            var testWorkspaceId = ObjectId.GenerateNewId().ToString();

            IEnumerable<OntologyRelationListItemServiceModel> rowdatas = new List<OntologyRelationListItemServiceModel>()
            {
                new OntologyRelationListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyRelation1",
                    FirstEntityId = ObjectId.GenerateNewId().ToString(),
                    SecondEntityId = ObjectId.GenerateNewId().ToString(),
                    Description = "description"
                },
                new OntologyRelationListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyRelation2",
                    FirstEntityId = ObjectId.GenerateNewId().ToString(),
                    SecondEntityId = ObjectId.GenerateNewId().ToString(),
                    Description = "description"
                },
                new OntologyRelationListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyRelation3",
                    FirstEntityId = ObjectId.GenerateNewId().ToString(),
                    SecondEntityId = ObjectId.GenerateNewId().ToString(),
                    Description = "description"
                },
                new OntologyRelationListItemServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = testWorkspaceId,
                    Name = "OntologyRelation4",
                    FirstEntityId = ObjectId.GenerateNewId().ToString(),
                    SecondEntityId = ObjectId.GenerateNewId().ToString(),
                    Description = "description"
                }
            };

            long count = rowdatas.ToList().Count;

            var serviceModelTuple = new Tuple<long, IEnumerable<OntologyRelationListItemServiceModel>>(count, rowdatas);

            ontologyRelationServiceMock.Setup(m => m.GetOntologyRelationsAsync(testWorkspaceId, 0, null)).Returns(Task.FromResult(serviceModelTuple));

            var actualDatas = await controllerUnderTest.GetRelations(testWorkspaceId, 0, null);

            VerifyResult(serviceModelTuple.Item2.ToArray(), actualDatas.Items.ToArray());
        }

        [Fact]
        public async Task GetRelation()
        {
            var expectedRelation = new OntologyRelationServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "RelationName",
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Properties = new List<OntologyRelationPropertyServiceModel>(),
                FirstEntityId = ObjectId.GenerateNewId().ToString(),
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
                Description = "description"
            };

            ontologyRelationServiceMock.Setup(x => x.GetOntologyRelationAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(expectedRelation));

            var actualRelation = await controllerUnderTest.GetRelation(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

            Assert.Equal(expectedRelation.Id, actualRelation.Id);
            Assert.Equal(expectedRelation.Name, actualRelation.Name);
            Assert.Equal(expectedRelation.Description, actualRelation.Description);
        }

        [Fact]
        public async Task Create()
        {
            OntologyRelationCreateBindingModel bindingModel = new OntologyRelationCreateBindingModel
            {
                Name = "Relation1",
                Description = "Relation1 description",
                FirstEntityId = ObjectId.GenerateNewId().ToString(),
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
            };

            OntologyRelationServiceModel serviceModel = new OntologyRelationServiceModel
            {
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Relation1",
                Properties = new List<OntologyRelationPropertyServiceModel>(),
                Description = "Relation1 description",
                FirstEntityId = ObjectId.GenerateNewId().ToString(),
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
            };

            ontologyRelationServiceMock.Setup(m => m.CreateOntologyRelationAsync(It.IsAny<OntologyRelationCreateServiceModel>())).Returns(Task.FromResult(serviceModel));

            var result = await controllerUnderTest.CreateRelation(ObjectId.GenerateNewId().ToString(), bindingModel);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(bindingModel.Name, result.Name);
        }

        [Fact]
        public async Task Update()
        {
            var expectedRelation = new OntologyRelationServiceModel
            {
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "UpdateName1",
                Description = "UpdateDescription",
                FirstEntityId = ObjectId.GenerateNewId().ToString(),
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
                Properties = new List<OntologyRelationPropertyServiceModel>(){
                    new OntologyRelationPropertyServiceModel()
                    {
                        Name = "Property1",
                        Description = "PropertyDescription1",
                        Type = ObjectId.GenerateNewId().ToString(),
                    },
                    new OntologyRelationPropertyServiceModel()
                    {
                        Name = "Property2",
                        Description = "PropertyDescription2",
                        Type = ObjectId.GenerateNewId().ToString(),
                    }
                },
            };

            var bindingRelation = new OntologyRelationUpdateBindingModel
            {
                Name = "UpdateName1",
                Description = "UpdateDescription",
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
                Properties = new List<OntologyPropertyBindingModel>(),
            };

            this.ontologyRelationServiceMock.Setup(x => x.UpdateOntologyRelationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyRelationUpdateServiceModel>())).Returns(Task.FromResult(
                    expectedRelation));

            var actualRelation = await this.controllerUnderTest.UpdateRelation(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), bindingRelation);

            this.AssertViewModel(expectedRelation, actualRelation);
        }

        [Fact]
        public async Task UpdateRelationWithEmptyName()
        {
            var bindingRelation = new OntologyRelationUpdateBindingModel
            {
                Name = "",
                Description = "UpdateDescription",
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
                Properties = new List<OntologyPropertyBindingModel>(),
            };

            this.ontologyRelationServiceMock.Setup(x => x.UpdateOntologyRelationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyRelationUpdateServiceModel>())).Returns(Task.FromResult(
                    new OntologyRelationServiceModel()));

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateRelation(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), bindingRelation));

            this.ontologyRelationServiceMock.Verify(x => x.UpdateOntologyRelationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyRelationUpdateServiceModel>()), Times.Never);

            Assert.Equal(MusicKGMessages.OntologyRelationNameEmptyMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateRelationWithDuplicateProperties()
        {
            var bindingRelation = new OntologyRelationUpdateBindingModel
            {
                Properties = new List<OntologyPropertyBindingModel>
                {
                    new OntologyPropertyBindingModel
                    {
                        Name = "TestProperty"
                    },
                    new OntologyPropertyBindingModel
                    {
                        Name = "TestProperty"
                    }
                }
            };

            this.ontologyRelationServiceMock.Setup(x => x.UpdateOntologyRelationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyRelationUpdateServiceModel>())).Returns(Task.FromResult(
                    new OntologyRelationServiceModel()));

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateRelation(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), bindingRelation));

            this.ontologyRelationServiceMock.Verify(x => x.UpdateOntologyRelationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyRelationUpdateServiceModel>()), Times.Never);

            Assert.Equal(MusicKGMessages.OntologyRelationDuplicatePropertyNameMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateRelationWithInvalidPropertyName()
        {
            var bindingRelation = new OntologyRelationUpdateBindingModel
            {
                Properties = new List<OntologyPropertyBindingModel>
                {
                    new OntologyPropertyBindingModel
                    {
                        Name = "Name123"
                    },
                    new OntologyPropertyBindingModel
                    {
                        Name = "Value_Test"
                    },
                    new OntologyPropertyBindingModel
                    {
                        Name = "Value.Test"
                    }
                }
            };

            this.ontologyRelationServiceMock.Setup(x => x.UpdateOntologyRelationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyRelationUpdateServiceModel>())).Returns(Task.FromResult(
                    new OntologyRelationServiceModel()));

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.controllerUnderTest.UpdateRelation(ObjectId.GenerateNewId().ToString(),
                ObjectId.GenerateNewId().ToString(), bindingRelation));

            this.ontologyRelationServiceMock.Verify(x => x.UpdateOntologyRelationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<OntologyRelationUpdateServiceModel>()), Times.Never);

            Assert.Equal(MusicKGMessages.OntologyRelationPropertyNameWrongMessage, exception.Message);
        }

        private void AssertViewModel(OntologyRelationServiceModel expected, OntologyRelationViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.FirstEntityId, actual.FirstEntityId);
            Assert.Equal(expected.SecondEntityId, actual.SecondEntityId);
            AssertPropertyServiceModel(expected.Properties.ToArray(), actual.Properties.ToArray());
        }

        private void AssertPropertyServiceModel(OntologyRelationPropertyServiceModel[] expected, OntologyPropertyViewModel[] actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Type, actual[i].Type);
                Assert.Equal(expected[i].Description.ToString(), actual[i].Description);
            }

        }

        private void VerifyResult(OntologyRelationListItemServiceModel[] rawData, OntologyRelationListItemViewModel[] actualData)
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
