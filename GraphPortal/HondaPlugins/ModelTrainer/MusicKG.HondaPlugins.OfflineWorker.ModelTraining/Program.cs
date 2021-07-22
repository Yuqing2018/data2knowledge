using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.Scheduler.Engine;
using MusicKG.Scheduler.Extensions;
using MusicKG.Scheduler.Service;
using Serilog;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.OfflineWorker.ModelTraining
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var jobName = args[0];

            var host = CreateHostBuilder(args).Build();

            var jobService = host.Services.GetRequiredService<IJobService>();

            var taskService = host.Services.GetRequiredService<IJobTaskService>();

            var taskEngine = host.Services.GetRequiredService<IJobTaskExecutor>();

            var jobServiceModel = await jobService.GetByNameAsync(jobName);

            var taskServiceModel = await taskService.ListPreparedAsync(jobServiceModel.Id, 3);

            

            //engine.ExecuteAsync(new Scheduler.Service.Models.JobServiceModel
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Description = "Train model.",
            //    JobType = HondaJobTypes.ModelTraining.ToString(),
            //    LastRunAt = null,
            //    Name = "ModelTrainer",
            //    Schedule = null,
            //    Actions = new List<JobActionServiceModel>
            //    {
            //        new JobActionServiceModel
            //        {
            //            ActionId = ModelTrainingActions.TrainingDataCollection.ToString(),
            //            Description = "Collect model training data.",
            //            IsDefault = true,
            //            Options = new TrainingDataCollectionOptions
            //            {
            //                ExecutorName = HondaModelTrainerExecutors.ClassificationModelTrainingDataCollector.ToString(),
            //                Ignore = false,
            //            }.ToBsonDocument()
            //        },
            //        new JobActionServiceModel
            //        {
            //            ActionId = ModelTrainingActions.ModelTraining.ToString(),
            //            Description = "Train model.",
            //            IsDefault = false,
            //            Options = new TrainingDataCollectionOptions
            //            {
            //                ExecutorName = HondaModelTrainerExecutors.ClassificationModelTrainer.ToString(),
            //                Ignore = false,
            //            }.ToBsonDocument()
            //        },
            //        new JobActionServiceModel
            //        {
            //            ActionId = ModelTrainingActions.ModelServing.ToString(),
            //            Description = "Serve trained model.",
            //            IsDefault = false,
            //            Options = new TrainingDataCollectionOptions
            //            {
            //                ExecutorName = HondaModelTrainerExecutors.HondaModelServer.ToString(),
            //                Ignore = false,
            //            }.ToBsonDocument()
            //        },
            //        new JobActionServiceModel
            //        {
            //            ActionId = ModelTrainingActions.TrainingHistoryUpdateing.ToString(),
            //            Description = "Update model training history.",
            //            IsDefault = false,
            //            Options = new TrainingDataCollectionOptions
            //            {
            //                ExecutorName = HondaModelTrainerExecutors.HondaTrainingHistoryUpdater.ToString(),
            //                Ignore = false,
            //            }.ToBsonDocument()
            //        }
            //    }
            //});
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, logging) =>
                {
                    logging.ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient()
                        .AddHondaDataAccess(hostContext.Configuration.GetSection("HondaDataAccess"))
                        .AddScheduler(hostContext.Configuration.GetSection("Scheduler"), (services, configuration) =>
                        {
                            return services.AddPlugins(hostContext.Configuration);
                        });
                });
    }

}
