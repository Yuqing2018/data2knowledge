using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Enums;
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
    public class OntologyEntityServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly IOntologyService serviceUnderTest;
        private readonly Mock<IOntologyExportProvider> exporterMock;

        public OntologyEntityServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));

            this.exporterMock = new Mock<IOntologyExportProvider>();

            Func<OntologyDownloadFileTypeEnum, IOntologyExportProvider> exporterFactory = r => { return exporterMock.Object; };

            this.serviceUnderTest = new OntologyService(context, null, exporterFactory);
        }

        [Fact]
        public async Task Create()
        {
            OntologyEntityCreateServiceModel createServiceModel = new OntologyEntityCreateServiceModel
            {
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Name = "Entity1",
                Description = "Entity1 description",
                Color = "#111111",
            };

            var serviceModel = await serviceUnderTest.CreateOntologyEntityAsync(createServiceModel);

            Assert.NotNull(serviceModel);
            Assert.NotEmpty(serviceModel.Id.ToString());
            Assert.Equal(createServiceModel.Name, serviceModel.Name);
            Assert.NotEmpty(serviceModel.Color);
            Assert.Equal(createServiceModel.Name, serviceModel.Name);

            var dataModel = await context.OntologyEntities.Find(u => u.Id == new ObjectId(serviceModel.Id)).FirstOrDefaultAsync();

            Assert.NotNull(dataModel);
            Assert.Equal(serviceModel.Name, dataModel.Name);
        }

        [Fact]
        public async Task GetAllEntities()
        {
            ObjectId testWorkspaceId = ObjectId.GenerateNewId();

            List<OntologyEntityDataModel> existingEntities = await this.PrepareDatasAsync(testWorkspaceId.ToString());

            var (totalCount, actualData) = await serviceUnderTest.GetOntologyEntitiesAsync(testWorkspaceId.ToString(), 0, null);

            var actualResult = actualData.ToList();

            Assert.Equal(existingEntities.Count, totalCount);

            for (int i = 0; i < totalCount; i++)
            {
                AssertServiceModel(existingEntities[i], actualResult[i]);
            }
        }

        [Fact]
        public async Task GetEntity()
        {
            var testWorkspaceId = ObjectId.GenerateNewId().ToString();

            List<OntologyEntityDataModel> existingEntities = await PrepareDatasAsync(testWorkspaceId);

            var expectedEntity = existingEntities.First();

            var actualEntity = await serviceUnderTest.GetOntologyEntityAsync(testWorkspaceId, expectedEntity.Id.ToString());

            Assert.Equal(expectedEntity.Id.ToString(), actualEntity.Id);
            Assert.Equal(expectedEntity.Name, actualEntity.Name);
            Assert.Equal(expectedEntity.WorkspaceId.ToString(), actualEntity.WorkspaceId);
            Assert.Equal(expectedEntity.Description, actualEntity.Description);
            Assert.Equal(expectedEntity.Color, actualEntity.Color);
        }

        [Fact]
        public async Task UpdateEntity()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatasAsync(workspaceId);
            var entityBeforeUpdated = preparedData.First();

            var updateServiceModel = new OntologyEntityUpdateServiceModel
            {
                Name = "UpdateName",
                IsNameAssigned = true,
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
                IsPropertiesAssigned = true,
                Description = "UpdateDescription",
                IsDescriptionAssigned = true,
                Color = "FF00AA",
                IsColorAssigned = true,

            };

            var entityServiceModel = await serviceUnderTest.UpdateOntologyEntityAsync(workspaceId, entityBeforeUpdated.Id.ToString(), updateServiceModel);

            Assert.NotNull(entityServiceModel);
            Assert.Equal(updateServiceModel.Name, entityServiceModel.Name);
            Assert.Equal(updateServiceModel.Description, entityServiceModel.Description);
            Assert.Equal(updateServiceModel.Color, entityServiceModel.Color);

            var updatedEntity = await this.context.OntologyEntities.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Id == entityBeforeUpdated.Id).FirstOrDefaultAsync();

            Assert.NotNull(updatedEntity);
            Assert.Equal(updateServiceModel.Name, updatedEntity.Name);
            Assert.Equal(updateServiceModel.Description, updatedEntity.Description);

            AssertPropertyServiceModel(updateServiceModel.Properties.ToArray(), updatedEntity.Properties.ToArray());
        }

        [Fact]
        public async Task DeleteEntity()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatasAsync(workspaceId);

            var entityIdToBeDeleted = preparedData.First().Id.ToString();

            await serviceUnderTest.DeleteOntologyEntityAsync(workspaceId, entityIdToBeDeleted);

            var actualEntity = await this.context.OntologyEntities.Find(x => x.Id == new ObjectId(entityIdToBeDeleted)).FirstOrDefaultAsync();

            Assert.Null(actualEntity);
        }

        [Fact]
        public async Task UploadOntology()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();

            var prepareDatas = await PrepareUploadDatas(workspaceId);

            OntologyUploadServiceModel expectedUploadServiceModel = new OntologyUploadServiceModel
            {
                Entities = prepareDatas.Item1.Select(x => OntologyEntityDataModelToServiceModel(x)).ToList(),
                Relations = prepareDatas.Item2.Select(x => OntologyRelationDataModelToDownloadServiceModel(x)).ToList(),
            };

            var actualUploadServiceModel = await serviceUnderTest.UploadOntologyAsync(workspaceId, expectedUploadServiceModel);

            Assert.NotNull(actualUploadServiceModel);
            Assert.NotNull(actualUploadServiceModel.Entities);
            Assert.Equal(expectedUploadServiceModel.Entities.Count, actualUploadServiceModel.Entities.Count);
            Assert.False(actualUploadServiceModel.Entities.Exists(x => String.IsNullOrEmpty(x.Id)));

            Assert.NotNull(actualUploadServiceModel.Relations);
            Assert.Equal(expectedUploadServiceModel.Relations.Count, actualUploadServiceModel.Relations.Count);
            Assert.False(actualUploadServiceModel.Relations.Exists(x => String.IsNullOrEmpty(x.Id)));

            var uploadEntitiesCount = actualUploadServiceModel.Entities.Count;

            for (int i = 0; i < uploadEntitiesCount; i++)
            {
                var expectedEntity = expectedUploadServiceModel.Entities[i];

                var uploadedEntity = actualUploadServiceModel.Entities[i];

                var actualEntity = await context.OntologyEntities.Find(u => u.Id == new ObjectId(uploadedEntity.Id)).FirstOrDefaultAsync();

                Assert.Equal(expectedEntity.Name, uploadedEntity.Name);

                Assert.NotNull(actualEntity);
                Assert.Equal(expectedEntity.Name, actualEntity.Name);
                Assert.Equal(expectedEntity.Description, actualEntity.Description);
                Assert.Equal(expectedEntity.Color, actualEntity.Color);

                this.AssertPropertyServiceModel(expectedEntity.Properties.ToArray(), actualEntity.Properties.ToArray());
            }

            var uploadRelationsCount = actualUploadServiceModel.Relations.Count;

            for (int i = 0; i < uploadRelationsCount; i++)
            {
                var expectedRelation = expectedUploadServiceModel.Relations[i];

                var uploadedRelation = actualUploadServiceModel.Relations[i];

                var actualRelation = await context.OntologyRelations.Find(u => u.Id == new ObjectId(uploadedRelation.Id)).FirstOrDefaultAsync();

                var actualRelationFirstEntityName = (await context.OntologyEntities.Find(u => u.Id == actualRelation.FirstEntityId).FirstOrDefaultAsync()).Name;
                var actualRelationSecondEntityName = (await context.OntologyEntities.Find(u => u.Id == actualRelation.SecondEntityId).FirstOrDefaultAsync()).Name;

                Assert.Equal(expectedRelation.Name, uploadedRelation.Name);

                Assert.NotNull(actualRelation);
                Assert.Equal(expectedRelation.Name, actualRelation.Name);
                
                Assert.Equal(expectedRelation.FirstEntityName, actualRelationFirstEntityName);
                Assert.Equal(expectedRelation.SecondEntityName, actualRelationSecondEntityName);
                Assert.Equal(expectedRelation.Description, actualRelation.Description);

                this.AssertRelationPropertyServiceModel(expectedRelation.Properties.ToArray(), actualRelation.Properties.ToArray());
            }
        }

        #region ontology download

        [Fact]
        public async Task DownloadOntologyToJson()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();

            var prepareDatas = await PrepareUploadDatas(workspaceId);

            await this.context.OntologyEntities.InsertManyAsync(prepareDatas.Item1);

            await this.context.OntologyRelations.InsertManyAsync(prepareDatas.Item2);

            var downloadByteArray = await serviceUnderTest.DownloadOntologyAsync(workspaceId, OntologyDownloadFileTypeEnum.Json);

            Assert.NotNull(downloadByteArray);
        }

        [Fact]
        public async Task DownloadOntologyToSchema()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();

            var prepareDatas = await PrepareUploadDatas(workspaceId);

            await this.context.OntologyEntities.InsertManyAsync(prepareDatas.Item1);

            await this.context.OntologyRelations.InsertManyAsync(prepareDatas.Item2);

            var downloadByteArray = await serviceUnderTest.DownloadOntologyAsync(workspaceId, OntologyDownloadFileTypeEnum.HugeGraphSchema);

            Assert.NotNull(downloadByteArray);
        }
        #endregion

        #region Negative

        [Fact]
        public async Task UpdateEntityWithInvalidWorkspaceId()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatasAsync(workspaceId);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => serviceUnderTest.UpdateOntologyEntityAsync(ObjectId.GenerateNewId().ToString(), preparedData.First().Id.ToString(), new OntologyEntityUpdateServiceModel()));

            Assert.Equal(MusicKGMessages.OntologyEntityNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateEntityWithInvalidentityId()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await PrepareDatasAsync(workspaceId);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => serviceUnderTest.UpdateOntologyEntityAsync(workspaceId, ObjectId.GenerateNewId().ToString(), new OntologyEntityUpdateServiceModel()));

            Assert.Equal(MusicKGMessages.OntologyEntityNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateEntityWithExistName()
        {
            string workspaceId = ObjectId.GenerateNewId().ToString();
            var preparedData = await this.PrepareDatasAsync(workspaceId);

            var firstEntity = preparedData.First();

            var updateEntity = new OntologyEntityUpdateServiceModel()
            {
                Name = preparedData[1].Name,
                IsNameAssigned = true,
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => serviceUnderTest.UpdateOntologyEntityAsync(firstEntity.WorkspaceId.ToString(), firstEntity.Id.ToString(), updateEntity));

            Assert.Equal(MusicKGMessages.OntologyEntityNameExistMessage, exception.Message);
        }

        #endregion

        private void AssertServiceModel(OntologyEntityDataModel expected, OntologyEntityListItemServiceModel actual)
        {
            Assert.Equal(expected.WorkspaceId.ToString(), actual.WorkspaceId);
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Color, actual.Color);
        }

        private void AssertPropertyServiceModel(OntologyEntityPropertyServiceModel[] expected, OntologyEntityPropertyDataModel[] actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Type, actual[i].Type.ToString());
                Assert.Equal(expected[i].Description.ToString(), actual[i].Description);
            }
        }

        private void AssertRelationPropertyServiceModel(OntologyRelationPropertyServiceModel[] expected, OntologyRelationPropertyDataModel[] actual)
        {
            Assert.Equal(expected.Count(), actual.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Type, actual[i].Type.ToString());
                Assert.Equal(expected[i].Description.ToString(), actual[i].Description);
            }
        }

        private async Task<List<OntologyEntityDataModel>> PrepareDatasAsync(string workspaceId)
        {
            List<OntologyEntityDataModel> result = new List<OntologyEntityDataModel>
            {
                GenarateOntologyEntityDataModel(workspaceId, "Entity1", "description1", "#111111"),
                GenarateOntologyEntityDataModel(workspaceId, "Entity2", "description2", "#222222"),
                GenarateOntologyEntityDataModel(workspaceId, "Entity3", "description3", "#444444"),
                GenarateOntologyEntityDataModel(workspaceId, "Entity4", "description4", "#661100")
            };

            await this.context.OntologyEntities.InsertManyAsync(result);

            return result;
        }

        private async Task<Tuple<IEnumerable<OntologyEntityDataModel>, IEnumerable<OntologyRelationDataModel>>> PrepareUploadDatas(string workspaceId)
        {
            var existEntities = await PrepareDatasAsync(workspaceId);

            var firstEntityId = existEntities[0].Id;
            var secondEntityId = existEntities[1].Id;

            List<OntologyEntityDataModel> entities = new List<OntologyEntityDataModel>
            {
                GenarateOntologyEntityDataModel(workspaceId, "uploadEntity1", "description1", "#111111"),
                GenarateOntologyEntityDataModel(workspaceId, "uploadEntity2", "description2", "#222222"),
                GenarateOntologyEntityDataModel(workspaceId, "uploadEntity3", "description3", "#444444"),
                GenarateOntologyEntityDataModel(workspaceId, "uploadEntity4", "description4", "#661100")
            };

            List<OntologyRelationDataModel> relations = new List<OntologyRelationDataModel>
            {
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secondEntityId, "relation1", "description1"),
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secondEntityId, "relation2", "description2"),
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secondEntityId, "relation3", "description3"),
                GenarateOntologyRelationDataModel(workspaceId, firstEntityId, secondEntityId, "relation4", "description4")
            };

            return new Tuple<IEnumerable<OntologyEntityDataModel>, IEnumerable<OntologyRelationDataModel>>(entities, relations);
        }

        private OntologyEntityDataModel GenarateOntologyEntityDataModel(string workspaceId, string name, string description, string color)
        {
            return new OntologyEntityDataModel
            {
                Id = ObjectId.GenerateNewId(),
                WorkspaceId = new ObjectId(workspaceId),
                Name = name,
                Description = description,
                Properties = new List<OntologyEntityPropertyDataModel>(),
                Color = color,
            };
        }

        private OntologyRelationDataModel GenarateOntologyRelationDataModel(string workspaceId, ObjectId firstEntityId, ObjectId secondEntityId, string name, string description)
        {
            return new OntologyRelationDataModel
            {
                Id = ObjectId.GenerateNewId(),
                WorkspaceId = new ObjectId(workspaceId),
                Name = name,
                Properties = new List<OntologyRelationPropertyDataModel>(),
                FirstEntityId = firstEntityId,
                SecondEntityId = secondEntityId,
                Description = description,
            };
        }

        private OntologyEntityServiceModel OntologyEntityDataModelToServiceModel(OntologyEntityDataModel dataModel)
        {
            return new OntologyEntityServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                Properties = dataModel.Properties?.Select(x => new OntologyEntityPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type.ToString()
                }).ToList() ?? new List<OntologyEntityPropertyServiceModel>(),
                Description = dataModel.Description,
                Color = dataModel.Color
            };
        }

        private OntologyRelationDownloadServiceModel OntologyRelationDataModelToDownloadServiceModel(OntologyRelationDataModel dataModel)
        {
            return new OntologyRelationDownloadServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                FirstEntityName = this.context.OntologyEntities.Find(u => u.Id == dataModel.FirstEntityId).First().Name,
                SecondEntityName = this.context.OntologyEntities.Find(u => u.Id == dataModel.SecondEntityId).First().Name,
                Properties = dataModel.Properties?.Select(x => new OntologyRelationPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = this.context.EntityPropertyTypes.Find(u => u.Id == x.Type).First().Name,
                }).ToList() ?? new List<OntologyRelationPropertyServiceModel>(),
                Description = dataModel.Description,
            };
        }

    }
}
