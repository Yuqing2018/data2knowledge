using MusicKG.DataManager.ModelTrainer;
using MusicKG.DataManager.ModelTrainer.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.DataManager.ModelTrainer.Executors;

namespace MusicKG.DataManager.ModelTrainingWorker
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services.AddModelTrainer(configuration, (services, configuration) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultTrainingDataCollector>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultModelTrainer>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultModelServer>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultTrainingHistoryUpdater>());

                return services.AddConfiguration<DefaultModelTrainerDbSettings>(configuration)
                    .AddConfiguration<ModelTrainingSettings>(configuration);
            });
        }

        private static IServiceCollection AddConfiguration<TConfig>(this IServiceCollection service, IConfiguration configuration) where TConfig : class
        {
            return service.AddSingleton(provider =>
            {
                var setting = Activator.CreateInstance<TConfig>();

                configuration.GetSection(typeof(TConfig).Name).Bind(setting);

                return setting;
            });
        }
    }
}