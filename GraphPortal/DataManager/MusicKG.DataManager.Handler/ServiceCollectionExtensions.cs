using MusicKG.DataManager.Translator.Actions;
using MusicKG.DataManager.Translator.Executors;
using MusicKG.DataManager.Handler.Actions;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace MusicKG.DataManager.Translator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataHandler(this IServiceCollection services, IConfiguration configuration, Func<IServiceCollection, IConfigurationSection, IServiceCollection> customerExecutorRegister)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, CollectDocumentAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, HandleDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, UpdateTaskStatusAction>());

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultDocumentCollector>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultDataHandler>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultTaskStatusUpdater>());

            var section = configuration.GetSection("DataHandler");

            return customerExecutorRegister == null ? services : customerExecutorRegister.Invoke(services, section);
        }
    }
}