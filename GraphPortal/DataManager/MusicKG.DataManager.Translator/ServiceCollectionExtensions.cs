using MusicKG.DataManager.Translator.Actions;
using MusicKG.DataManager.Translator.Executors;
using MusicKG.DataManager.Translator.Settings;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace MusicKG.DataManager.Translator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataTranslator(this IServiceCollection services, IConfiguration configuration, 
            Func<IServiceCollection, IConfigurationSection, IServiceCollection> customerExecutorRegister)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, CollectDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, NormalizeDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, AnnotateDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, ConsumeDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, CreateTaskAction>());

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultMongoDbDataCollector>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultMySqlDataCollector>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultDataNormalizer>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultDataAnnotator>()); 
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultLabelingToolDataConsumer>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultDataConsumer>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IActionExecutor, DefaultTaskCreator>());

            var section = configuration.GetSection("DataTranslator");

            services = services.AddConfiguration<DefaultDataCollectorSettings>(section);

            return customerExecutorRegister == null ? services : customerExecutorRegister.Invoke(services, section);
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
