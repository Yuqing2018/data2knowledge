using Moq;
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

namespace MusicKG.WebApi.Test.Controllers
{
    public class DictionaryControllerTest
    {
        public DictionaryControllerTest()
        {
        }

        [Fact]
        public async Task Create()
        {
            DictionaryCreateBindingModel bindingModel = new DictionaryCreateBindingModel
            {
                Name = "dictionary1",
                EntityId = "EntityId",
                Vocabularies = new List<string>() { "word1", "word2" }
            };

            DictionaryServiceModel serviceModel = new DictionaryServiceModel
            {
                WorkspaceId = "workspaceId",
                Id = "mockid",
                Name = "dictionary1",
                EntityId = "EntityId",
                Vocabularies = new List<string>() { "word1", "word2" },
                CreatedBy = "mockid",
                CreatedAt = DateTime.UtcNow,
            };

            var service = new Mock<IDictionaryService>();
            service.Setup(m => m.CreateDictionaryAsync(It.IsAny<DictionaryCreateServiceModel>())).Returns(Task.FromResult(serviceModel));

            DictionaryController controller = new DictionaryController(service.Object);

            var result = await controller.CreateDictionary(serviceModel.WorkspaceId, bindingModel);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.NotEmpty(result.Id);
            Assert.Equal(bindingModel.Name, result.Name);
        }

        [Fact]
        public async Task Update()
        {
            DictionaryUpdateBindingModel bindingModel = new DictionaryUpdateBindingModel()
            {
                Name = "newdictionary",
                EntityId = "newEntityId",
                Vocabularies = new List<string>() { "newword1", "newword2" }
            };

            DictionaryServiceModel serviceModel = new DictionaryServiceModel
            {
                WorkspaceId = "workspaceId",
                Id = "mockid",
                Name = "newdictionary",
                EntityId = "newEntityId",
                CreatedBy = "mockid",
                Vocabularies = new List<string>() { "newword1", "newword2" },
            };

            var service = new Mock<IDictionaryService>();
            service.Setup(m => m.UpdateDictionaryAsync(serviceModel.WorkspaceId, serviceModel.Id,
                It.IsAny<DictionaryUpdateServiceModel>())).Returns(Task.FromResult(serviceModel));

            DictionaryController controller = new DictionaryController(service.Object);
            var result = await controller.UpdateDictionary(serviceModel.WorkspaceId, serviceModel.Id, bindingModel);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.NotEmpty(result.Id);
            Assert.Equal(result.Name, bindingModel.Name);
            Assert.Equal(result.Vocabularies, bindingModel.Vocabularies);
        }

        [Fact]
        public async Task GetDictionary()
        {
            DictionaryServiceModel serviceModel = new DictionaryServiceModel
            {
                WorkspaceId = "workspaceId",
                Id = "mockid",
                Name = "newdictionary",
                EntityId = "newEntityId",
                CreatedBy = "mockuserId",
                CreatedAt = DateTime.UtcNow,
                Vocabularies = new List<string>() { "newword1", "newword2" },
            };

            var service = new Mock<IDictionaryService>();
            service.Setup(m => m.GetDictionaryAsync("workspaceId", "mockid")).Returns(Task.FromResult(serviceModel));

            DictionaryController controller = new DictionaryController(service.Object);

            var actualModel = await controller.GetDictionary("workspaceId", "mockid", 0, null);

            Assert.NotNull(actualModel);
            Assert.Equal(serviceModel.Id, actualModel.Id);
            Assert.Equal(serviceModel.Name, actualModel.Name);
            Assert.Equal(serviceModel.Vocabularies.Count(), actualModel.Entries.TotalCount);
        }

        [Fact]
        public async Task GetDictionaries()
        {
            IEnumerable<DictionaryListItemServiceModel> rowdatas = new List<DictionaryListItemServiceModel>()
            {
                new DictionaryListItemServiceModel
                {
                    Id = "mockId1",
                    WorkspaceId = "workspace1",
                    Name = "dictionary1",
                    EntityId = "EntityId1"
                },
                new DictionaryListItemServiceModel
                {
                    Id = "mockId2",
                    WorkspaceId = "workspace1",
                    Name = "dictionary2",
                    EntityId = "EntityId2"
                },
                new DictionaryListItemServiceModel
                {
                    Id = "mockId3",
                    WorkspaceId = "workspace1",
                    Name = "dictionary3",
                    EntityId = "EntityId3"
                },
                new DictionaryListItemServiceModel
                {
                    Id = "mockId4",
                    WorkspaceId = "workspace1",
                    Name = "dictionary4",
                    EntityId = "EntityId4"
                }
            };

            long count = rowdatas.ToList().Count;

            var serviceModelTuple = new Tuple<long, IEnumerable<DictionaryListItemServiceModel>>(count, rowdatas);

            var service = new Mock<IDictionaryService>();
            service.Setup(m => m.GetDictionariesAsync("workspace1", 0, null, true)).Returns(Task.FromResult(serviceModelTuple));

            DictionaryController controller = new DictionaryController(service.Object);
            var actualDatas = await controller.GetDictionaries("workspace1", 0, null);

            VerifyResult(serviceModelTuple.Item2.ToArray(), actualDatas.Items.ToArray());
        }

        [Fact]
        public async Task GetDictionaryEntries()
        {
            IEnumerable<string> rowdatas = new List<string>()
            {
                "城市a","a行业","建筑a","a天气","a季节"
            };

            long count = rowdatas.ToList().Count;

            var expectedDatas = new Tuple<long, IEnumerable<string>>(count, rowdatas);

            var service = new Mock<IDictionaryService>();
            service.Setup(m => m.GetDictionaryEntriesAsync("workspace1", "dictionaryId", "a", 0, null)).Returns(Task.FromResult(expectedDatas));

            DictionaryController controller = new DictionaryController(service.Object);
            var actualDatas = await controller.GetDictionaryEntries("workspace1", "dictionaryId", "a", 0, null);

            Assert.NotNull(actualDatas);
            Assert.Equal(expectedDatas.Item2.Count(), actualDatas.Items.Count());

            for (int i = 0; i < expectedDatas.Item2.Count(); i++)
            {
                Assert.Equal(expectedDatas.Item2.ElementAt(i), actualDatas.Items.ElementAt(i));
            }
        }

        private void VerifyResult(DictionaryListItemServiceModel[] rawData, DictionaryListItemViewModel[] actualData)
        {
            Assert.NotNull(actualData);
            Assert.Equal(rawData.Length, actualData.Length);

            for (int i = 0; i < rawData.Count(); i++)
            {
                Assert.Equal(rawData[i].Id, actualData[i].Id);
                Assert.Equal(rawData[i].Name, actualData[i].Name);
                Assert.Equal(rawData[i].WorkspaceId, actualData[i].WorkspaceId);
                Assert.Equal(rawData[i].EntityId, actualData[i].EntityId);
            }
        }

    }
}
