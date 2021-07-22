using MusicKG.HondaPlugins.DataAccess;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class RawVehicleDataService : IRawVehicleDataService
    {
        private readonly IRawDbContext rawDbContext;
        private readonly IIgnoredVehicleService ignoredVehicleService;
        private readonly ILogger<RawVehicleDataService> logger;

        public RawVehicleDataService(IRawDbContext rawDbContext, IIgnoredVehicleService ignoredVehicleService, ILogger<RawVehicleDataService> logger)
        {
            this.rawDbContext = rawDbContext;
            this.ignoredVehicleService = ignoredVehicleService;
            this.logger = logger;
        }

        public async Task<List<string>> ListCarModelsAsync()
        {
            var ignored = await ignoredVehicleService.ListAsync(null);
            
            var carModels = await rawDbContext.RawVehicleData.AsQueryable().Where(v => !ignored.Keys.Contains(v.CarModel))
                .Select(v => v.CarModel).Distinct().OrderBy(m => m).ToListAsync();

            return carModels?.Where(c => !ignored.ContainsKey(c))?.ToList();
        }

        public async Task<List<string>> ListCarTypesAsync(List<string> carModel = null)
        {
            var ignored = await ignoredVehicleService.ListAsync(null);
            var ignoredCarTypes = ignored.Values.SelectMany(i => i).ToHashSet();
            var ignoredCarModels = ignored.Keys.ToList();

            var querable = rawDbContext.RawVehicleData.AsQueryable();

            if (carModel != null && carModel.Count > 0)
                querable = querable.Where(v => carModel.Contains(v.CarModel));

            querable = querable.Where(v => !ignoredCarModels.Contains(v.CarModel) && !ignoredCarTypes.Contains(v.CarType));

            return await querable.Select(v => v.CarType).Distinct().OrderBy(t => t).ToListAsync();
        }

        public async Task<List<string>> ListModelYearsAsync(List<string> carModel = null, List<string> carType = null)
        {
            var ignored = await ignoredVehicleService.ListAsync(null);

            var ignoredCarTypes = ignored.Values.SelectMany(i => i).ToHashSet();
            var ignoredCarModels = ignored.Keys.ToList();

            var querable = rawDbContext.RawVehicleData.AsQueryable();

            if (carModel != null && carModel.Count > 0)
                querable = querable.Where(v => carModel.Contains(v.CarModel));

            if (carType != null && carType.Count > 0)
                querable = querable.Where(v => carType.Contains(v.CarType));

            querable = querable.Where(v => !ignoredCarModels.Contains(v.CarModel) && !ignoredCarTypes.Contains(v.CarType));

            return await querable.Select(v => v.ModelYear).Distinct().OrderBy(y => y).ToListAsync();
        }
    }
}
