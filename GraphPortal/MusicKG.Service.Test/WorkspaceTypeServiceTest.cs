using MongoDB.Bson;
using Moq;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    [Collection("MongoCollection")]
    public class WorkspaceTypeServiceTest
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        private readonly IWorkspaceTypeService serviceUnderTest;

        public WorkspaceTypeServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.serviceUnderTest = new WorkspaceTypeService(this.context);
        }

        #region Get

        #region Positive

        [Fact]
        public async Task GetWorkspaceTypes()
        {
            var rawData = await this.PrepareDatas();

            var (count, workspaceTypes) = await this.serviceUnderTest.GetWorkspaceTypesAsync(0, null, null);
            var workspaceTypeArray = workspaceTypes.ToArray();

            Assert.Equal(rawData.Count, count);
            Assert.Equal(rawData.Count, workspaceTypeArray.Length);

            for (int i = 0; i < workspaceTypeArray.Length; i++)
            {
                this.AssertServiceModel(rawData[i], workspaceTypeArray[i]);
            }
        }

        [Fact]
        public async Task GetWorkspaceTypesByStatus()
        {
            var rawData = await this.PrepareDatas();

            var (count, workspaceTypes) = await this.serviceUnderTest.GetWorkspaceTypesAsync(0, null, WorkspaceTypeStatusEnum.Enabled);
            var workspaceTypeArray = workspaceTypes.ToArray();

            rawData = rawData.Where(x => x.Status == WorkspaceTypeStatusEnum.Enabled).ToList();

            Assert.Equal(rawData.Count, count);
            Assert.Equal(rawData.Count, workspaceTypeArray.Length);

            for (int i = 0; i < workspaceTypeArray.Length; i++)
            {
                this.AssertServiceModel(rawData[i], workspaceTypeArray[i]);
            }
        }

        [Fact]
        public async Task GetWorkspaceTypesPaginationWithEnoughItems()
        {
            int from = 1, size = 3;

            var rawData = await this.PrepareDatas();

            var (count, workspaceTypes) = await this.serviceUnderTest.GetWorkspaceTypesAsync(from, size, null);
            var workspaceTypeArray = workspaceTypes.ToArray();

            Assert.Equal(rawData.Count, count);
            Assert.Equal(size, workspaceTypeArray.Length);

            for (int i = 0; i < workspaceTypeArray.Length; i++)
            {
                this.AssertServiceModel(rawData[i + from], workspaceTypeArray[i]);
            }
        }

        [Fact]
        public async Task GetWorkspaceTypesPaginationWithoutEnoughItems()
        {
            int from = 1, size = 12;

            var rawData = await this.PrepareDatas();

            var (count, workspaceTypes) = await this.serviceUnderTest.GetWorkspaceTypesAsync(from, size, null);
            var workspaceTypeArray = workspaceTypes.ToArray();

            Assert.Equal(rawData.Count, count);
            Assert.Equal(rawData.Count - from, workspaceTypeArray.Length);

            for (int i = 0; i < workspaceTypeArray.Length; i++)
            {
                this.AssertServiceModel(rawData[i + from], workspaceTypeArray[i]);
            }
        }

        [Fact]
        public async Task GetWorkspaceType()
        {
            var expectedData = await this.PrepareDatas();

            var actualData = await this.serviceUnderTest.GetWorkspaceTypeAsync(expectedData.First().Id.ToString());

            Assert.NotNull(actualData);
            this.AssertServiceModel(expectedData.First(), actualData);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task GetWorkspaceTypeWithInvalidId()
        {
            await this.PrepareDatas();

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.GetWorkspaceTypeAsync(ObjectId.GenerateNewId().ToString()));
            Assert.Equal(MusicKGMessages.WorkspaceTypeNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Create

        #region Positive

        [Fact]
        public async Task CreateWorkspaceType()
        {
            var createServiceModel = new WorkspaceTypeCreateServiceModel
            {
                Name = "WorkspaceType",
                Status = WorkspaceTypeStatusEnum.Enabled,
                WorkflowId = ObjectId.GenerateNewId().ToString()
            };

            var serviceModel = await this.serviceUnderTest.CreateWorkspaceTypeAsync(createServiceModel);

            Assert.NotNull(serviceModel);

            var dataModel = await this.context.WorkspaceTypes.Find(x => x.Id == new ObjectId(serviceModel.Id)).FirstOrDefaultAsync();

            Assert.NotNull(dataModel);
            Assert.Equal(createServiceModel.Name, dataModel.Name);
            Assert.Equal(createServiceModel.Status, dataModel.Status);
            Assert.Equal(createServiceModel.WorkflowId, dataModel.WorkflowId.ToString());
        }

        #endregion

        #region Negative

        [Fact]
        public async Task CreateWorkspaceTypeWithExistingName()
        {
            var rawData = await this.PrepareDatas();

            var createServiceModel = new WorkspaceTypeCreateServiceModel
            {
                Name = rawData.First().Name
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.CreateWorkspaceTypeAsync(createServiceModel));
            Assert.Equal(MusicKGMessages.WorkspaceTypeNameExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Update

        [Fact]
        public async Task UpdateWorksapceType()
        {
            var rawData = (await this.PrepareDatas()).First();

            var updateServiceModel = new WorkspaceTypeUpdateServiceModel
            {
                Name = "UpdatedWorkspaceTypeName",
                IsNameAssigned = false,
                Status = WorkspaceTypeStatusEnum.Disabled,
                IsStatusAssigned = true
            };

            var serviceModel = await this.serviceUnderTest.UpdateWorkspaceTypeAsync(rawData.Id.ToString(), updateServiceModel);

            Assert.NotNull(serviceModel);

            var dataModel = await this.context.WorkspaceTypes.Find(x => x.Id == rawData.Id).FirstOrDefaultAsync();

            Assert.NotNull(dataModel);

            Assert.Equal(updateServiceModel.Status, dataModel.Status);
            Assert.NotEqual(updateServiceModel.Name, dataModel.Name);
        }

        #endregion

        private void AssertServiceModel(WorkspaceTypeDataModel expected, WorkspaceTypeServiceModel actual)
        {
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.WorkflowId.ToString(), actual.WorkflowId);
            Assert.Null(actual.WorkflowName);
        }

        private async Task<List<WorkspaceTypeDataModel>> PrepareDatas()
        {
            List<WorkspaceTypeDataModel> result = new List<WorkspaceTypeDataModel>();
            result.Add(this.CreateDataModel("WorkflowType1", WorkspaceTypeStatusEnum.Enabled));
            result.Add(this.CreateDataModel("WorkflowType2", WorkspaceTypeStatusEnum.Enabled));
            result.Add(this.CreateDataModel("WorkflowType3", WorkspaceTypeStatusEnum.Disabled));
            result.Add(this.CreateDataModel("WorkflowType4", WorkspaceTypeStatusEnum.Enabled));
            result.Add(this.CreateDataModel("WorkflowType5", WorkspaceTypeStatusEnum.Enabled));
            result.Add(this.CreateDataModel("WorkflowType6", WorkspaceTypeStatusEnum.Disabled));
            result.Add(this.CreateDataModel("WorkflowType7", WorkspaceTypeStatusEnum.Enabled));
            result.Add(this.CreateDataModel("WorkflowType8", WorkspaceTypeStatusEnum.Disabled));
            result.Add(this.CreateDataModel("WorkflowType9", WorkspaceTypeStatusEnum.Enabled));
            result.Add(this.CreateDataModel("WorkflowType10", WorkspaceTypeStatusEnum.Enabled));

            await this.context.WorkspaceTypes.InsertManyAsync(result);

            return result;
        }

        private WorkspaceTypeDataModel CreateDataModel(string name, WorkspaceTypeStatusEnum status)
        {
            return new WorkspaceTypeDataModel
            {
                Name = name,
                Status = status,
                WorkflowId = ObjectId.GenerateNewId()
            };
        }
    }
}
