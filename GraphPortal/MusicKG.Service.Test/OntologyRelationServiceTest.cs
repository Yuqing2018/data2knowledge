using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Extensions;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.Service.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.Service.Test
{
    public class OntologyRelationServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly IOntologyService serviceUnderTest;

        public OntologyRelationServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.serviceUnderTest = new OntologyService(context, null, null);
        }

        [Fact]
        public async Task Create()
        {
            var testWorkspaceId = ObjectId.GenerateNewId().ToString();
            var existEntities = await PrepareOntologyEntitiesDatasAsync(testWorkspaceId);

            var firstEntityId = existEntities[0].Id.ToString();
            var secongEntityId = existEntities[1].Id.ToString();

            OntologyRelationCreateServiceModel createServiceModel = new OntologyRelationCreateServiceModel
            {
                WorkspaceId = testWorkspaceId,
                Name = "Relation1",
                FirstEntityId = firstEntityId.ToString(),
                SecondEntityId = secongEntityId.ToString(),
                Description = "Relation1 description",
            };

            var serviceModel = await serviceUnderTest.CreateOntologyRelationAsync(createServiceModel);

            Assert.NotNull(serviceModel);
            Assert.NotEmpty(serviceModel.Id.ToString());
            Assert.Equal(createServiceModel.Name, serviceModel.Name);
            Assert.Equal(createServiceModel.FirstEntityId, serviceModel.FirstEntityId);
            Assert.Equal(createServiceModel.SecondEntityId, serviceModel.SecondEntityId);

            var actualDataModel = await context.OntologyRelations.Find(u => u.Id == new ObjectId(serviceModel.Id)).FirstOrDefaultAsync();

            Assert.NotNull(actualDataModel);
            Assert.Equal(serviceModel.Name, actualDataModel.Name);
            Assert.Equal(serviceModel.Description, actualDataModel.Description);
            Assert.Equal(serviceModel.FirstEntityId, actualDataModel.FirstEntityId.ToString());
            Assert.Equal(serviceModel.SecondEntityId, actualDataModel.SecondEntityId.ToString());
        }

        [Fact]
        public async Task GetAllRelations()
        {
            var testWorkspaceId = ObjectId.GenerateNewId().ToString();

            List<OntologyRelationDataModel> existingRelations = await PrepareDatasAsync(testWorkspaceId);

            var (totalCount, actualData) = await serviceUnderTest.GetOntologyRelationsAsync(testWorkspaceId, 0, null);

            var actualResult = actualData.ToList();

            Assert.Equal(existingRelations.Count, totalCount);

            for (int i = 0; i < totalCount; i++)
            {
                AssertServiceModel(existingRelations[i], actualResult[i]);
            }
        }

        [Fact]
        public async Task GetRelation()
        {
            string testWorkspaceId = ObjectId.GenerateNewId().ToString();

            List<OntologyRelationDataModel> existingRelations = await PrepareDatasAsync(testWorkspaceId);

            var expectedRelation = existingRelations.First();

            var actualRelation = await serviceUnderTest.GetOntologyRelationAsync(testWorkspaceId, expectedRelation.Id.ToString());

            Assert.Equal(expectedRelation.Id.ToString(), actualRelation.Id);
            Assert.Equal(expectedRelation.Name, actualRelation.Name);
            Assert.Equal(expectedRelation.FirstEntityId.ToString(), actualRelation.FirstEntityId);
            Assert.Equal(expectedRelation.SecondEntityId.ToString(), actualRelation.SecondEntityId);
            Assert.Equal(expectedRelation.Description, actualRelation.Description);
        }

        [Fact]
        public async Task UpdateRelation()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();

            var existEntities = await PrepareOntologyEntitiesDatasAsync(workspaceId);

            var updateSecongEntityId = existEntities[0].Id.ToString();

            var preparedData = await this.PrepareDatasAsync(workspaceId);
            var relationBeforeUpdated = preparedData.First();

            var updateServiceModel = new OntologyRelationUpdateServiceModel
            {
                Name = "UpdateName",
                IsNameAssigned = true,
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
                IsPropertiesAssigned = true,
                Description = "UpdateDescription",
                IsDescriptionAssigned = true,
                SecondEntityId = updateSecongEntityId,
                IsSecondEntityAssigned = true
            };



            var relationServiceModel = await serviceUnderTest.UpdateOntologyRelationAsync(workspaceId, relationBeforeUpdated.Id.ToString(), updateServiceModel);

            Assert.NotNull(relationServiceModel);
            Assert.Equal(updateServiceModel.Name, relationServiceModel.Name);
            Assert.Equal(updateServiceModel.Description, relationServiceModel.Description);

            var updatedRelation = await this.context.OntologyRelations.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Id == relationBeforeUpdated.Id).FirstOrDefaultAsync();

            Assert.NotNull(updatedRelation);
            Assert.Equal(relationServiceModel.Name, updatedRelation.Name);
            Assert.Equal(relationServiceModel.Description, updatedRelation.Description);
            Assert.Equal(relationServiceModel.SecondEntityId, updatedRelation.SecondEntityId.ToString());

            AssertPropertyServiceModel(updateServiceModel.Properties.ToArray(), updatedRelation.Properties.ToArray());
        }

        [Fact]
        public async Task DeleteRelation()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatasAsync(workspaceId);

            var relationIdToBeDeleted = preparedData.First().Id.ToString();

            await serviceUnderTest.DeleteOntologyRelationAsync(workspaceId, relationIdToBeDeleted);

            var actualRelation = await this.context.OntologyRelations.Find(x => x.Id == new ObjectId(relationIdToBeDeleted)).FirstOrDefaultAsync();

            Assert.Null(actualRelation);
        }

        #region Negative

        [Fact]
        public async Task UpdateRelationWithInvalidWorkspaceId()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatasAsync(workspaceId);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => serviceUnderTest.UpdateOntologyRelationAsync(ObjectId.GenerateNewId().ToString(), preparedData.First().Id.ToString(), new OntologyRelationUpdateServiceModel()));

            Assert.Equal(MusicKGMessages.OntologyRelationNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateRelationWithInvalidRelationId()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await PrepareDatasAsync(workspaceId);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => serviceUnderTest.UpdateOntologyRelationAsync(workspaceId, ObjectId.GenerateNewId().ToString(), new OntologyRelationUpdateServiceModel()));

            Assert.Equal(MusicKGMessages.OntologyRelationNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateRelationWithExistName()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatasAsync(workspaceId);

            var firstRelation = preparedData.First();

            var updateRelation = new OntologyRelationUpdateServiceModel()
            {
                Name = preparedData[1].Name,
                IsNameAssigned = true,
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => serviceUnderTest.UpdateOntologyRelationAsync(firstRelation.WorkspaceId.ToString(), firstRelation.Id.ToString(), updateRelation));

            Assert.Equal(MusicKGMessages.OntologyRelationNameExistMessage, exception.Message);
        }

        #endregion

        #region Checker

        [Fact]
        public async Task OntologyRelationCheckCreateFirstEntityNotExist()
        {
            var serviceModel = new OntologyRelationCreateServiceModel
            {
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Name = "entityName1",
                FirstEntityId = ObjectId.GenerateNewId().ToString(),
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
            };

            var result = await serviceModel.Check(serviceModel.WorkspaceId, this.context);

            Assert.False(result.Item1);
            Assert.Equal(Service.Resources.MusicKGMessages.OntologyRelationFirstEntityNotExistMessage, result.Item2);
        }
        
        [Fact]
        public async Task OntologyRelationCheckUpdateIvalidSecondEntityId()
        {
            var dataModel = new OntologyRelationDataModel
            {
                SecondEntityId = ObjectId.GenerateNewId()
            };

            await this.context.OntologyRelations.InsertOneAsync(dataModel);

            var serviceModel = new OntologyRelationUpdateServiceModel
            {
                IsSecondEntityAssigned = true,
                SecondEntityId = ObjectId.GenerateNewId().ToString(),
            };

            var result = await serviceModel.Check(dataModel.WorkspaceId.ToString(), dataModel.Id.ToString(), this.context);

            Assert.False(result.Item1);
            Assert.Equal(Service.Resources.MusicKGMessages.OntologyRelationSecondEntityNotExistMessage, result.Item2);
        }
        #endregion

        private void AssertServiceModel(OntologyRelationDataModel expected, OntologyRelationListItemServiceModel actual)
        {
            Assert.Equal(expected.WorkspaceId.ToString(), actual.WorkspaceId);
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.FirstEntityId.ToString(), actual.FirstEntityId);
            Assert.Equal(expected.SecondEntityId.ToString(), actual.SecondEntityId);
            Assert.Equal(expected.Description, actual.Description);
        }

        private void AssertPropertyServiceModel(OntologyRelationPropertyServiceModel[] expected, OntologyRelationPropertyDataModel[] actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Type, actual[i].Type.ToString());
                Assert.Equal(expected[i].Description.ToString(), actual[i].Description);
            }
        }

        private async Task<List<OntologyEntityDataModel>> PrepareOntologyEntitiesDatasAsync(string workspaceId)
        {
            List<OntologyEntityDataModel> result = new List<OntologyEntityDataModel>()
            {
                new OntologyEntityDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    WorkspaceId = new ObjectId(workspaceId),
                    Name = RandomStringHelper.RandomString(10),
                    Description = "Exist entity 1 description",
                    Properties = new List<OntologyEntityPropertyDataModel>(),
                    Color = "#dd1111",
                },
                new OntologyEntityDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    WorkspaceId = new ObjectId(workspaceId),
                    Name = RandomStringHelper.RandomString(10),
                    Description = "Exist entity 2 description",
                    Properties = new List<OntologyEntityPropertyDataModel>(),
                    Color = "#223366",
                }
            };

            await this.context.OntologyEntities.InsertManyAsync(result);

            return result;
        }

        private async Task<List<OntologyRelationDataModel>> PrepareDatasAsync(string workspaceId)
        {
            var existEntities = await PrepareOntologyEntitiesDatasAsync(workspaceId);

            var firstEntityId = existEntities[0].Id.ToString();
            var secongEntityId = existEntities[1].Id.ToString();

            List<OntologyRelationDataModel> result = new List<OntologyRelationDataModel>
            {
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secongEntityId, "relation1", "description1"),
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secongEntityId, "relation2", "description2"),
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secongEntityId, "relation3", "description3"),
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secongEntityId, "relation4", "description4")
            };

            await this.context.OntologyRelations.InsertManyAsync(result);

            return result;
        }

        private OntologyRelationDataModel GenarateOntologyRelationDataModel(string workspaceId, string firstEntityId, string secongEntityId, string name, string description)
        {
            return new OntologyRelationDataModel
            {
                Id = ObjectId.GenerateNewId(),
                WorkspaceId = new ObjectId(workspaceId),
                Name = name,
                Properties = new List<OntologyRelationPropertyDataModel>(),
                FirstEntityId = new ObjectId(firstEntityId),
                SecondEntityId = new ObjectId(secongEntityId),
                Description = description,
            };
        }
    }
}
