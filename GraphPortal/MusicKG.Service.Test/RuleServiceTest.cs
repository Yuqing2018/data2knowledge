using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using MusicKG.Service.Test.Fixtures;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using Xunit;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    public class RuleServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        public RuleServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
        }

        #region Positive

        [Fact]
        public async Task Create()
        {
            var rule = new RuleCreateServiceModel()
            {
                EntityId = ObjectId.GenerateNewId(2).ToString(),
                Name = "zheng ze",
                Regexs = new List<string>()
                {
                    "aa", "dd", "cc"
                },
                WorkspaceId = ObjectId.GenerateNewId(2).ToString()
            };

            var service = new RuleService(context);

            var ruleServiceModel = await service.CreateRuleAsync(rule);
            Assert.NotNull(ruleServiceModel);

            var result = context.Rules.Find(m => m.Name == rule.Name).FirstOrDefault();
            Assert.NotNull(result.Id.ToString());
            Assert.Equal(rule.Name, result.Name);
            Assert.Equal(rule.EntityId, result.EntityId.ToString());
            Assert.Equal(rule.Regexs, result.Regexs);
            Assert.True(result.Regexs.All(rule.Regexs.Contains));
        }

        [Fact]
        public async Task Update()
        {
            var workspaceId = ObjectId.GenerateNewId(DateTime.Now).ToString();
            var rule = await PrepareData(workspaceId);

            var ruleId = rule.Id.ToString();
            var ruleUpdate = new RuleUpdateServiceModel()
            {
                IsEntityIdAssigned = true,
                IsNameAssigned = true,
                IsRegexAssigned = true,
                EntityId = ObjectId.GenerateNewId(2).ToString(),
                Name = "zheng ze2",
                Regexs = new List<string>()
                {
                    "dd", "dd", "cc"
                }
            };
            var service = new RuleService(context);

            await service.UpdateRuleAsync(ruleId, ruleUpdate);

            var ruleData = context.Rules.Find(m => m.Id == new ObjectId(ruleId)).FirstOrDefault();
            Assert.NotNull(ruleData.Id.ToString());
            Assert.Equal(ruleUpdate.EntityId, ruleData.EntityId.ToString());
            Assert.Equal(ruleUpdate.Name, ruleData.Name);
            Assert.True(ruleData.Regexs.All(ruleUpdate.Regexs.Contains));
        }

        [Fact]
        public async Task Delete()
        {
            var rule = await PrepareData(ObjectId.GenerateNewId(2).ToString());
            var service = new RuleService(context);

            var ruleId = rule.Id.ToString();
            await service.DeleteRuleAsync(ruleId);

            var delete = context.Rules.Find(m => m.Id == new ObjectId(ruleId)).FirstOrDefault();
            Assert.Null(delete);
        }

        [Fact]
        public async Task GetOne()
        {
            var workspaceId = ObjectId.GenerateNewId(DateTime.Now).ToString();
            var rule = await PrepareData(workspaceId);

            var service = new RuleService(context);

            var getResult = await service.GetRuleAsync(workspaceId, rule.Id.ToString());
            Assert.NotNull(getResult.Id);
            Assert.Equal(rule.Name, getResult.Name);
            Assert.Equal(rule.EntityId.ToString(), getResult.OntologyEntity.Id);
            Assert.Equal(rule.Regexs, getResult.Regexs);
        }

        [Fact]
        public async Task GetAll()
        {
            var workspaceId = ObjectId.GenerateNewId(DateTime.Now).ToString();
            var expectedRules = await PrepareDatas(workspaceId);

            var service = new RuleService(context);
            var getAll = await service.GetRulesAsync(workspaceId, 0, null);

            Assert.NotNull(getAll);
            Assert.Equal(expectedRules.Count, getAll.Item1);

            var result = getAll.Item2.ToList();
            for (int i = 0; i < result.Count; i++)
            {
                var expectedRule = expectedRules.First(r => r.Id.ToString().Equals(result[i].Id));
                Assert.Equal(expectedRule.WorkspaceId.ToString(), result[i].WorkspaceId);
                Assert.Equal(expectedRule.Name, result[i].Name);
                Assert.Equal(expectedRule.EntityId.ToString(), result[i].OntologyEntity.Id);
            }
        }

        #endregion

        #region Negative

        [Fact]
        public async Task CreateWithInvalidName()
        {
            var rule = await PrepareData(ObjectId.GenerateNewId(2).ToString());
            var create = new RuleCreateServiceModel()
            {
                EntityId = rule.EntityId.ToString(),
                Name = rule.Name,
                Regexs = rule.Regexs,
                WorkspaceId = rule.WorkspaceId.ToString()
            };

            var service = new RuleService(context);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.CreateRuleAsync(create));

            Assert.Equal(MusicKGMessages.RuleNameExistMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateWithInvalidId()
        {
            var rule = await PrepareData(ObjectId.GenerateNewId(2).ToString());

            var service = new RuleService(context);

            var ruleUpdate = new RuleUpdateServiceModel()
            {
                IsEntityIdAssigned = true,
                IsNameAssigned = true,
                IsRegexAssigned = true,
                EntityId = ObjectId.GenerateNewId(2).ToString(),
                Name = "zheng ze2",
                Regexs = new List<string>()
                {
                    "dd", "dd", "cc"
                }
            };

            var exception =
                await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.UpdateRuleAsync(ObjectId.GenerateNewId(2).ToString(), ruleUpdate));

            Assert.Equal(MusicKGMessages.RuleNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task GetOneWithInvalidId()
        {
            var rule = await PrepareData(ObjectId.GenerateNewId(2).ToString());

            var service = new RuleService(context);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                service.GetRuleAsync(rule.WorkspaceId.ToString(), ObjectId.GenerateNewId(112).ToString()));

            Assert.Equal(MusicKGMessages.RuleNotExistMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteWithInvalidId()
        {
            var rule = await PrepareData(ObjectId.GenerateNewId(2).ToString());

            var service = new RuleService(context);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() =>
                service.DeleteRuleAsync(ObjectId.GenerateNewId(2).ToString()));

            Assert.Equal(MusicKGMessages.RuleNotExistMessage, exception.Message);
        }

        #endregion

        private async Task<RuleDataModel> PrepareData(string workspaceId)
        {
            var entity = new OntologyEntityDataModel
            {
                Name = "TestEntity",
                Id = ObjectId.GenerateNewId(2),
                Color = "sss",
                Description = "Test",
                WorkspaceId = new ObjectId(workspaceId)
            };

            await context.OntologyEntities.InsertOneAsync(entity);

            var rule = new RuleDataModel()
            {
                Name = "Test",
                EntityId = entity.Id,
                Regexs = new List<string>() { "aa", "vv" },
                WorkspaceId = new ObjectId(workspaceId)
            };

            await context.Rules.InsertOneAsync(rule);

            return rule;
        }

        private async Task<List<RuleDataModel>> PrepareDatas(string workspaceId)
        {
            var entity = new OntologyEntityDataModel
            {
                Id = ObjectId.GenerateNewId(2),
                Name = "TestEntity"
            };

            await context.OntologyEntities.InsertOneAsync(entity);

            var rules = Enumerable.Range(1, 10).Select(m => new RuleDataModel
            {
                Name = "TestRuleName" + m,
                EntityId = entity.Id,
                Regexs = Enumerable.Range(10, 20).Select(s => $"TestRegex{s}").ToList(),
                WorkspaceId = new ObjectId(workspaceId)
            }).ToList();

            await context.Rules.InsertManyAsync(rules);

            return rules;
        }
    }
}
