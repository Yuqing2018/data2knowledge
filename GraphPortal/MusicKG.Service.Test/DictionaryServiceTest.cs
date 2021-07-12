using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.Service.Test
{
    [Collection("MongoCollection")]
    public class DictionaryServiceTest
    {
        private readonly MongoFixture mongoFixture;
        private readonly IDictionaryService serviceUnderTest;
        private readonly IMusicKGContext context;

        public DictionaryServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.serviceUnderTest = new DictionaryService(context, null);
        }

        [Fact]
        public async Task Create()
        {
            var user = await PrepareUserAsync("userTest");
            var createServiceModel = new DictionaryCreateServiceModel
            {
                Name = "dictionary1",
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Vocabularies = new List<string>() { "word1", "word2" },
                CreatedBy = user.Id.ToString()
            };

            var dictionaryServiceModel = await serviceUnderTest.CreateDictionaryAsync(createServiceModel);

            Assert.NotNull(dictionaryServiceModel);
            Assert.NotEmpty(dictionaryServiceModel.Id.ToString());
            Assert.Equal(createServiceModel.Name, dictionaryServiceModel.Name);

            var dictionaryDataModel = await context.Dictionaries.Find(u => u.Id == new ObjectId(dictionaryServiceModel.Id)).FirstOrDefaultAsync();

            Assert.NotNull(dictionaryDataModel);
            Assert.NotEmpty(dictionaryDataModel.Id.ToString());
            Assert.Equal(createServiceModel.Name, dictionaryDataModel.Name);
        }

        [Fact]
        public async Task Update()
        {
            var user = await PrepareUserAsync("userTest");
            DictionaryDataModel existingDictionary = new DictionaryDataModel
            {
                WorkspaceId = ObjectId.GenerateNewId(),
                Name = "dictionary1",
                Vocabularies = new List<string>() { "word1", "word2" },
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id,
                IsDeleted = false
            };

            await context.Dictionaries.InsertOneAsync(existingDictionary);

            var updateServiceModel = new DictionaryUpdateServiceModel
            {
                Name = "newdictionary",
                IsNameAssigned = true,
                Vocabularies = new List<string>() { "World", "word" },
                IsVocabulariesAssigned = true
            };

            var dictionaryServiceModel = await serviceUnderTest.UpdateDictionaryAsync(existingDictionary.WorkspaceId.ToString(), existingDictionary.Id.ToString(), updateServiceModel);

            Assert.NotNull(dictionaryServiceModel);
            Assert.Equal(dictionaryServiceModel.Name, updateServiceModel.Name);

            var dictionaryDataModel = await context.Dictionaries.Find(u => u.WorkspaceId == existingDictionary.WorkspaceId && u.Id == existingDictionary.Id).FirstOrDefaultAsync();

            Assert.NotNull(dictionaryDataModel);
            Assert.Equal(dictionaryDataModel.Name, updateServiceModel.Name);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(20, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(20, false)]
        [InlineData(100, false)]
        public async void AutoUpdateDictTest(int newEntriesCount, bool includeExists)
        {
            var workspaceId = ObjectId.GenerateNewId();
            var user = await PrepareUserAsync("userTest");
            var dictDatas = Enumerable.Range(0, 2).Select(j => new DictionaryDataModel
            {
                WorkspaceId = workspaceId,
                Name = j == 0 ? "零件名" : "故障原因",
                Vocabularies = Enumerable.Range(0, 200).Select(v => RandomStringHelper.RandomString(20)).ToList(),
                CreatedBy = user.Id,
                IsDeleted = false
            }).ToList();

            await context.Dictionaries.InsertManyAsync(dictDatas);

            var part_dict = dictDatas.First();
            var updatedEntries = includeExists ? part_dict.Vocabularies.ToList() : new List<string>();

            updatedEntries.AddRange(Enumerable.Range(0, newEntriesCount).Select(v => $"零件名_{RandomStringHelper.RandomString(10)}"));

            await serviceUnderTest.UpdateOneVocabsAsync(workspaceId.ToString(), part_dict.Id.ToString(), updatedEntries);

            var actualResult = await context.Dictionaries.Find(u => u.WorkspaceId == workspaceId && u.Id == part_dict.Id).FirstOrDefaultAsync();

            Assert.Equal(part_dict.Vocabularies.Count + newEntriesCount, actualResult.Vocabularies.Count);
        }

        [Fact]
        public async Task Delete()
        {
            var existDictionaries = await PrepareDatasAsync(ObjectId.GenerateNewId());

            var dictionary = existDictionaries[0];
            var dictionsryIdToBeDeleted = dictionary.Id;

            await serviceUnderTest.DeleteDictionaryAsync(dictionary.WorkspaceId.ToString(), dictionary.Id.ToString());

            var actualDictionary = await this.context.Dictionaries.Find(x => x.Id == dictionsryIdToBeDeleted).FirstOrDefaultAsync();

            Assert.NotNull(actualDictionary);
            Assert.True(actualDictionary.IsDeleted);
        }

        [Fact]
        public async Task GetDictionary()
        {
            ObjectId testWorkspaceId = ObjectId.GenerateNewId();
            var existDictionaries = await PrepareDatasAsync(testWorkspaceId);

            var expectedData = existDictionaries.First();

            var actualData = await serviceUnderTest.GetDictionaryAsync(testWorkspaceId.ToString(), expectedData.Id.ToString());

            Assert.Equal(expectedData.Id.ToString(), actualData.Id);
            Assert.Equal(expectedData.Name, actualData.Name);
            Assert.Equal(expectedData.WorkspaceId.ToString(), actualData.WorkspaceId);
        }

        [Fact]
        public async Task GetDictionaries()
        {
            ObjectId testWorkspaceId = ObjectId.GenerateNewId();
            var existDictionaries = await PrepareDatasAsync(testWorkspaceId);
            var expectedData = existDictionaries.OrderByDescending(x => x.CreatedAt).ToList();
            
            var (totalCount, actualData) = await serviceUnderTest.GetDictionariesAsync(testWorkspaceId.ToString(), 0, null);
            var actualResult = actualData.ToList();

            Assert.Equal(expectedData.Count, totalCount);

            for (int i = 0; i < totalCount; i++)
            {
                AssertServiceModel(expectedData[i], actualResult[i]);
            }
        }

        [Fact]
        public async Task GetDictionaryEntries()
        {
            ObjectId testWorkspaceId = ObjectId.GenerateNewId();
            var existDictionaries = await PrepareDatasAsync(testWorkspaceId);
            var dictionaryToSearch = existDictionaries.Last();
            var filterStr = "p";

            var expectedEntries = dictionaryToSearch.Vocabularies.Where(x => x.Contains(filterStr, StringComparison.OrdinalIgnoreCase)).ToList();

            var (totalCount, actualData) = await serviceUnderTest.GetDictionaryEntriesAsync(testWorkspaceId.ToString(), dictionaryToSearch.Id.ToString(), filterStr, 0, null);
            var actualResult = actualData.ToList();

            Assert.Equal(expectedEntries.Count, totalCount);

            for (int i = 0; i < totalCount; i++)
            {
                Assert.Equal(expectedEntries[i], actualResult[i]);
            }
        }

        private async Task<UserDataModel> PrepareUserAsync(string userName)
        {
            var salt = RandomStringHelper.RandomString(256);
            var userExisting = new UserDataModel
            {
                Name = userName,
                Salt = salt,
                Password = UserService.HashPassword(salt, "password"),
                Tokens = new List<UserTokenDataModel>(),
                Status = UserStatusEnum.Enabled,
                Roles = new List<UserRoleEnum>(new UserRoleEnum[] { UserRoleEnum.Manager }),
                CreatedBy = "user1"
            };

            await context.Users.InsertOneAsync(userExisting);
            return userExisting;
        }

        private async Task<List<DictionaryDataModel>> PrepareDatasAsync(ObjectId workspaceId)
        {
            var user = await PrepareUserAsync("userTest");

            List<DictionaryDataModel> result = new List<DictionaryDataModel>()
            {
                new DictionaryDataModel
                {
                    WorkspaceId = workspaceId,
                    Name = "dictionary1",
                    Vocabularies = new List<string>() { "word1", "word2" },
                    CreatedBy = user.Id,
                    IsDeleted = false
                },
                new DictionaryDataModel
                {
                    WorkspaceId = workspaceId,
                    Name = "dictionary2",
                    Vocabularies = new List<string>() { "word3", "word4" },
                    CreatedBy = user.Id,
                    IsDeleted = false
                },
                new DictionaryDataModel
                {
                    WorkspaceId = workspaceId,
                    Name = "dictionary3",
                    Vocabularies = new List<string>() { "potato", "tomato" },
                    CreatedBy = user.Id,
                    IsDeleted = false
                },
                new DictionaryDataModel
                {
                    WorkspaceId = workspaceId,
                    Name = "dictionary4",
                    Vocabularies = new List<string>() { "World", "word", "possible", "animal", "person" },
                    CreatedBy = user.Id,
                    IsDeleted = false
                }
            };

            await context.Dictionaries.InsertManyAsync(result);

            return result;
        }

        private void AssertServiceModel(DictionaryDataModel expected, DictionaryListItemServiceModel actual)
        {
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.WorkspaceId.ToString(), actual.WorkspaceId);
            Assert.Equal(expected.Vocabularies.Count(), actual.EntriesCount);
        }
    }
}
