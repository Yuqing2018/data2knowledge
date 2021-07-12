using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MusicKG.Scheduler.Example
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services, IConfiguration pluginConfiguraiton)
        {
            return services;
        }
    }
}
