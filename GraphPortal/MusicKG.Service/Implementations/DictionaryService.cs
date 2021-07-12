using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Resources;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.Models;
using System.Diagnostics;

namespace MusicKG.Service.Implementations
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<DictionaryService> logger;

        /// <summary>
        /// Dictionary service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public DictionaryService(
            IMusicKGContext context,
            ILogger<DictionaryService> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        
        public async Task<string> GetDictionaryIdByNameAsync(string workspaceId, string name)
        {
            var dictionary = await context.Dictionaries.Find(w => !w.IsDeleted && w.WorkspaceId == new ObjectId(workspaceId) && w.Name == name).FirstOrDefaultAsync();

            if (dictionary == null)
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryNotExistMessage, HttpStatusCode.BadRequest);

            return dictionary.Id.ToString();
        }

        /// <summary>
        /// Get dictionary by dictionary ID.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="id">Dictionary ID.</param>
        /// <returns>Dictionary service object.</returns>
        public async Task<DictionaryServiceModel> GetDictionaryAsync(string workspaceId, string id)
        {
            var dictionary = await context.Dictionaries.Find(w => !w.IsDeleted && w.WorkspaceId == new ObjectId(workspaceId) && w.Id == new ObjectId(id)).FirstOrDefaultAsync();

            if (dictionary == null)
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryNotExistMessage, HttpStatusCode.BadRequest);

            return DictionaryDataModelToServiceModel(dictionary);
        }

        /// <summary>
        /// Convert dictionary data model to service model.
        /// </summary>
        /// <param name="dataModel">Data object.</param>
        /// <returns>Dictionary service object.</returns>
        private DictionaryServiceModel DictionaryDataModelToServiceModel(DictionaryDataModel dataModel)
        {
            return new DictionaryServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                EntityId = dataModel.EntityId.ToString(),
                CreatedAt = dataModel.CreatedAt,
                CreatedBy = this.context.Users.Find(u => u.Id == dataModel.CreatedBy).First().Name,
                Vocabularies = dataModel.Vocabularies,
            };
        }

        private DictionaryListItemServiceModel DictionaryDataModelToListItemServiceModel(DictionaryDataModel dataModel)
        {
            return new DictionaryListItemServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                EntityId = dataModel.EntityId.ToString(),
                EntriesCount = dataModel.Vocabularies.Count()
            };
        }
        /// <summary>
        /// Create a Dictionary.
        /// </summary>
        /// <param name="serviceModel">Dictionary create service object.</param>
        /// <returns>Dictionary service object.</returns>
        public async Task<DictionaryServiceModel> CreateDictionaryAsync(DictionaryCreateServiceModel serviceModel)
        {
            var count = await context.Dictionaries.CountDocumentsAsync(u => u.WorkspaceId == new ObjectId(serviceModel.WorkspaceId) && u.Name == serviceModel.Name);

            if (count > 0)
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryNameExistMessage, HttpStatusCode.BadRequest);

            var dictionary = new DictionaryDataModel
            {
                WorkspaceId = new ObjectId(serviceModel.WorkspaceId),
                Name = serviceModel.Name,
                Vocabularies = serviceModel.Vocabularies,
                CreatedBy = new ObjectId(serviceModel.CreatedBy),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            if (!string.IsNullOrEmpty(serviceModel.EntityId))
            {
                dictionary.EntityId = new ObjectId(serviceModel.EntityId);
            }

            try
            {
                await context.Dictionaries.InsertOneAsync(dictionary);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.DictionaryCreateFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = MusicKGMessages.DictionaryNameExistMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }


            return DictionaryDataModelToServiceModel(dictionary);
        }

        /// <summary>
        /// Get dictionaries by a workspace id.
        /// </summary>
        /// <param name="workspaceId">Workspace id.</param>
        /// <param name="from"></param>
        /// <param name="size">The size you want to get.</param>
        /// <returns>Tuple object</returns>
        public async Task<Tuple<long, IEnumerable<DictionaryListItemServiceModel>>> GetDictionariesAsync(string workspaceId, int from, int? size, bool topLatest = true)
        {
            var builder = Builders<DictionaryDataModel>.Filter;

            var filter = builder.Eq(x => x.IsDeleted, false);

            if (!String.IsNullOrEmpty(workspaceId))
            {
                filter &= builder.Eq(x => x.WorkspaceId, new ObjectId(workspaceId));
            }

            var dictionariesFind = context.Dictionaries.Find(filter);

            if (topLatest)
            {
                dictionariesFind.Sort(Builders<DictionaryDataModel>.Sort.Descending(x => x.CreatedAt));
            }
            else
            {
                dictionariesFind.Sort(Builders<DictionaryDataModel>.Sort.Ascending(x => x.CreatedAt));
            }

            var totalCount = await dictionariesFind.CountDocumentsAsync();

            if (from > 0)
                dictionariesFind = dictionariesFind.Skip(from);

            if (size.HasValue)
                dictionariesFind = dictionariesFind.Limit(size.Value);


            var dictionaries = await dictionariesFind.ToListAsync();

            var dictionaryServiceModels = dictionaries.Select(u => DictionaryDataModelToListItemServiceModel(u));

            return new Tuple<long, IEnumerable<DictionaryListItemServiceModel>>(totalCount, dictionaryServiceModels);
        }

        public async Task<IEnumerable<string>> GetAllDictionaryEntriesAsync(string workspaceId)
        {
            return await context.Dictionaries.AsQueryable()
                .Where(dic => dic.WorkspaceId == new ObjectId(workspaceId))
                .SelectMany(dic => dic.Vocabularies).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get dictionary entries by workspace id, dictionary id  and filter string
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="dictionaryId"></param>
        /// <param name="filterStr"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<Tuple<long, IEnumerable<string>>> GetDictionaryEntriesAsync(string workspaceId, string dictionaryId, string filterStr, int from, int? size)
        {
            IEnumerable<string> dictionaryEntries = null;

            var dictionary = await context.Dictionaries.Find(w => !w.IsDeleted && w.WorkspaceId == new ObjectId(workspaceId) && w.Id == new ObjectId(dictionaryId)).FirstOrDefaultAsync();

            if (dictionary == null)
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryNotExistMessage, HttpStatusCode.BadRequest);

            dictionaryEntries = dictionary.Vocabularies;

            if (!string.IsNullOrEmpty(filterStr))
                dictionaryEntries = dictionaryEntries.Where(x =>!string.IsNullOrWhiteSpace(x) && x.Contains(filterStr, StringComparison.OrdinalIgnoreCase));

            var totalCount = dictionaryEntries.Count();
            var entries = dictionaryEntries.Skip(from).Take(size ?? int.MaxValue);

            return new Tuple<long, IEnumerable<string>>(totalCount, entries);
        }

        /// <summary>
        /// Update dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary ID.</param>
        /// <param name="serviceModel">Service object.</param>
        /// <returns>Dictionary service object.</returns>
        public async Task<DictionaryServiceModel> UpdateDictionaryAsync(string workspaceId, string dictionaryId, DictionaryUpdateServiceModel serviceModel)
        {
            var dictionary = await context.Dictionaries.Find(u => !u.IsDeleted && u.WorkspaceId == new ObjectId(workspaceId) && u.Id == new ObjectId(dictionaryId)).FirstOrDefaultAsync();

            if (dictionary == null)
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryNotExistMessage, HttpStatusCode.BadRequest);

            var update = Builders<DictionaryDataModel>.Update.Set(u => u.Id, dictionary.Id);

            if (serviceModel.IsNameAssigned)
                update = update.Set(u => u.Name, serviceModel.Name);

            if (serviceModel.IsEntityIdAssigned)
                update = update.Set(u => u.EntityId, new ObjectId(serviceModel.EntityId));

            if (serviceModel.IsVocabulariesAssigned)
            {
                if (serviceModel.Vocabularies.Count() == 0)
                    update = update.Unset(u => u.Vocabularies);
                else
                    update = update.Set(u => u.Vocabularies, serviceModel.Vocabularies);
            }

            try
            {
                dictionary = await context.Dictionaries.FindOneAndUpdateAsync<DictionaryDataModel>(u => !u.IsDeleted && u.Id == dictionary.Id && u.WorkspaceId == new ObjectId(workspaceId), update,
                new FindOneAndUpdateOptions<DictionaryDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGMessages.DictionaryUpdateFailedMessage;

                if ("DuplicateKey".Equals(e.CodeName))
                    message = MusicKGMessages.DictionaryNameExistMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return DictionaryDataModelToServiceModel(dictionary);
        }

        /// <summary>
        /// Delete dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary ID.</param>
        /// <returns>null</returns>
        public async Task DeleteDictionaryAsync(string workspaceId, string dictionaryId)
        {
            var dictionary = await context.Dictionaries.Find(u => !u.IsDeleted && u.WorkspaceId == new ObjectId(workspaceId) && u.Id == new ObjectId(dictionaryId)).FirstOrDefaultAsync();

            if (dictionary == null)
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryNotExistMessage, HttpStatusCode.BadRequest);

            var update = Builders<DictionaryDataModel>.Update
                .Set(u => u.IsDeleted, true)
                .Set(u => u.Name, dictionary.Name + DateTime.UtcNow)
                .Set(t => t.DeletedAt, DateTime.UtcNow);

            try
            {
                var result = await context.Dictionaries.UpdateOneAsync(u => u.WorkspaceId == new ObjectId(workspaceId) && u.Id == dictionary.Id && !u.IsDeleted, update);

                if (result.MatchedCount == 0)
                    ErrorHelper.ThrowException(MusicKGMessages.DictionaryNotExistMessage, HttpStatusCode.BadRequest);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.DictionaryDeleteFailedMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task UpdateOneVocabsAsync(string workspaceId, string Id, List<string> vocabs)
        {
            var dictionary = await context.Dictionaries.Find(u => !u.IsDeleted && u.WorkspaceId == new ObjectId(workspaceId) && u.Id == new ObjectId(Id)).FirstOrDefaultAsync();
            var update = Builders<DictionaryDataModel>.Update.Set(u => u.Id, dictionary.Id);
            update = update.AddToSetEach(u => u.Vocabularies, vocabs);

            try
            {
                dictionary = await context.Dictionaries.FindOneAndUpdateAsync<DictionaryDataModel>(u => !u.IsDeleted && u.Id == dictionary.Id && u.WorkspaceId == new ObjectId(workspaceId), update,
                new FindOneAndUpdateOptions<DictionaryDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGMessages.DictionaryUpdateFailedMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }
    }
}
