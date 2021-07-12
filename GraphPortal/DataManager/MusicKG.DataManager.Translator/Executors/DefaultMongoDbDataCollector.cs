using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Settings;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicKG.DataManager.Translator.Executors
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public class DefaultMongoDbDataCollector : DataCollector
    {
        private readonly DefaultDataCollectorSettings settings;

        public DefaultMongoDbDataCollector(DefaultDataCollectorSettings settings, ILogger<DefaultMongoDbDataCollector> logger) : base(logger)
        {
            this.settings = settings;
            ExecutorType = DataTranslationDefaultExecutors.DefaultMongoDataCollector.ToString();
        }

        protected override IEnumerable<List<Dictionary<string, object>>> CollectData(string actionId, 
            DataTranslatorContext context, DataCollectionOptions options, 
            DateTime start, DateTime end)
        {
            var dbSettings = context.Parameters.DBSettings ?? settings.Settings[context.Parameters.DataSourceName];
  
            var collection = GetCollection(dbSettings);

            var queryCount = (int)Math.Ceiling((decimal)context.DataCount / options.BatchSize);

            return Enumerable.Range(0, queryCount).Select(index =>
            {
                var result = collection.AsQueryable()
                    .Where(d => d[dbSettings.TimestampFieldName] >= start && d[dbSettings.TimestampFieldName] < end)
                    .Skip(index * options.BatchSize).Take(options.BatchSize).ToList();

                return result?.Select(r => r.ToDictionary()).ToList();
            });
        }

        protected async override Task<long> CountDataAsync(string actionId, DataTranslatorContext context, DataCollectionOptions options, DateTime start, DateTime end)
        {
            var dbSettings = context.Parameters.DBSettings ?? settings.Settings[context.Parameters.DataSourceName];

            var collection = GetCollection(dbSettings);

            var filter = GetFilter(dbSettings, start, end);

            var count = await collection.CountDocumentsAsync(filter);

            return count;
        }

        private FilterDefinition<BsonDocument> GetFilter(DefaultDbSetting dbSettings,DateTime start, DateTime end)
        {
            var builder = Builders<BsonDocument>.Filter;

            return builder.And(builder.Gte(d => d[dbSettings.TimestampFieldName], start), builder.Lt(d => d[dbSettings.TimestampFieldName], end));
        }

        private IMongoCollection<BsonDocument> GetCollection(DefaultDbSetting dbSettings)
        {
            var mongoSettings = MongoClientSettings.FromConnectionString(dbSettings.ConnectionString);

            mongoSettings.MaxConnectionIdleTime = TimeSpan.FromSeconds(int.MaxValue);

            var client = new MongoClient(mongoSettings);

            var database = client.GetDatabase(dbSettings.Database);

            return database.GetCollection<BsonDocument>(dbSettings.TableName);

        }
    }
}
