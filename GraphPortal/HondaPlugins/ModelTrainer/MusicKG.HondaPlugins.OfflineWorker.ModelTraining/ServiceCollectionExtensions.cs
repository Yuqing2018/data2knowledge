using MusicKG.DataManager.ModelTrainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicKG.HondaPlugins.ModelTrainer;

namespace MusicKG.HondaPlugins.OfflineWorker.ModelTraining
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services.AddHondaModelTrainer(configuration);
        }
    }
}