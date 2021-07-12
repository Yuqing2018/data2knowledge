using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.Service.Implementations
{
    public class CategoryService: ICategoryService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<CategoryService> logger;

        /// <summary>
        /// Category service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public CategoryService(
            IMusicKGContext context,
            ILogger<CategoryService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<CategoryDataModel> GetOneAsync(string workspaceId, string categoryId)
        {
            var data = await context.Categories.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Id == new ObjectId(categoryId)).FirstOrDefaultAsync();
            return data;
        }
        public async Task<List<CategoryDataModel>> GetAllAsync(string workspaceId)
        {
            return await this.context.Categories.Find(x => x.WorkspaceId == new ObjectId(workspaceId)).ToListAsync();
        }

        public async Task<Tuple<long, IEnumerable<CategoryServiceModel>>> GetCategoriesAsync(string workspaceId, string keyword, int from, int? size)
        {
            var builder = Builders<CategoryDataModel>.Filter;

            var filter = builder.Eq(x => x.WorkspaceId, new ObjectId(workspaceId));

            if (!string.IsNullOrEmpty(keyword))
            {
                filter &= builder.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
            }

            var categoryFilter = this.context.Categories.Find(filter);
            
            var totalCount = await categoryFilter.CountDocumentsAsync();

            if (from > 0)
                categoryFilter = categoryFilter.Skip(from);

            if (size.HasValue)
                categoryFilter = categoryFilter.Limit(size.Value);


            var categories = await categoryFilter.ToListAsync();

            var categoryServiceModels = categories.Select(u => CategoryDataModelToServiceModel(u));

            return new Tuple<long, IEnumerable<CategoryServiceModel>>(totalCount, categoryServiceModels);
        }

        /// <summary>
        /// get category dictionary by workspaceId.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <returns>Dictionary<string, string>: key:Name, Value:OrderId</returns>
        public async Task<Dictionary<string, string>> GetCategoryDictAsync(string workspaceId)
        {
            var categoryList = await this.context.Categories.Find(x => x.WorkspaceId == new ObjectId(workspaceId)).ToListAsync();
            var categoryDict = categoryList.GroupBy(x => x.Id).ToDictionary(k => k.Key.ToString(), v => v.FirstOrDefault(x => x.Id == v.Key).Name.ToString());
            return categoryDict;
        }

        public async Task<CategoryServiceModel> UpdateAsync(string workspaceId, string categoryId, string name)
        {
            var category = await context.Categories.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Id == new ObjectId(categoryId)).FirstOrDefaultAsync();

            if (category == null)
                ErrorHelper.ThrowException(MusicKGMessages.HondaSyndromeNotExistMessage, HttpStatusCode.BadRequest);

            var update = Builders<CategoryDataModel>.Update.Set(u => u.Id, category.Id);

            if (category.Name != name)
                update = update.Set(u => u.Name, name);

            try
            {
                category = await context.Categories.FindOneAndUpdateAsync<CategoryDataModel>(u => u.Id == category.Id, update,
                new FindOneAndUpdateOptions<CategoryDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGMessages.HondaSyndromeUpdateFailedMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return CategoryDataModelToServiceModel(category);
        }

        public async Task<bool> CreateManyAsync(string workspaceId, string createdBy, List<string> names)
        {
            var isValidWorkspace = context.Workspaces.AsQueryable().Any(x => x.Id == new ObjectId(workspaceId));
            if (!isValidWorkspace)
            {
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceNotExistMessage, HttpStatusCode.BadRequest);
            }

            var categories = names.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Select(name => new CategoryDataModel()
            {
                WorkspaceId = new ObjectId(workspaceId),
                Name = name,
                CreatedBy = new ObjectId(createdBy),
                CreatedAt = DateTime.UtcNow
            });

            try
            {
               await  context.Categories.InsertManyAsync(categories);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.HondaSyndromeCreateFailedMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return true;
        }

        public async Task<CategoryServiceModel> CreateOneAsync(CategoryServiceModel serviceModel)
        {
            var isValidWorkspace = context.Workspaces.AsQueryable().Any(x => x.Id == new ObjectId(serviceModel.WorkspaceId));
            if (!isValidWorkspace)
            {
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceNotExistMessage, HttpStatusCode.BadRequest);
            }

            var category = new CategoryDataModel()
            {
                WorkspaceId = new ObjectId(serviceModel.WorkspaceId),
                Id = string.IsNullOrWhiteSpace(serviceModel.Id) ? ObjectId.GenerateNewId() : new ObjectId(serviceModel.Id),
                Name = serviceModel.Name,
                CreatedBy = new ObjectId(serviceModel.CreatedBy),
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await context.Categories.InsertOneAsync(category);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.HondaSyndromeCreateFailedMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return serviceModel;
        }

        private CategoryServiceModel CategoryDataModelToServiceModel(CategoryDataModel dataModel)
        {
            return new CategoryServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,                
                CreatedAt = dataModel.CreatedAt,
                CreatedBy = this.context.Users.Find(u => u.Id == dataModel.CreatedBy).First().Name
            };
        }
    }
}
