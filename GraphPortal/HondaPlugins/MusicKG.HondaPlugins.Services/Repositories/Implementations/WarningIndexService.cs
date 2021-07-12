using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class WarningIndexService : IWarningIndexService
    {
        private readonly IHondaMongoDbContext context;
        private readonly ILogger<WarningIndexService> logger;

        /// <summary>
        /// MQIRepository service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public WarningIndexService(
            IHondaMongoDbContext context,
            ILogger<WarningIndexService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<List<WarningIndexDataModel>> GetListAsync(WarningType warningType)
        {
            var results = await context.WarningIndex.AsQueryable().Where(x => x.WarningType == warningType).ToListAsync();
            return results;
        }

        public async Task<List<WarningIndexDataModel>> SaveManyAsync(List<WarningIndexDataModel> datas)
        {
            await context.WarningIndex.InsertManyAsync(datas);
            return datas;
        }
    }
}
