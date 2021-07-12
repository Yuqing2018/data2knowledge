using MusicKG.HondaPlugins.DataManager.Executors;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.DataManager.Translator;
using MusicKG.DataManager.Models.Settings;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace MusicKG.HondaPlugins.DataManager
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHondaDataManager(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddConfiguration<LabelingServiceSettings>(configuration)
                .AddHondaDataHandler(configuration)
                .AddHondaDataTranslator(configuration);
        }

        public static IServiceCollection AddHondaDataTranslator(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services.AddDataTranslator(configuration, HondaTranslatorExecutorRegister);
        }

        public static IServiceCollection AddHondaDataHandler(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services.AddDataHandler(configuration, HondaDataHandlerExecutorRegister);
        }

        private static IServiceCollection HondaDataHandlerExecutorRegister(IServiceCollection services, 
            IConfigurationSection configurationSection)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaDocumentCollector>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaDataHandler>());

            return services;
        }

        private static IServiceCollection HondaTranslatorExecutorRegister(IServiceCollection services, 
            IConfigurationSection configurationSection)
        {
            #region Data Collectors

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaVehicleFaultDataCollector>());

            #endregion

            #region Data Normalizers

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaDataNormalizer>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaVehicleFaultDataNormalizer>());

            #endregion

            #region Data Annotators

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaDataAnnotator>());

            #endregion

            #region Data Consumers

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaLabelingToolDataConsumer>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaBusinessDataConsumer>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, HondaVehicleFaultDataConsumer>());

            #endregion

            return services.AddConfiguration<VehicleDataConstructorSettings>(configurationSection);
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
