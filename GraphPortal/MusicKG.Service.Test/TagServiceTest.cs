using MongoDB.Bson;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Fixtures;
using MusicKG.Service.Test.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static MusicKG.Service.Helpers.ErrorHelper;

namespace MusicKG.Service.Test
{
    public class TagServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        public TagServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
        }

        #region GetTagValues

        [Theory]
        [ClassData(typeof(ClassDataForPagination))]
        public async Task GetTagValuesPaginationTest(PaginationSkip skip, PaginationLimit limit)
        {
            var (_, tags) = await PrepareDataAsync();
            var expectedTag = tags.First();

            var expectedTotalCount = tags.First().Values.Count;

            int skipValue = PaginationHelper.GetSkipValue(skip, expectedTotalCount);
            int? limitValue = PaginationHelper.GetLimitValue(limit, expectedTotalCount, skipValue);

            var service = new TagService(context, null);

            var (actualTotalCount, actualTagValues) = await service.GetTagValuesAsync(expectedTag.WorkspaceId.ToString(), expectedTag.Type, "", skipValue, limitValue);

            Assert.Equal(expectedTotalCount, actualTotalCount);
            Assert.NotNull(actualTagValues);
            Assert.Equal(actualTagValues.Count(), PaginationHelper.GetExpectedCount(skipValue, limitValue, expectedTotalCount));
        }

        [Theory]
        [InlineData(0, TagTypeEnum.None, true)]
        [InlineData(0, TagTypeEnum.Document, true)]
        [InlineData(0, TagTypeEnum.Paragraph, true)]
        [InlineData(0, TagTypeEnum.Graph, true)]
        [InlineData(1, TagTypeEnum.None, true)]
        [InlineData(1, TagTypeEnum.Document, true)]
        [InlineData(1, TagTypeEnum.Paragraph, true)]
        [InlineData(1, TagTypeEnum.Graph, true)]
        [InlineData(2, TagTypeEnum.None, true)]
        [InlineData(2, TagTypeEnum.Document, true)]
        [InlineData(2, TagTypeEnum.Paragraph, true)]
        [InlineData(2, TagTypeEnum.Graph, true)]
        [InlineData(3, TagTypeEnum.None, true)]
        [InlineData(3, TagTypeEnum.Document, true)]
        [InlineData(3, TagTypeEnum.Paragraph, true)]
        [InlineData(3, TagTypeEnum.Graph, true)]
        [InlineData(0, TagTypeEnum.None, false)]
        [InlineData(0, TagTypeEnum.Document, false)]
        [InlineData(0, TagTypeEnum.Paragraph, false)]
        [InlineData(0, TagTypeEnum.Graph, false)]
        [InlineData(1, TagTypeEnum.None, false)]
        [InlineData(1, TagTypeEnum.Document, false)]
        [InlineData(1, TagTypeEnum.Paragraph, false)]
        [InlineData(1, TagTypeEnum.Graph, false)]
        [InlineData(2, TagTypeEnum.None, false)]
        [InlineData(2, TagTypeEnum.Document, false)]
        [InlineData(2, TagTypeEnum.Paragraph, false)]
        [InlineData(2, TagTypeEnum.Graph, false)]
        [InlineData(3, TagTypeEnum.None, false)]
        [InlineData(3, TagTypeEnum.Document, false)]
        [InlineData(3, TagTypeEnum.Paragraph, false)]
        [InlineData(3, TagTypeEnum.Graph, false)]
        public async Task GetTagValuesTest(int workspaceIndex, TagTypeEnum type, bool emptyFilter)
        {
            var (workspaces, tags) = await PrepareDataAsync();
            var workspaceId = workspaceIndex >= workspaces.Count() ? ObjectId.GenerateNewId().ToString() : workspaces.ElementAt(workspaceIndex).Id.ToString();

            var filter = "";
            if (!emptyFilter)
            {
                filter = "Tag_1";
            }

            var expectedTag = tags.Where(tag => tag.WorkspaceId.ToString() == workspaceId && tag.Type == type).FirstOrDefault();
            var expectedTagValues = new List<TagValueDataModel>();
            if (expectedTag != null)
            {
                expectedTagValues = expectedTag.Values.Where(value => value.Value.Contains(filter)).ToList();
            }

            var service = new TagService(context, null);

            var (actualTotalCount, actualTagValues) = await service.GetTagValuesAsync(workspaceId, type, filter, 0, null);

            Assert.NotNull(actualTagValues);
            Assert.Equal(expectedTagValues.Count, actualTagValues?.Count());
            Assert.All(actualTagValues, actualTagValue =>
            {
                var expectedValue = expectedTagValues.FirstOrDefault(value => value.Id.ToString() == actualTagValue.Id);

                Assert.Equal(expectedValue.Value, actualTagValue.Value);
                Assert.Equal(expectedValue.Color, actualTagValue.Color);
                Assert.Equal(expectedValue.Description, actualTagValue.Description);
            });
        }

        #endregion

        #region Add Tag Values

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public async Task AddTagValuesTest(bool tagExists, bool hasDuplicateValues)
        {
            var (workspaces, tags) = await PrepareDataAsync();

            string workspaceId;
            TagTypeEnum type;
            TagDataModel expectedTag = null;

            if (tagExists)
            {
                expectedTag = tags.First();
                workspaceId = expectedTag.WorkspaceId.ToString();
                type = expectedTag.Type;
            }
            else
            {
                var workspace = new WorkspaceDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "NewWorkspace"
                };
                await context.Workspaces.InsertOneAsync(workspace);
                workspaceId = workspace.Id.ToString();
                type = TagTypeEnum.None;
            }

            var serviceModel = new TagServiceModel
            {
                WorkspaceId = workspaceId,
                Type = type,
                Values = tagExists ?
                    expectedTag.Values.Select(value => new TagValueServiceModel { Value = value.Value + "_New", Color = value.Color, Description = value.Description + "_New" }).ToList() :
                    Enumerable.Range(1, new Random().Next(3, 10)).Select(i => new TagValueServiceModel { Value = $"NewTag_{i}", Color = $"TestColor_{i}", Description = $"Description_{i}" }).ToList()
            };

            if (tagExists && hasDuplicateValues)
            {
                serviceModel.Values.AddRange(expectedTag.Values.Select(value => new TagValueServiceModel { Value = value.Value, Color = value.Color, Description = value.Description }));
            }

            var service = new TagService(context, null);

            await service.AddTagValuesAsync(serviceModel);

            var actualTag = await context.Tags.AsQueryable().Where(tag => tag.WorkspaceId == new ObjectId(workspaceId) && tag.Type == type).FirstOrDefaultAsync();

            Assert.NotNull(actualTag);

            if (tagExists)
            {
                var expectedValues = new List<TagValueDataModel>(expectedTag.Values.Select(value => value));
                foreach (var value in serviceModel.Values)
                {
                    if (!expectedValues.Any(expectedValue => expectedValue.Value == value.Value))
                    {
                        expectedValues.Add(new TagValueDataModel { Value = value.Value, Color = value.Color, Description = value.Description });
                    }
                }
                Assert.Equal(expectedValues.Count, actualTag.Values.Count);

                Assert.All(actualTag.Values, actualValue =>
                {
                    var expectedValue = expectedValues.First(e => e.Value == actualValue.Value);
                    Assert.Equal(expectedValue.Color, actualValue.Color);
                    Assert.Equal(expectedValue.Description, actualValue.Description);
                });
            }
            else
            {
                Assert.Equal(serviceModel.Values.Count, actualTag.Values.Count);
                Assert.All(actualTag.Values, actualValue =>
                {
                    var expectedValue = serviceModel.Values.First(e => e.Value == actualValue.Value);
                    Assert.Equal(expectedValue.Color, actualValue.Color);
                    Assert.Equal(expectedValue.Description, actualValue.Description);
                });
            }
        }

        #endregion

        #region Replace Tag

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public async Task ReplaceTagTest(bool tagExists, bool hasDuplicateValues)
        {
            var (workspaces, tags) = await PrepareDataAsync();

            string workspaceId;
            TagTypeEnum type;
            TagDataModel expectedTag = null;

            if (tagExists)
            {
                expectedTag = tags.First();
                workspaceId = expectedTag.WorkspaceId.ToString();
                type = expectedTag.Type;
            }
            else
            {
                var workspace = new WorkspaceDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "NewWorkspace"
                };
                await context.Workspaces.InsertOneAsync(workspace);
                workspaceId = workspace.Id.ToString();
                type = TagTypeEnum.None;
            }

            var serviceModel = new TagServiceModel
            {
                WorkspaceId = workspaceId,
                Type = type,
                Values = tagExists ?
                    expectedTag.Values.Select(value => new TagValueServiceModel { Value = value.Value + "_New", Color = value.Color, Description = value.Description + "_New" }).ToList() :
                    Enumerable.Range(1, new Random().Next(3, 10)).Select(i => new TagValueServiceModel { Value = $"NewTag_{i}", Color = $"TestColor_{i}", Description = $"TestDescription_{i}" }).ToList()
            };

            if (tagExists && hasDuplicateValues)
            {
                serviceModel.Values.AddRange(expectedTag.Values.Select(value => new TagValueServiceModel { Value = value.Value, Color = value.Color, Description = value.Description }));
            }

            var service = new TagService(context, null);

            await service.ReplaceTagAsync(serviceModel);

            var actualTag = await context.Tags.AsQueryable().Where(tag => tag.WorkspaceId == new ObjectId(workspaceId) && tag.Type == type).FirstOrDefaultAsync();

            Assert.NotNull(actualTag);

            Assert.Equal(serviceModel.Values.Count, actualTag.Values.Count);
            Assert.All(actualTag.Values, actualValue =>
            {
                var expectedValue = serviceModel.Values.First(e => e.Value == actualValue.Value);
                Assert.Equal(expectedValue.Color, actualValue.Color);
                Assert.Equal(expectedValue.Description, actualValue.Description);
            });
        }

        #endregion

        #region Update Tag Value

        [Theory]
        [InlineData(TagTypeEnum.None)]
        [InlineData(TagTypeEnum.Document)]
        [InlineData(TagTypeEnum.Paragraph)]
        [InlineData(TagTypeEnum.Graph)]
        public async Task UpdateTagValueTest(TagTypeEnum type)
        {
            var (workspaces, tags) = await PrepareDataAsync();
            var workspaceId = workspaces.First().Id.ToString();

            var expectedTag = tags.Where(tag => tag.WorkspaceId.ToString() == workspaceId && tag.Type == type).FirstOrDefault();
            string valueId = expectedTag.Values.First().Id.ToString();

            var service = new TagService(context, null);

            var serviceModel = new TagValueUpdateServiceModel
            {
                IsValueAssigned = true,
                Value = RandomStringHelper.RandomString(10),
                IsColorAssigned = true,
                Color = RandomStringHelper.RandomColorString(),
                IsDescriptionAssigned = true,
                Description = RandomStringHelper.RandomString(10)
            };
            
            await service.UpdateTagValueAsync(workspaceId, type, valueId, serviceModel);

            var actualTag = await context.Tags.AsQueryable().Where(tag => tag.WorkspaceId == new ObjectId(workspaceId) && tag.Type == type).FirstOrDefaultAsync();

            var actualValues = actualTag.Values;
            var actualValue = actualValues.FirstOrDefault(value => value.Id.ToString() == valueId);
            Assert.Equal(serviceModel.Value, actualValue.Value);
            Assert.Equal(serviceModel.Color, actualValue.Color);
            Assert.Equal(serviceModel.Description, actualValue.Description);
            Assert.All(actualValues, value =>
            {
                if (value.Id.ToString() != valueId)
                {
                    var expectedValue = expectedTag.Values.First(tmpValue => tmpValue.Id == value.Id);
                    Assert.Equal(expectedValue.Value, value.Value);
                    Assert.Equal(expectedValue.Color, value.Color);
                    Assert.Equal(expectedValue.Description, value.Description);
                }
            });
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public async Task UpdateTagFailedTest(bool workspaceExists, bool valueExists)
        {
            var (workspaces, tags) = await PrepareDataAsync();
            var workspaceId = workspaceExists ? workspaces.First().Id.ToString() : ObjectId.GenerateNewId().ToString();
            var tagToBeUpdated = tags.FirstOrDefault(tag => tag.WorkspaceId.ToString() == workspaceId && tag.Type == TagTypeEnum.None);
            var valueToBeUpdated = tagToBeUpdated?.Values?.First();

            var serviceModel = new TagValueUpdateServiceModel
            {
                IsValueAssigned = true,
                Value = valueExists ? tagToBeUpdated?.Values?.LastOrDefault()?.Value : RandomStringHelper.RandomString(10),
                IsColorAssigned = true,
                Color = RandomStringHelper.RandomColorString(),
                IsDescriptionAssigned = true,
                Description = RandomStringHelper.RandomString(10)
            };

            var service = new TagService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorMessageException>(() => service.UpdateTagValueAsync(workspaceId, TagTypeEnum.None, valueToBeUpdated == null ? ObjectId.GenerateNewId().ToString() : valueToBeUpdated.Id.ToString(), serviceModel));
            if (!workspaceExists)
            {
                Assert.Equal(Resources.MusicKGMessages.NoTagValueCanBeUpdatedMessage, exception.Message);
            }
            else if (valueExists)
            {
                Assert.Equal(Resources.MusicKGMessages.ValueAlreadyExistsMessage, exception.Message);
            }
        }

        #endregion

        #region Delete tag values

        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(1, false)]
        [InlineData(3, true)]
        public async Task DeleteTagValuesTest(int workspaceIndex, bool oneOrMore)
        {
            var (workspaces, tags) = await PrepareDataAsync();
            var workspaceId = workspaceIndex >= workspaces.Count() ? ObjectId.GenerateNewId().ToString() : workspaces.ElementAt(workspaceIndex).Id.ToString();

            var expectedTag = tags.Where(tag => tag.WorkspaceId.ToString() == workspaceId && tag.Type == TagTypeEnum.None).FirstOrDefault();

            var valuesToBeDelete = expectedTag?.Values?.Take(oneOrMore ? 1 : new Random().Next(3, expectedTag.Values.Count))?.Select(value => value.Id.ToString());

            if (valuesToBeDelete == null)
            {
                valuesToBeDelete = Enumerable.Range(1, 10).Select(i => ObjectId.GenerateNewId().ToString());
            }

            var service = new TagService(context, null);

            await service.DeleteTagValuesAsync(workspaceId, TagTypeEnum.None, valuesToBeDelete.ToList());

            var actualTag = await context.Tags.AsQueryable().Where(tag => tag.WorkspaceId == new ObjectId(workspaceId) && tag.Type == TagTypeEnum.None).FirstOrDefaultAsync();

            if (workspaceIndex >= workspaces.Count())
            {
                Assert.Null(actualTag);
            }
            else
            {
                Assert.Equal(expectedTag.Values.Count - valuesToBeDelete.Count(), actualTag.Values.Count);
            }
        }

        #endregion

        private async Task<(IEnumerable<WorkspaceDataModel>, IEnumerable<TagDataModel>)> PrepareDataAsync()
        {
            var workspaces = Enumerable.Range(1, 3).Select(i => new WorkspaceDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"TestWorkspace{i}",
            }).ToList();

            await context.Workspaces.InsertManyAsync(workspaces);

            var tags = workspaces.SelectMany((workspace, i) => Enum.GetValues(typeof(TagTypeEnum)).Cast<TagTypeEnum>().Select(type =>
            {
                return new TagDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    WorkspaceId = workspace.Id,
                    Type = type,
                    Values = Enumerable.Range(1, new Random().Next(3, 10)).Select(j =>
                    {
                        return new TagValueDataModel
                        {
                            Id = ObjectId.GenerateNewId(),
                            Value = $"Workspace_{i}_Type_{type}_Tag_{j}",
                            Description = $"Workspace_{i}_Type_{type}_Tag_{j}_Description",
                            Color = RandomStringHelper.RandomColorString()
                        };
                    }).ToList()
                };
            }).ToList()).ToList();

            await context.Tags.InsertManyAsync(tags);

            return (workspaces, tags);
        }
    }

    public class ClassDataForPagination : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var skips = Enum.GetValues(typeof(PaginationSkip));
            var limits = Enum.GetValues(typeof(PaginationLimit));
            foreach (var skip in skips)
            {
                foreach (var limit in limits)
                {
                    yield return new object[] { skip, limit };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
