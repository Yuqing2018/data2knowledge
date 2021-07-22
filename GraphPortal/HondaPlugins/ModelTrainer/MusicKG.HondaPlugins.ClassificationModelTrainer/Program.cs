using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.Scheduler.Engine;
using MusicKG.Scheduler.Extensions;
using MusicKG.Scheduler.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using System;
using CommandLine;
using MusicKG.HondaPlugins.DataAccess;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using MusicKG.HondaPlugins.ModelTrainer.Settings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.Utility;

namespace MusicKG.HondaPlugins.ModelTrainer
{
    [Verb("train", HelpText = "Train and serve model automaically.")]
    class TrainOptions
    {
        [Option('n', "name", Required = true, HelpText = "The model name to be trained.")]
        public HondaModelNames ModelName { get; set; }
    }

    [Verb("ls", HelpText = "List model versions.")]
    class ListOptions
    {
    }

    [Verb("revert", HelpText = "Revert model to specified version")]
    class RevertOptions
    {
        [Option('n', "name", Required = true, HelpText = "The model name to be reverted.")]
        public HondaModelNames ModelName { get; set; }

        [Option('v', "version", Required = true, HelpText = "The version to be reverted to.")]
        public int Version { get; set; }
    }

    class Program
    {
        const string ClassificationTrainJobName = "classification-model-trainer";
        const string DecisionTreeTrainJobName = "riskwarning-model-trainer";

        async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var services = host.Services;

            Parser.Default.ParseArguments<TrainOptions, ListOptions>(args)
                .WithParsed<TrainOptions>(options => TrainModel(options, services))
                .WithParsed<ListOptions>(options => ListModelVersions(services))
                .WithParsed<RevertOptions>(options => RevertModel(options, services));
        }
        
        public static void RevertModel(RevertOptions options, IServiceProvider services)
        {
            var classificationSettings = services.GetRequiredService<HondaClassificationModelTrainingSettings>();
            var riskSettings = services.GetRequiredService<HondaRiskModelTrainingSettings>();

            var revertScript = options.ModelName == HondaModelNames.TextClassificationModel ? classificationSettings.RevertModelServingScriptFile : riskSettings.RevertModelServingScriptFile;

            switch (options.ModelName)
            {
                case HondaModelNames.TextClassificationModel:
                    ShellHelper.RunShell(classificationSettings.RevertModelServingScriptFile,
                        classificationSettings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                        error =>
                        {
                            using (var writer = Console.Error)
                            {
                                writer.WriteLine(error);
                            }
                        },
                        info => Console.WriteLine(info),
                        classificationSettings.ModelServingLocation,
                        classificationSettings.ModelBackupLocation,
                        options.Version.ToString());
                    break;
                case HondaModelNames.DecisionTreeModel:
                    ShellHelper.RunShell(riskSettings.RevertModelServingScriptFile,
                        riskSettings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                        error =>
                        {
                            using (var writer = Console.Error)
                            {
                                writer.WriteLine(error);
                            }
                        },
                        info => Console.WriteLine(info),
                        riskSettings.ModelServingLocation,
                        riskSettings.ModelBackupLocation,
                        options.Version.ToString(),
                        riskSettings.RiskModelInitUrl);
                    break;
            }
        }

        public static void ListModelVersions(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<IHondaMongoDbContext>();

            var histories = dbContext.TrainingHistory.AsQueryable().ToList();

            var lines = histories.GroupBy(x => $"{x.ModelName}_{x.CurrentVersion}").ToDictionary(k => k.Key, v => v.SelectMany(h => h.ModelVersions).ToList())
                .SelectMany(h =>
                {
                    return h.Value.Select((v, index) =>
                    {
                        var modelName = h.Key.Split("_")[0];
                        var currentVersion = h.Key.Split("_")[1];
                        var version = v.Version.ToString() == currentVersion ? $"{v.Version} (Current)" : v.Version.ToString();
                        if (index == 0)
                            return new List<string> { modelName, version, v.TrainedAt.ToString() };
                        return new List<string> { "", version, v.TrainedAt.ToString() };
                    });
                }).ToList();

            lines.Insert(0, new List<string> { "MODEL NAME", "MODEL VERSION", "TRAINED DATE" });

            var column1Max = lines.Max(x => x[0].Length);
            var column2Max = lines.Max(x => x[1].Length);
            var column3Max = lines.Max(x => x[2].Length);

            lines.ForEach(row =>
            {
                row[0] = row[0].PadRight(column1Max);
                row[1] = row[1].PadRight(column2Max);
                row[2] = row[2].PadRight(column3Max);
            });

            lines.ForEach(row => Console.WriteLine($"{row[0]}\t{row[1]}\t{row[2]}"));
        }

        public static void TrainModel(TrainOptions options, IServiceProvider services)
        {
            var jobName = options.ModelName == HondaModelNames.TextClassificationModel ? ClassificationTrainJobName : DecisionTreeTrainJobName;

            var jobService = services.GetRequiredService<IJobService>();

            using (var scope = services.CreateScope())
            {
                var engine = scope.ServiceProvider.GetRequiredService<IJobEngine>();

                var jobServiceModel = jobService.GetByNameAsync(jobName).GetAwaiter().GetResult();

                engine.ExecuteAsync(jobServiceModel).GetAwaiter().GetResult();
            }
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
                            return services.AddHondaModelTrainer(hostContext.Configuration);
                        });
                });
    }
}
