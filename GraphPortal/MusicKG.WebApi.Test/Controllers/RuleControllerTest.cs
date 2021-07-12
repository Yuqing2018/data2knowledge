using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Moq;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.BindingModels;
using Xunit;

namespace MusicKG.WebApi.Test.Controllers
{
    public class RuleControllerTest
    {
        private readonly RuleController ruleController;
        private readonly Mock<IRuleService> ruleServiceMock;

        public RuleControllerTest()
        {
            this.ruleServiceMock = new Mock<IRuleService>();
            this.ruleController = new RuleController(this.ruleServiceMock.Object);
        }

        [Fact]
        public async void Create()
        {
            var ruleCreate = new RuleCreateBindingModel()
            {
                EntityId = "1",
                Name = "Create",
                Regexes = new List<string>() { "aa", "bb", "cc" }
            };

            var ruleId = "MockId";

            ruleServiceMock.Setup(m => m.CreateRuleAsync(It.IsAny<RuleCreateServiceModel>()))
                .Returns(Task.FromResult(ruleId));

            var result = await ruleController.CreateRule("1", ruleCreate);

            Assert.NotNull(result);
            Assert.Equal(ruleId, result);
        }

        [Fact]
        public async void GetAll()
        {
            var ruleServiceGet = new Tuple<long, IEnumerable<RuleServiceModel>>(1, Enumerable.Range(1, 20).Select(m => new RuleServiceModel
            {
                Id = $"MockId{m}",
                Name = $"MockName{m}",
                OntologyEntity = new OntologyEntityServiceModel
                {
                    Id = $"MockEntityId{m}",
                    Name = $"MockEntityName{m}"
                },
                WorkspaceId = $"MockWorkspaceId{m}"
            }));

            ruleServiceMock.Setup(m => m.GetRulesAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(ruleServiceGet));

            var ruleResult = await ruleController.GetRules("1", 0, 0);
            Assert.NotNull(ruleResult);

            var expected = ruleServiceGet.Item2.ToList();
            var actualResult = ruleResult.Items.ToList();
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.Equal(expected[i].Name, actualResult[i].Name);
                Assert.Equal(expected[i].OntologyEntity.Id, actualResult[i].EntityId);
                Assert.Equal(expected[i].WorkspaceId, actualResult[i].WorkspaceId);
                Assert.Equal(expected[i].OntologyEntity.Name, actualResult[i].EntityName);
            }
        }

        [Fact]
        public async void GetOne()
        {
            var ruleServiceResult = new RuleServiceModel()
            {
                OntologyEntity = new OntologyEntityServiceModel
                {
                    Id = ObjectId.GenerateNewId(2).ToString(),
                    Name = "MockName"
                },
                Id = "mock",
                Name = "bb",
                Regexs = new List<string>() { "aa", "vv" },
                WorkspaceId = "1"
            };

            ruleServiceMock.Setup(m => m.GetRuleAsync("1", "mock")).Returns(Task.FromResult(ruleServiceResult));

            var result = await ruleController.GetRule("1", "mock");
            Assert.NotNull(result);
            Assert.Equal(ruleServiceResult.Name, result.Name);
            Assert.Equal(ruleServiceResult.OntologyEntity.Id, result.EntityId);
            Assert.Equal(ruleServiceResult.Id, result.Id);
            Assert.Equal(ruleServiceResult.WorkspaceId, result.WorkspaceId);
        }
    }
}
