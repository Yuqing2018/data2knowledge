using MusicKG.HondaPlugins.DataAccess.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MusicKG.HondaPlugins.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHondaDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddConfiguration<HondaMongoDbSettings>(configuration)
                .AddConfiguration<MariaRawDataDbSettings>(configuration)
                .AddConfiguration<MongoRawDataDbSettings>(configuration)
                .AddConfiguration<ConstantSettings>(configuration)                
                .AddSingleton<IHondaMongoDbContext, HondaMongoDbContext>()
                .AddDbContext<IRawDbContext, RawDbContext>((provider, optionsBuilder) =>
                {
                    var settings = provider.GetRequiredService<MariaRawDataDbSettings>();
                    optionsBuilder.UseMySql(settings.ConnectionString);
                }, ServiceLifetime.Transient);
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
