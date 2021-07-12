using MusicKG.HondaPlugins.WarningCalculator.Actions;
using MusicKG.HondaPlugins.WarningCalculator.Executors;
using MusicKG.HondaPlugins.WarningCalculator.Services;
using MusicKG.HondaPlugins.WarningCalculator.Settings;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace MusicKG.HondaPlugins.WarningCalculator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWarningCalculator(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, CollectVehicleFaultDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, LinkRelatedDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, CalculateWarningAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, SaveCalculationResultAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, AnnounceWarningAction>());

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, VehicleFaultDataCollector>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, RelatedDataLinker>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, WarningRecordCalculator>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, CalculationResultSaver>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, WarningAnnouncer>());

            return services.AddConfiguration<RiskWarningModelSettings>(configuration)
                .AddScoped<IWarningTaskCalculationService, WarningTaskCalculationService>()
                .AddScoped<IRiskWarningAIService, RiskWarningAIService>();
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
