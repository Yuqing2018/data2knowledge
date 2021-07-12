using MusicKG.WebApi.ClientWrapper;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.DataAccess;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class SyndromeService : ISyndromeService
    {
        private readonly IHondaMongoDbContext context;
        private readonly ILogger<SyndromeService> logger;
        private readonly IHttpClientFactory httpClientFactory; 

        /// <summary>
        /// 不良症状的Service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="ConstantSettings">必须有</param>
        public SyndromeService(IHondaMongoDbContext context, ILogger<SyndromeService> logger, 
            IHttpClientFactory httpClientFactory, ConstantSettings ConstantSettings)
        {
            this.context = context;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ExistsAsync(string syndromeId)
        {
            return await context.Syndromes.AsQueryable().AnyAsync(s => s.Id == new ObjectId(syndromeId));
        }

        public async Task<List<SyndromeServiceModel>> GetDistinctSyndromeAsync(List<string> syndromeIds)
        {
            if (syndromeIds == null || syndromeIds.Count == 0)
                return new List<SyndromeServiceModel>();

            var ids = syndromeIds?.Select(id => new ObjectId(id))?.ToList();

            return await context.Syndromes.AsQueryable()
                .Where(c => ids.Contains(c.Id))
                .Select(c => new SyndromeServiceModel
                {
                    MongoId = c.Id,
                    Name = c.Name,
                    BadGrade = c.BadGrade
                }).Distinct().ToListAsync();
        }

        public async Task<List<SyndromeServiceModel>> GetSameSyndromeAsync(List<string> syndromeId)
        {
            if (syndromeId == null || syndromeId.Count == 0)
                return new List<SyndromeServiceModel>();

            var syndromes = await context.Syndromes.AsQueryable().ToListAsync();

            var syndromeNames = syndromes?.Where(s => syndromeId.Contains(s.Id.ToString()))?.Select(s => s.Name)?.ToHashSet();

            if (syndromeNames == null)
                return new List<SyndromeServiceModel>();

            return syndromes?.Where(s => syndromeNames.Contains(s.Name))
                .Select(s => new SyndromeServiceModel
                {
                    MongoId = s.Id,
                    Name = s.Name,
                    BadGrade = s.BadGrade
                })?.ToList();
        }

        public async Task LinkSyndromeAsync<T>(List<T> data) where T : VehicleFaultListServiceModel
        {
            var syndromeIds = data.Select(d => new ObjectId(d.Syndrome)).ToHashSet();

            var syndromes = await context.Syndromes.AsQueryable()
                .Where(c => syndromeIds.Contains(c.Id)).ToListAsync();

            data.ForEach(d =>
            {
                var syndrome = syndromes.FirstOrDefault(s => s.Id.ToString() == d.Syndrome);
                d.SyndromeModel = new SyndromeServiceModel
                {
                    MongoId = syndrome?.Id ?? ObjectId.Empty,
                    Name = syndrome?.Name ?? ConstantSettings.UnknownString,
                    BadGrade = syndrome?.BadGrade ?? BadGrade.C
                };
            });
        }


        #region syndrome data model CRUD
        public async Task<SyndromeDataModel> UpdateAsync(string id, string name, BadGrade? grade)
        {
            SyndromeDataModel result = null;

            var syndrome = await context.Syndromes.Find(x => x.Id == new ObjectId(id)).FirstOrDefaultAsync();

            if (syndrome == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.HondaSyndromeNotExistMessage, HttpStatusCode.BadRequest);

            try
            {
                var update = Builders<SyndromeDataModel>.Update.Set(u => u.Id, syndrome.Id);

                var needUpdate = false;

                if (!string.IsNullOrWhiteSpace(name) && syndrome.Name != name)
                {
                    update = update.Set(u => u.Name, name);
                    needUpdate = true;
                }
                if (grade.HasValue && syndrome.BadGrade != grade.Value)
                {
                    update = update.Set(u => u.BadGrade, grade);
                    needUpdate = true;
                }

                if (needUpdate)
                {
                    #region 更新 syndrome

                    result = await context.Syndromes.FindOneAndUpdateAsync<SyndromeDataModel>(u => u.Id == syndrome.Id, update, new FindOneAndUpdateOptions<SyndromeDataModel>
                    {
                        ReturnDocument = ReturnDocument.After
                    });

                    #endregion

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        #region 同步更新Category
                     
                        try
                        {
                            await UpdateCategoryAsync(syndrome.Id.ToString(), name);
                        }
                        catch
                        {
                            await context.Syndromes.ReplaceOneAsync(s => s.Id == syndrome.Id, syndrome);
                            throw;
                        }

                        #endregion
                    }
                }
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.HondaSyndromeUpdateFailedMessage;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return result;
        }

        public async Task<SyndromeDataModel> CreateAsync(string name, BadGrade grade)
        {
            var syndrome = new SyndromeDataModel()
            {
                Name = name,
                BadGrade = grade,
                CreatedAt = DateTime.UtcNow
            };
            try
            {
                #region upsert Syndrome
                
                await context.Syndromes.InsertOneAsync(syndrome);

                #endregion

                #region Category 同步更新或者新增
                try
                {
                    await CreateCategoryAsync(syndrome.Id.ToString(), syndrome.Name);
                }
                catch
                {
                    await context.Syndromes.DeleteOneAsync(s => s.Id == syndrome.Id);
                    throw;
                }
                #endregion
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.HondaSyndromeCreateFailedMessage;

                logger?.LogError(e, message);

                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return syndrome;
        }

        public async Task<Tuple<long, IEnumerable<SyndromeServiceModel>>> ListAsync(string keyword, int from, int? size)
        {
            var querable = context.Syndromes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                querable = querable.Where(w => w.Name.Contains(keyword));

            var totalCount = await querable.LongCountAsync();

            querable = querable.OrderByDescending(s => s.CreatedAt);

            if (from > 0)
                querable = querable.Skip(from);

            if (size.HasValue)
                querable = querable.Take(size.Value);

            var syndromeList = await querable.ToListAsync();

            return new Tuple<long, IEnumerable<SyndromeServiceModel>>(totalCount, 
                syndromeList.Select(x=> new SyndromeServiceModel()
                {
                    MongoId = x?.Id ?? ObjectId.Empty,
                    Name = x?.Name ?? ConstantSettings.UnknownString,
                    BadGrade = x?.BadGrade ?? BadGrade.C
                }).ToList());
        }

        #endregion

        private async Task UpdateCategoryAsync(string id, string name)
        {
            var httpClient = httpClientFactory.CreateClient();

            var user = await UserWrapper.LoginAsync(httpClient, ConstantSettings.LabelToolUrl, ConstantSettings.DefaultManagerName, ConstantSettings.DefaultManagerPassword);

            string token = user?.Token;

            try
            {
                await CategoryWrapper.UpdateAsync(httpClient, ConstantSettings.LabelToolUrl, ConstantSettings.DefaultWorkSpaceId, id, name, token);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(token))
                    await UserWrapper.LogoutAsync(httpClient, ConstantSettings.LabelToolUrl, token);
            }
        }

        private async Task CreateCategoryAsync(string id, string name)
        {
            var httpClient = httpClientFactory.CreateClient();

            var user = await UserWrapper.LoginAsync(httpClient, ConstantSettings.LabelToolUrl, ConstantSettings.DefaultManagerName, ConstantSettings.DefaultManagerPassword);

            string token = user?.Token;

            try
            {
                await CategoryWrapper.CreateAsync(httpClient, ConstantSettings.LabelToolUrl, ConstantSettings.DefaultWorkSpaceId, id, name, token);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(token))
                    await UserWrapper.LogoutAsync(httpClient, ConstantSettings.LabelToolUrl, token);
            }
        }
    }
}
