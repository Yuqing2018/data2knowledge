using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Resources;
using System.Net;

namespace MusicKG.Service.Implementations
{
    public class TagService : ITagService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<TagService> logger;

        /// <summary>
        /// Tag service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public TagService(
            IMusicKGContext context,
            ILogger<TagService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Add tag values.
        /// </summary>
        /// <param name="serviceModel">Tag service model.</param>
        /// <returns></returns>
        public async Task AddTagValuesAsync(TagServiceModel serviceModel)
        {
            var existingTag = await GetTagAsync(serviceModel.WorkspaceId, serviceModel.Type);

            if (existingTag == null)
            {
                await CreateTagAsync(serviceModel.WorkspaceId, serviceModel.Type, serviceModel.Values);
            }
            else
            {
                var existingValues = existingTag.Values.Select(value => value.Value).ToHashSet();

                var filterBuilder = Builders<TagDataModel>.Filter;
                var updateBuilder = Builders<TagDataModel>.Update;

                var filter = filterBuilder.Eq(tag => tag.WorkspaceId, new ObjectId(serviceModel.WorkspaceId)) & filterBuilder.Eq(tag => tag.Type, serviceModel.Type);

                var newValues = serviceModel.Values?.Where(value => !existingValues.Contains(value.Value))?.ToList();
            
                if (newValues != null && newValues.Count > 0)
                {
                    await context.Tags.UpdateOneAsync(filter, 
                        updateBuilder.PushEach(tag => tag.Values, 
                        newValues.Select(newValue => new TagValueDataModel
                        {
                            Id = ObjectId.GenerateNewId(),
                            Value = newValue.Value,
                            Description = newValue.Description,
                            Color = newValue.Color
                        })));
                }
            }
        }

        /// <summary>
        /// Delete tag values.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="valueIds">Tag value Ids.</param>
        /// <returns></returns>
        public async Task DeleteTagValuesAsync(string workspaceId, TagTypeEnum type, List<string> valueIds)
        {
            var filterBuilder = Builders<TagDataModel>.Filter;
            var updateBuilder = Builders<TagDataModel>.Update;

            var filter = filterBuilder.Eq(tag => tag.WorkspaceId, new ObjectId(workspaceId)) & filterBuilder.Eq(tag => tag.Type, type);

            var tmpValueIds = valueIds?.Select(id => new ObjectId(id)).ToList();
            
            var update = valueIds == null || valueIds.Count == 0 ? 
                updateBuilder.Set(tag => tag.Values, new List<TagValueDataModel>()) : 
                updateBuilder.PullFilter(tag => tag.Values, value => tmpValueIds.Contains(value.Id));

            await context.Tags.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Get tag values.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="strFilter">Tag value filter.</param>
        /// <param name="from">Pagination from.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns></returns>
        public async Task<(long total, IEnumerable<TagValueServiceModel> values)> GetTagValuesAsync(string workspaceId, TagTypeEnum type, string strFilter, int from, int? size)
        {
            var query = context.Tags.AsQueryable().Where(tag => tag.WorkspaceId == new ObjectId(workspaceId) && tag.Type == type)
                .SelectMany(tag => tag.Values);

            if (!string.IsNullOrEmpty(strFilter))
            {
                query = query.Where(tagValue => tagValue.Value.Contains(strFilter));
            }

            var totalCount = await query.LongCountAsync();

            var resultValues = await query.Skip(from < 0 ? 0 : from).Take(size == null || size.Value <= 0 ? int.MaxValue : size.Value).ToListAsync();

            return (totalCount, resultValues?.Select(value => new TagValueServiceModel
            {
                Id = value.Id.ToString(),
                Value = value.Value,
                Color = value.Color,
                Description = value.Description
            }));
        }

        /// <summary>
        /// Replace tag with given value.
        /// </summary>
        /// <param name="serviceModel">Tag service model.</param>
        /// <returns></returns>
        public async Task ReplaceTagAsync(TagServiceModel serviceModel)
        {
            var existingTag = await GetTagAsync(serviceModel.WorkspaceId, serviceModel.Type);

            if (existingTag == null)
            {
                await CreateTagAsync(serviceModel.WorkspaceId, serviceModel.Type, serviceModel.Values);
            }
            else
            {
                existingTag.Values = serviceModel.Values?.Select(value => new TagValueDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Value = value.Value,
                    Description = value.Description,
                    Color = value.Color
                })?.ToList();

                await context.Tags.ReplaceOneAsync(tag => tag.WorkspaceId == new ObjectId(serviceModel.WorkspaceId) && tag.Type == serviceModel.Type, existingTag);
            }
        }

