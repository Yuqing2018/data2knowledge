using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.Services.Repositories.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MusicKG.HondaPlugins.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHondaServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<ISyndromeService, SyndromeService>()
                .AddScoped<IVehicleFaultStatisticsService, VehicleFaultStatisticsService>()
                .AddScoped<IVehicleFaultDataService, VehicleFaultDataService>()
                .AddScoped<IWarningTaskService, WarningTaskService>()
                .AddScoped<IWarningIndexService, WarningIndexService>()
                .AddScoped<IWarningTaskDetailService, WarningTaskDetailService>()
                .AddScoped<IWarningRecordService, WarningRecordService>()
                .AddScoped<IDataReannotationTaskService, DataReannotationTaskService>()
                .AddScoped<IRawSalesService, RawSalesService>()
                .AddScoped<IRawQISService, RawQISService>()
                .AddScoped<IRawVehicleDataService, RawVehicleDataService>()
                .AddScoped<IIgnoredVehicleService, IgnoredVehicleService>()
                .AddScoped<IOverallMetricsService, OverallMetricsService>();
        }

        public static IServiceCollection AddConfiguration<TConfig>(this IServiceCollection service, IConfiguration configuration) where TConfig : class
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
