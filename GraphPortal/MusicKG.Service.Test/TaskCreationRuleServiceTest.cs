using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Fixtures;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    [Collection("MongoCollection")]
    public class TaskCreationRuleServiceTest
    {
        private readonly MongoFixture mongoFixture;
        private readonly ITaskCreationRuleService serviceUnderTest;
        private readonly IMusicKGContext context;

        public TaskCreationRuleServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.serviceUnderTest = new TaskCreationRuleService(context, null);
        }

        [Fact]
        public async Task GetTaskCreationRuleTest()
        {
            var workspace = await PrepareOneWorkspaceAsync();
            var taskRuleDataModel = await PrepareTaskRuleAsync(workspace.Id.ToString());
            var taskRuleServiceModel = await serviceUnderTest.GetTaskCreationRuleAsync(workspace.Id.ToString());
            Assert.NotNull(taskRuleServiceModel);
            Assert.NotNull(taskRuleDataModel);
            Assert.Equal(taskRuleServiceModel.WorkspaceId, taskRuleDataModel.WorkspaceId.ToString());
            Assert.Equal(taskRuleServiceModel.Rules.Count(), taskRuleDataModel.Rules.Count());
        }

        [Theory]
        [InlineData(true,true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task UpdateRule(bool isExist,bool validUser)
        {
            var workspace = await PrepareOneWorkspaceAsync();
            var users = await PrepareUserDatasAsync();
            var taskRule = await PrepareTaskRuleAsync(workspace.Id.ToString());
            var updateServiceModel = new TaskCreationRuleUpdateServiceModel
            {
                CreateUser = validUser ? users.FirstOrDefault(x => x.Roles.Contains(UserRoleEnum.Manager))?.Id.ToString() : ObjectId.GenerateNewId().ToString(),
                Rules = taskRule.Rules?.Select(rule => new AutoTaskRuleServiceModel
                {
                    Annotators = Enumerable.Range(1, 3).Select(x => ObjectId.GenerateNewId().ToString()).ToList(),
                    MaxFinishDays = 14,
                    DocumentCount = 2,
                    DocumentTags = new List<string>() { $"rule_{rule.Name}_tag1", $"rule_{rule.Name}_tag2" },
                    IsAutoApproved = !rule.IsAutoApproved,
                    IsAutoMerged = !rule.IsAutoMerged,
                    Name = rule.Name,
                    Overlap = rule.Overlap
                })?.ToList()
            };
            if (isExist)
            {
                await serviceUnderTest.UpdateRuleAsync(workspace.Id.ToString(), updateServiceModel);

                var taskRuleDataModel = await context.AutoTaskCreationRules.Find(u => u.WorkspaceId == workspace.Id).FirstAsync();
                Assert.NotNull(taskRuleDataModel);
                if (validUser)
                {
                    Assert.Equal(updateServiceModel.CreateUser, taskRuleDataModel.CreatedUser.ToString());
                }
                else
                {
                    Assert.NotEqual(updateServiceModel.CreateUser, taskRuleDataModel.CreatedUser.ToString());
                }

                Assert.All(taskRuleDataModel.Rules, a =>
                {
                    var taskRuleExpected = updateServiceModel.Rules.Where(u => u.Name.Equals(a.Name)).First();
                    Assert.Equal(taskRuleExpected.Name, a.Name);
                    Assert.Equal(taskRuleExpected.Annotators, a.Annotators.Select(x => x.ToString()).ToList());
                    Assert.Equal(taskRuleExpected.MaxFinishDays, a.MaxFinishDays);
                    Assert.Equal(taskRuleExpected.DocumentTags, a.DocumentTags);
                    Assert.Equal(taskRuleExpected.DocumentCount, a.DocumentCount);
                    Assert.Equal(taskRuleExpected.IsAutoApproved, a.IsAutoApproved);
                    Assert.Equal(taskRuleExpected.IsAutoMerged, a.IsAutoMerged);
                    Assert.Equal(taskRuleExpected.Overlap, a.Overlap);
                });
            }
            else
            {
                var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => serviceUnderTest.UpdateRuleAsync(ObjectId.GenerateNewId().ToString(), updateServiceModel));
                Assert.Equal(MusicKGMessages.AutoTaskCreationRuleNotExistsMessage, exception.Message);
            }
        }

        private async Task<AutoTaskCreationRulesDataModel> PrepareTaskRuleAsync(string workspaceId)
        {
            var taskRules = new AutoTaskCreationRulesDataModel()
            {
                Id = ObjectId.GenerateNewId(),
                WorkspaceId = ObjectId.Parse(workspaceId),
                CreatedUser = ObjectId.GenerateNewId(),
                Rules = Enumerable.Range(1, 3).Select(i => new AutoTaskRuleDataModel
                {
                    Name = $"Rule_{i}",
                    Annotators = Enumerable.Range(1, 3).Select(x => ObjectId.GenerateNewId()).ToList(),
                    MaxFinishDays = 7,
                    DocumentCount = 1,
                    DocumentTags = new List<string>() { },
                    IsAutoApproved = true,
                    IsAutoMerged = true,
                    Overlap = 1
                }).ToList(),
            };

            await context.AutoTaskCreationRules.InsertOneAsync(taskRules);
            return taskRules;
        }

        private async Task<List<UserDataModel>> PrepareUserDatasAsync()
        {
            var users = Enumerable.Range(1, 3).Select(i => new UserDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"Annoator_{i}",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Annotator }
            }).ToList();
            users.Add(new UserDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Admin",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Administrator }
            });
            users.Add(new UserDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Manager",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Manager }
            });
            await context.Users.InsertManyAsync(users);

            return users;
        }

        private async Task<WorkspaceDataModel> PrepareOneWorkspaceAsync()
        {
            var workspace = new WorkspaceDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"WorkspaceName",
                Type = ObjectId.GenerateNewId(),
                Language = LanguageEnum.Chinese,
                IsAutoMerging = true,
                Description = "Description",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = ObjectId.GenerateNewId(),
            };
            await context.Workspaces.InsertOneAsync(workspace);
            return workspace;
        }
    }
}