        /// <summary>
        /// Update tag value.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="valueId">Value Id to be update.</param>
        /// <param name="value">Value to be updated.</param>
        /// <returns></returns>
        public async Task UpdateTagValueAsync(string workspaceId, TagTypeEnum type, string valueId, TagValueUpdateServiceModel serviceModel)
        {
            if (!serviceModel.IsColorAssigned && !serviceModel.IsDescriptionAssigned && !serviceModel.IsValueAssigned)
            {
                return;
            }

            var filterBuilder = Builders<TagDataModel>.Filter;
            var updateBuilder = Builders<TagDataModel>.Update;

            if (serviceModel.IsValueAssigned)
            {
                var count = await context.Tags.AsQueryable().Where(tag => tag.WorkspaceId == new ObjectId(workspaceId) && tag.Type == type && tag.Values.Any(value => value.Value == serviceModel.Value)).CountAsync();

                if (count > 0)
                {
                    ErrorHelper.ThrowException(MusicKGMessages.ValueAlreadyExistsMessage, HttpStatusCode.BadRequest);
                }
            }

            var filter = filterBuilder.Eq(tag => tag.WorkspaceId, new ObjectId(workspaceId)) & filterBuilder.Eq(tag => tag.Type, type);
            var update = updateBuilder.Set(tag => tag.Type, type);

            if (serviceModel.IsValueAssigned)
            {
                update = update.Set($"{nameof(TagDataModel.Values)}.$[value].{nameof(TagValueDataModel.Value)}", serviceModel.Value);
            }                
            if (serviceModel.IsDescriptionAssigned)
            {
                update = update.Set($"{nameof(TagDataModel.Values)}.$[value].{nameof(TagValueDataModel.Description)}", serviceModel.Description);
            }
            if (serviceModel.IsColorAssigned)
            {
                update = update.Set($"{nameof(TagDataModel.Values)}.$[value].{nameof(TagValueDataModel.Color)}", serviceModel.Color);
            }

            var arrayFilter = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument($"value._id", new ObjectId(valueId))),
            };

            var result = await context.Tags.UpdateOneAsync(filter, update, new UpdateOptions
            {
                ArrayFilters = arrayFilter
            });

            if (result.ModifiedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.NoTagValueCanBeUpdatedMessage, HttpStatusCode.BadRequest);
        }

        private async Task<TagDataModel> CreateTagAsync(string workspaceId, TagTypeEnum type, List<TagValueServiceModel> values)
        {
            var dataModel = new TagDataModel
            {
                Type = type,
                WorkspaceId = new ObjectId(workspaceId),
                Values = values?.Select(value => new TagValueDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Value = value.Value,
                    Description = value.Description,
                    Color = value.Color
                })?.ToList()
            };

            await context.Tags.InsertOneAsync(dataModel);

            return dataModel;
        }

        private async Task<TagDataModel> GetTagAsync(string workspaceId, TagTypeEnum type)
        {
            var result = await context.Tags.AsQueryable().FirstOrDefaultAsync(tag => tag.WorkspaceId == new ObjectId(workspaceId) && tag.Type == type);

            return result;
        }
    }
}
