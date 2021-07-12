using MusicKG.HondaPlugins.WarningCalculator;
using MusicKG.HondaPlugins.PredictWrapper.Extensions;
using MusicKG.HondaPlugins.DataManager;
using MusicKG.HondaPlugins.Services;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MusicKG.HondaPlugins.OfflineWorker
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services.AddHondaDataAccess(configuration.GetSection("HondaDataAccess"))
                .AddHondaServices(configuration)
                .AddHondaDataManager(configuration)
                .AddVehicleFaultPredictor(configuration)
                .AddWarningCalculator(configuration);
        }
    }
}