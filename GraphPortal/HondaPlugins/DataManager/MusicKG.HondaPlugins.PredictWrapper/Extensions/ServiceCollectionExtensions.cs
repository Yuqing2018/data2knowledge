using MusicKG.HondaPlugins.PredictWrapper.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MusicKG.HondaPlugins.PredictWrapper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVehicleFaultPredictor(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddConfiguration<VehicleFaultPredictSettings>(configuration)
                .AddScoped<IVehiclePredictor, VehiclePredictor>();
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
