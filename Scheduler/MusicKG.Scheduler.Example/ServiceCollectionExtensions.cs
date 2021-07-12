using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Example.Actions;
using MusicKG.Scheduler.Example.Executors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MusicKG.Scheduler.Example
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExample(this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, ExampleAction1>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, ExampleAction2>());

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, ExampleExecutor1>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, ExampleExecutor2>());

            return services.AddScoped<IExampleScopedService, ExampleScopedService>();
        }
    }
}
