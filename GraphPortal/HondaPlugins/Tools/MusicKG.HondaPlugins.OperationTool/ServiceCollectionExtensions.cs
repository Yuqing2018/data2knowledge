using MusicKG.HondaPlugins.OperationTool.Executors;
using MusicKG.HondaPlugins.OperationTool.Settings;
using MusicKG.DataManager.ModelTrainer;
using MusicKG.DataManager.ModelTrainer.Executors;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace MusicKG.HondaPlugins.OperationTool
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHondaModelTrainer(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddModelTrainer(configuration, (services, configuration) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, ClassificationModelTrainingDataCollector>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, RiskModelTrainingDataCollector>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, ClassificationModelTrainer>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, RiskModelTrainer>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, ClassificationModelServer>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, RiskModelServer>());
                services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaTrainingHistoryUpdater>());

                return services.AddConfiguration<HondaClassificationModelTrainingSettings>(configuration)
                    .AddConfiguration<HondaRiskModelTrainingSettings>(configuration);
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