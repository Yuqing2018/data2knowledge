using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Tasks;
using MusicKG.DataManager.Translator.Settings;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.DataManager.Translator.Executors
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public class DefaultMySqlDataCollector : DataCollector
    {
        private readonly DefaultDataCollectorSettings settings;

        public DefaultMySqlDataCollector(DefaultDataCollectorSettings settings, ILogger<DefaultMySqlDataCollector> logger) : base(logger)
        {
            this.settings = settings;
            ExecutorType = DataTranslationDefaultExecutors.DefaultMySqlDataCollector.ToString();
        }

        protected override IEnumerable<List<Dictionary<string, object>>> CollectData(string actionId, 
            DataTranslatorContext context, DataCollectionOptions options, 
            DateTime start, DateTime end)
        {
            var dbSettings = context.Parameters.DBSettings ?? settings.Settings[context.Parameters.DataSourceName];

            var queryCount = (int)Math.Ceiling((decimal)context.DataCount / options.BatchSize);

            for (int index = 0; index < queryCount; index++)
            {
                var query = ParseQuery(dbSettings, start, end, index * options.BatchSize, options.BatchSize);

                var result = ExecuteQuery(dbSettings.ConnectionString, query);

                yield return result;
            }
        }

        private List<Dictionary<string, object>> ExecuteQuery(string connectionString, string query)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var com = new MySqlCommand(query, connection))
                {
                    using (var reader = com.ExecuteReader())
                    {
                        var fieldCount = reader.FieldCount;
                        while (reader.Read())
                        {
                            var itemDict = new Dictionary<string, object>();
                            for (int i = 0; i < fieldCount; i++)
                            {
                                var value = reader.GetValue(i);

                                if (value is string)
                                    value = value?.ToString()?.Trim();

                                itemDict.Add(reader.GetName(i), value);
                            }
                            if (itemDict.Count > 0)
                            {
                                result.Add(itemDict);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected async override Task<long> CountDataAsync(string actionId, DataTranslatorContext context, DataCollectionOptions options, DateTime start, DateTime end)
        {
            var dbSettings = context.Parameters.DBSettings ?? settings.Settings[context.Parameters.DataSourceName];

            var sqlCountQuery = ParseCountQuery(dbSettings, start, end);

            using (var con = new MySqlConnection(dbSettings.ConnectionString))
            {
                con.Open();

                using (var comCount = new MySqlCommand(sqlCountQuery, con))
                {
                    var count = await comCount.ExecuteScalarAsync();

                    return count == null ? 0 : (long)count;
                }
            }
        }

        private string ParseQuery(DefaultDbSetting settings, DateTime start, DateTime end, int skip, int limit)
        {
            return $"SELECT * FROM {settings.TableName} WHERE {settings.TimestampFieldName} >= '{start.ToString("yyyy-MM-dd HH:mm:ss")}' && {settings.TimestampFieldName} < '{end.ToString("yyyy-MM-dd HH:mm:ss")}' LIMIT {skip},{limit}";
        }

        private string ParseCountQuery(DefaultDbSetting settings, DateTime start, DateTime end)
        {
            return $"SELECT COUNT(*) FROM {settings.TableName} WHERE {settings.TimestampFieldName} >= '{start.ToString("yyyy-MM-dd HH:mm:ss")}' && {settings.TimestampFieldName} < '{end.ToString("yyyy-MM-dd HH:mm:ss")}'";
        }
    }
}
