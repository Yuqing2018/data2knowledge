using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Example.Context;
using MusicKG.Scheduler.Example.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Example.Executors
{
    public class ExampleExecutor1 : ActionExecutor<ExampleContext, JobActionOptions, DefaultActionData, ExampleAction2Data>
    {
        private readonly IExampleScopedService exampleService;

        public ExampleExecutor1(IExampleScopedService exampleService, ILogger<ExampleExecutor1> logger) : base(logger)
        {
            this.exampleService = exampleService;
            ExecutorType = nameof(ExampleExecutor1);
        }

        protected async override Task<ExampleAction2Data> ExecuteInternalAsync(string actionId, ExampleContext context, JobActionOptions options, DefaultActionData data)
        {
            logger.LogActionInfo(actionId, $"TaskDefine: {JsonConvert.SerializeObject(context.Task)}");
            logger.LogActionInfo(actionId, "Do something in example action1.");
            return new ExampleAction2Data { ExampleData = exampleService.GetResult() };
        }

        protected async override Task RevertInternalAsync(string actionId, ExampleContext context)
        {
        }
    }
}
