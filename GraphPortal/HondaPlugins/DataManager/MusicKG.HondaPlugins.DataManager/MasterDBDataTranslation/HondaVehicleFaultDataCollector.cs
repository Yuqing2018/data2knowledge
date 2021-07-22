using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaVehicleFaultDataCollector : DataCollector
    {
        private readonly IHondaMongoDbContext dbContext;

        public HondaVehicleFaultDataCollector(IHondaMongoDbContext dbContext, ILogger<HondaVehicleFaultDataCollector> logger) : base(logger)
        {
            this.dbContext = dbContext;
            ExecutorType = HondaExecutors.HondaVehicleFaultDataCollector.ToString();
        }

        protected override IEnumerable<List<Dictionary<string, object>>> CollectData(string actionId, 
            DataTranslatorContext context, 
            DataCollectionOptions options, 
            DateTime start, DateTime end)
        {
            var queryCount = (int)Math.Ceiling((decimal)context.DataCount / options.BatchSize);

            return Enumerable.Range(0, queryCount).Select(index =>
            {
                var result = dbContext.VehicleFault.AsQueryable()
                    .Where(d => d.FaultDate >= start && d.FaultDate <= end)
                    .Skip(index * options.BatchSize).Take(options.BatchSize).ToList();

                return result.Select(r =>
                {
                    r.Features.Add(nameof(VehicleFaultDataModel.DataSource), r.DataSource.ToString());
                    r.Features.Add(nameof(VehicleFaultDataModel.RawId), r.RawId);
                    return r.Features.ToDictionary(k => k.Key, v => v.Value as object);
                }).ToList();
            });
        }

        protected async override Task<long> CountDataAsync(string actionId, DataTranslatorContext context, DataCollectionOptions options, DateTime start, DateTime end)
        {
            var filter = GetFilter(start, end);

            return await dbContext.VehicleFault.CountDocumentsAsync(filter);
        }

        private FilterDefinition<VehicleFaultDataModel> GetFilter(DateTime start, DateTime end)
        {
            var builder = Builders<VehicleFaultDataModel>.Filter;

            return builder.And(builder.Gte(d => d.Timestamp, start), builder.Lt(d => d.Timestamp, end));
        }
    }
}
