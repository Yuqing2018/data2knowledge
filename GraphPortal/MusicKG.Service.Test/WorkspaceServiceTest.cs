using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Moq;
using System.Linq;
using MusicKG.Service.Test.Fixtures;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    public class WorkspaceServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private IMusicKGContext context;

        public WorkspaceServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
        }

        #region Get

        #region Positive

        [Theory]
        [InlineData(null, false, 0, null)]
        [InlineData(null, false, 0, 1)]
        [InlineData(null, false, 1, null)]
        [InlineData(null, false, 1, 1)]
        [InlineData(LanguageEnum.Chinese, false, 0, null)]
        [InlineData(LanguageEnum.English, false, 0, null)]
        [InlineData(null, true, 0, null)]
        [InlineData(LanguageEnum.Chinese, true, 0, null)]
        [InlineData(LanguageEnum.English, true, 0, null)]
        public async Task GetWorkspaces(LanguageEnum? language, bool IsCreatedBy, int from, int? size)
        {
            var (user, workspaceTypes, workspaces, tasks) = await PrepareWorkspaceData();

            var workspacesTotalExpected = workspaces.Where(w => (!language.HasValue || w.Language == language) && (!IsCreatedBy || w.CreatedBy == user.Id));
            var workspacesExpected = workspacesTotalExpected.Skip(from).Take(size ?? int.MaxValue);

            var service = new WorkspaceService(context, null);

            var (totalCount, workspacesReturned) = await service.GetWorkspacesAsync(language, IsCreatedBy ? user.Id.ToString() : null, from, size);

            Assert.NotNull(workspacesReturned);
            Assert.Equal(workspacesTotalExpected.Count(), totalCount);

            if (size.HasValue && size.Value < workspacesExpected.Count())
                Assert.Equal(workspacesReturned.Count(), size.Value);
            else
                Assert.Equal(workspacesReturned.Count(), workspacesExpected.Count());

            Assert.All(workspacesReturned, w =>
            {
                var workspaceExpected = workspacesExpected.Where(we => we.Id.ToString().Equals(w.Id)).First();
                Assert.Equal(workspaceExpected.Id.ToString(), w.Id);
                Assert.Equal(workspaceExpected.Name, w.Name);
                Assert.Equal(workspaceExpected.Description, w.Description);
                Assert.Equal(workspaceExpected.IsAutoMerging, w.IsAutoMerging);
                Assert.Equal(workspaceExpected.Language, w.Language);

                var workspaceTypeExpected = workspaceTypes.Where(te => te.Id.ToString().Equals(w.Type.Id)).First();
                Assert.Equal(workspaceTypeExpected.Name, w.Type.Name);
                Assert.Equal(workspaceTypeExpected.WorkflowId.ToString(), w.Type.WorkflowId);

                Assert.Equal(user.Id.ToString(), w.CreatedBy.Id);
                Assert.Equal(user.Name, w.CreatedBy.Name);
            });
        }

        [Fact]
        public async Task GetWorkspace()
        {
            var (user, readOnlyUser, workspaceTypes, workspaces, tasks) = await PrepareDataForGetWorkspacesList();
            var workspace = workspaces.First();

            var service = new WorkspaceService(context, null);

            var workspaceReturned = await service.GetWorkspaceAsync(workspace.Id.ToString());

            Assert.Equal(workspace.Id.ToString(), workspaceReturned.Id);
            Assert.Equal(workspace.Name, workspaceReturned.Name);
            Assert.Equal(workspace.IsAutoMerging, workspaceReturned.IsAutoMerging);
            Assert.Equal(workspace.Language, workspaceReturned.Language);
            Assert.Equal(workspace.Description, workspaceReturned.Description);

            var workspaceTypeExpected = workspaceTypes.Where(te => te.Id.ToString().Equals(workspaceReturned.Type.Id)).First();
            Assert.Equal(workspaceTypeExpected.Name, workspaceReturned.Type.Name);
            Assert.Equal(workspaceTypeExpected.WorkflowId.ToString(), workspaceReturned.Type.WorkflowId);

            Assert.Equal(user.Id.ToString(), workspaceReturned.CreatedBy.Id);
            Assert.Equal(user.Name, workspaceReturned.CreatedBy.Name);

            Assert.Equal(readOnlyUser.Id.ToString(), workspaceReturned.ReadOnlyUsers.First().Id);
            Assert.Equal(readOnlyUser.Name.ToString(), workspaceReturned.ReadOnlyUsers.First().Name);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task GetWorkspacesByUserId(bool isReadOnlyUser)
        {
            var (user, readOnlyUser, workspaceTypes, workspaces, tasks) = await PrepareDataForGetWorkspacesList();

            var workspacesExpected = workspaces.Where(w => isReadOnlyUser ?
                            w.ReadOnlyUserIds.Contains(readOnlyUser.Id) : w.CreatedBy == user.Id);

            var service = new WorkspaceService(context, null);

            var (totalCount, workspacesReturned) = await service.GetWorkspacesAsync(userId: isReadOnlyUser ? readOnlyUser.Id.ToString() : null);

            Assert.NotNull(workspacesReturned);
            Assert.Equal(workspacesExpected.Count(), totalCount);

            Assert.All(workspacesReturned, w =>
            {
                var workspaceExpected = workspacesExpected.Where(we => we.Id.ToString().Equals(w.Id)).First();
                Assert.Equal(workspaceExpected.Id.ToString(), w.Id);
                Assert.Equal(workspaceExpected.Name, w.Name);
                Assert.Equal(workspaceExpected.Description, w.Description);
                Assert.Equal(workspaceExpected.IsAutoMerging, w.IsAutoMerging);
                Assert.Equal(workspaceExpected.Language, w.Language);

                var workspaceTypeExpected = workspaceTypes.Where(te => te.Id.ToString().Equals(w.Type.Id)).First();
                Assert.Equal(workspaceTypeExpected.Name, w.Type.Name);
                Assert.Equal(workspaceTypeExpected.WorkflowId.ToString(), w.Type.WorkflowId);

                Assert.Equal(user.Id.ToString(), w.CreatedBy.Id);
                Assert.Equal(user.Name, w.CreatedBy.Name);
            });
        }

        #endregion

        #region Nagtive

        [Fact]
        public async Task GetWorkspaceWithInvalidId()
        {
            var (user, workspaceTypes, workspaces, tasks) = await PrepareWorkspaceData();
            var workspace = workspaces.First();

            var service = new WorkspaceService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.GetWorkspaceAsync(ObjectId.GenerateNewId().ToString()));
            Assert.Equal(MusicKGMessages.WorkspaceNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Create

        #region Positive

        [Fact]
        public async Task Create()
        {
            var (user, workspaceTypes, workspaces, tasks) = await PrepareWorkspaceData();
            var workspaceType = workspaceTypes.First();

            var createServiceModel = new WorkspaceCreateServiceModel
            {
                Name = "Workspace",
                Description = "Description",
                IsAutoMerging = true,
                Type = workspaceType.Id.ToString(),
                Language = LanguageEnum.Chinese,
                CreatedBy = user.Id.ToString(),
                ReadOnlyUsers = new List<string>()
                {
                    ObjectId.GenerateNewId(1).ToString(),
                    ObjectId.GenerateNewId(2).ToString()
                }
            };

            var service = new WorkspaceService(context, null);

            var workspaceId = await service.CreateWorkspaceAsync(createServiceModel);

            Assert.NotNull(workspaceId);

            var workspaceDataModel = await context.Workspaces.Find(u => u.Id == new ObjectId(workspaceId)).FirstOrDefaultAsync();

            Assert.NotNull(workspaceDataModel);
            Assert.Equal(createServiceModel.Name, workspaceDataModel.Name);
            Assert.Equal(createServiceModel.Description, workspaceDataModel.Description);
            Assert.Equal(createServiceModel.Type, workspaceDataModel.Type.ToString());
            Assert.Equal(createServiceModel.Language, workspaceDataModel.Language);
            Assert.Equal(new ObjectId(createServiceModel.CreatedBy), workspaceDataModel.CreatedBy);
            Assert.Equal(createServiceModel.IsAutoMerging, workspaceDataModel.IsAutoMerging);
            Assert.All(createServiceModel.ReadOnlyUsers, r =>
            {
                Assert.Contains(new ObjectId(r), workspaceDataModel.ReadOnlyUserIds);
            });
        }

        #endregion

        #region Nagtive

        [Fact]
        public async Task CreateWithExistingName()
        {
            var (user, workspaceTypes, workspaces, tasks) = await PrepareWorkspaceData();
            var workspace = workspaces.First();

            var createServiceModel = new WorkspaceCreateServiceModel
            {
                Name = workspace.Name,
                Type = workspace.Type.ToString(),
                CreatedBy = workspace.CreatedBy.ToString()
            };

            var service = new WorkspaceService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.CreateWorkspaceAsync(createServiceModel));
            Assert.Equal(MusicKGMessages.WorkspaceNameExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Update

        #region Positive

        [Fact]
        public async Task Update()
        {
            var (user, workspaceTypes, workspaces, tasks) = await PrepareWorkspaceData();
            var workspace = workspaces.First();

            var readOnlyUpdateUser = new UserDataModel()
            {
                Id = ObjectId.GenerateNewId(),
                Name = "ReadOnly",
                Roles = new List<UserRoleEnum> { UserRoleEnum.ReadOnly }
            };
            await context.Users.InsertOneAsync(readOnlyUpdateUser);

            var expectedModifiedData = new WorkspaceUpdateServiceModel
            {
                Name = $"{workspace.Name}Updated",
                IsNameAssigned = true,
                Description = $"{workspace.Description}Update",
                IsDescriptionAssigned = true,
                IsAutoMerging = !workspace.IsAutoMerging,
                ReadOnlyUserIds = new List<string>() { readOnlyUpdateUser.Id.ToString() }
            };

            var service = new WorkspaceService(context, null);

            await service.UpdateWorkspaceAsync(workspace.Id.ToString(), expectedModifiedData);

            var actualResult = await context.Workspaces.Find(u => u.Id == workspace.Id).FirstOrDefaultAsync();

            Assert.NotNull(actualResult);
            Assert.Equal(expectedModifiedData.Name, actualResult.Name);
            Assert.Equal(expectedModifiedData.Description, actualResult.Description);
            Assert.Equal(expectedModifiedData.IsAutoMerging, actualResult.IsAutoMerging);
            Assert.Equal(readOnlyUpdateUser.Id, actualResult.ReadOnlyUserIds.First());
        }

        #endregion

        #region Nagtive

        [Fact]
        public async Task UpdateWithInvalidId()
        {
            var updateServiceModel = new WorkspaceUpdateServiceModel();

            var service = new WorkspaceService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.UpdateWorkspaceAsync(ObjectId.GenerateNewId().ToString(), updateServiceModel));
            Assert.Equal(MusicKGMessages.WorkspaceNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateWorkspaceNameToExistingOne()
        {
            var (user, workspaceTypes, workspaces, tasks) = await PrepareWorkspaceData();
            var workspace = workspaces.First();

            var updateServiceModel = new WorkspaceUpdateServiceModel
            {
                Name = workspaces.Skip(1).First().Name,
                IsNameAssigned = true
            };

            var service = new WorkspaceService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.UpdateWorkspaceAsync(workspace.Id.ToString(), updateServiceModel));
            Assert.Equal(MusicKGMessages.WorkspaceNameExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Delete

        #region Positive

        [Theory]
        [InlineData(3)]
        [InlineData(0)]
        public async Task Delete(int tasksPerWorkspace)
        {
            var (user, workspaceTypes, workspaces, tasks) = await PrepareWorkspaceData(tasksPerWorkspace);
            var workspace = workspaces.First();

            var service = new WorkspaceService(context, null);

            await service.DeleteWorkspaceAsync(workspace.Id.ToString());

            var workspaceReturned = await context.Workspaces.Find(w => w.Id == new ObjectId(workspace.Id.ToString())).FirstAsync();
            Assert.NotNull(workspaceReturned);
            Assert.True(workspaceReturned.IsDeleted);
            Assert.NotEqual(workspace.Name, workspaceReturned.Name);

            var tasksReturned = await context.Tasks.Find(t => t.WorkspaceId == workspace.Id && t.IsDeleted).ToListAsync();
            Assert.NotNull(tasksReturned);
            Assert.Equal(tasks.Where(t => t.WorkspaceId == workspace.Id).Select(t => t.Id).OrderBy(tid => tid), tasksReturned.Select(t => t.Id).OrderBy(tid => tid));
        }

        #endregion

        #region Nagtive

        [Fact]
        public async Task DeleteWithInvalidId()
        {
            var service = new WorkspaceService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.DeleteWorkspaceAsync(ObjectId.GenerateNewId().ToString()));
            Assert.Equal(MusicKGMessages.WorkspaceNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        private async Task<(
            UserDataModel,
            IEnumerable<WorkspaceTypeDataModel>,
            IEnumerable<WorkspaceDataModel>,
            IEnumerable<TaskDataModel>)>
            PrepareWorkspaceData(int tasksPerWorkspace = 3)
        {
            var user = new UserDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "User",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Manager }
            };

            var workspaceTypes = Enumerable.Range(1, 3).Select(i => new WorkspaceTypeDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"WorkspaceType{i}",
                Status = WorkspaceTypeStatusEnum.Enabled,
                WorkflowId = ObjectId.GenerateNewId()
            }).ToList();

            var workspaces = workspaceTypes.SelectMany(
                (t, i) => Enum.GetValues(typeof(LanguageEnum)).Cast<LanguageEnum>().Select(
                    (l, j) => new WorkspaceDataModel
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = $"Workspace{i}-{j}",
                        Type = t.Id,
                        Language = l,
                        IsAutoMerging = true,
                        Description = $"Description{i}-{j}",
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Id
                    })).ToList();

            var tasks = workspaces.SelectMany(
                (w, i) => Enumerable.Range(1, tasksPerWorkspace).Select(
                    j => new TaskDataModel
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = $"Task {i}-{j}",
                        Status = TaskStatusEnum.ConflictResolved,
                        WorkspaceId = w.Id,
                        IsDeleted = false
                    })).ToList();

            await context.Users.InsertOneAsync(user);
            await context.WorkspaceTypes.InsertManyAsync(workspaceTypes);
            await context.Workspaces.InsertManyAsync(workspaces);
            if (tasksPerWorkspace > 0)
                await context.Tasks.InsertManyAsync(tasks);

            return (user, workspaceTypes, workspaces, tasks);
        }

        private async Task<(
            UserDataModel,
            UserDataModel,
            IEnumerable<WorkspaceTypeDataModel>,
            IEnumerable<WorkspaceDataModel>,
            IEnumerable<TaskDataModel>)>
            PrepareDataForGetWorkspacesList(int tasksPerWorkspace = 3)
        {
            var user = new UserDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "User",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Manager }
            };

            var readOnlyUser = new UserDataModel()
            {
                Id = ObjectId.GenerateNewId(),
                Name = "ReadOnly",
                Roles = new List<UserRoleEnum> { UserRoleEnum.ReadOnly }
            };

            var workspaceTypes = Enumerable.Range(1, 3).Select(i => new WorkspaceTypeDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"WorkspaceType{i}",
                Status = WorkspaceTypeStatusEnum.Enabled,
                WorkflowId = ObjectId.GenerateNewId()
            }).ToList();

            var workspaces = workspaceTypes.SelectMany(
                (t, i) => Enum.GetValues(typeof(LanguageEnum)).Cast<LanguageEnum>().Select(
                    (l, j) => new WorkspaceDataModel
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = $"Workspace{i}-{j}",
                        Type = t.Id,
                        Language = l,
                        IsAutoMerging = true,
                        Description = $"Description{i}-{j}",
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Id,
                        ReadOnlyUserIds = i / 2 == 0 ? new List<ObjectId>() { readOnlyUser.Id } : new List<ObjectId>()
                    })).ToList();

            var tasks = workspaces.SelectMany(
                (w, i) => Enumerable.Range(1, tasksPerWorkspace).Select(
                    j => new TaskDataModel
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = $"Task {i}-{j}",
                        Status = TaskStatusEnum.ConflictResolved,
                        WorkspaceId = w.Id,
                        IsDeleted = false
                    })).ToList();

            await context.Users.InsertOneAsync(user);
            await context.Users.InsertOneAsync(readOnlyUser);
            await context.WorkspaceTypes.InsertManyAsync(workspaceTypes);
            await context.Workspaces.InsertManyAsync(workspaces);
            if (tasksPerWorkspace > 0)
                await context.Tasks.InsertManyAsync(tasks);

            return (user, readOnlyUser, workspaceTypes, workspaces, tasks);
        }

        private void AssertWorkspaceServiceModel(WorkspaceDataModel expected, WorkspaceServiceModel actual)
        {
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.IsAutoMerging, actual.IsAutoMerging);
            Assert.Equal(expected.Language, actual.Language);
            Assert.Equal(expected.Type.ToString(), actual.Type.Id);
        }
    }
}
