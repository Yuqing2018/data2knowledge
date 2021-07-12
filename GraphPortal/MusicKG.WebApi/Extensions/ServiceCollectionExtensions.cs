using MusicKG.DataAccess;
using MusicKG.DataAccess.Settings;
using MusicKG.Service;
using MusicKG.Service.Enums;
using MusicKG.Service.Implementations;
using MusicKG.Service.Implementations.OntologyExport;
using MusicKG.Service.Settings;
using MusicKG.Service.SynchronizedWorkflow;
using MusicKG.Service.SynchronizedWorkflow.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MusicKG.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register application settings and services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Service collections.</returns>
        public static IServiceCollection RegisterSettingsAndServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(options => options.ParseFrom(configuration));
            services.Configure<UserSettings>(options => options.ParseFrom(configuration));
            services.Configure<S3Settings>(options => options.ParseFrom(configuration));

            services.AddHttpClient();

            services.AddSingleton(configuration);
            services.AddSingleton<IMusicKGContext, MusicKGContext>();

            var storageTypeSettings = new StorageTypeSettings(configuration);
            if (storageTypeSettings.GridFS)
                services.AddSingleton<IStorageService, GridFSStorageService>();
            else if (storageTypeSettings.S3)
                services.AddSingleton<IStorageService, S3StorageService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
            services.AddScoped<IOptionService, OptionService>();
            services.AddScoped<IRuleService, RuleService>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IWorkspaceTypeService, WorkspaceTypeService>();
            services.AddScoped<IWorkflowStepService, WorkflowStepService>();
            services.AddScoped<IOntologyService, OntologyService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IDialogOntologyService, DialogOntologyService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<ISyncWorkflowStepProcessService, SyncWorkflowStepProcessService>();
            services.AddScoped<ISyncProcessorFactory, SyncProcessorFactory>();
            services.AddScoped<ITaskCreationRuleService, TaskCreationRuleService>();
            services.AddScoped<OntologyJsonExporter>();
            services.AddScoped<OntologyHugeGraphSchemaExporter>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped(provider =>
            {
                Func<OntologyDownloadFileTypeEnum, IOntologyExportProvider> func = (resultType) =>
                {
                    switch (resultType)
                    {
                        case OntologyDownloadFileTypeEnum.Json:
                        default:
                            return provider.GetService<OntologyJsonExporter>();
                        case OntologyDownloadFileTypeEnum.HugeGraphSchema:
                            return provider.GetService<OntologyHugeGraphSchemaExporter>();
                    }
                };
                return func;
            });

            return services;
        }
    }
}
