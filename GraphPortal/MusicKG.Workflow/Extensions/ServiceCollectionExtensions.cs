using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Settings;
using MusicKG.Service;
using MusicKG.Service.Implementations;
using MusicKG.Service.Settings;
using MusicKG.Workflow.Services;
using MusicKG.Workflow.Services.Implementations;
using MusicKG.Workflow.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.AddSingleton(configuration);
            services.AddSingleton<IMusicKGContext, MusicKGContext>();
            services.AddSingleton<IWorkflowService, WorkflowService>();
            services.AddSingleton<IStorageService, S3StorageService>();

            services.AddSingleton<IProcessorFactory, ProcessorFactory>();
            services.AddSingleton<IWorkflowProcessService, WorkflowProcessService>();
            services.AddSingleton<IWorkflowStepProcessService, WorkflowStepProcessService>();
            services.AddSingleton<IWorkflowDocumentService, WorkflowDocumentService>();
            services.AddSingleton<IWorkflowDaemon, WorkflowDaemon>();

            return services;
        }

        public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WorkflowStepServiceSettings>(options => options.ParseFromConfiguration(configuration));
            services.Configure<MongoDBSettings>(options => options.ParseFrom(configuration));
            services.Configure<S3Settings>(options => options.ParseFrom(configuration));
            services.Configure<SupportedWorkflowSettings>(options => options.ParseFromParseFromConfiguration(configuration));

            return services;
        }
    }
}
