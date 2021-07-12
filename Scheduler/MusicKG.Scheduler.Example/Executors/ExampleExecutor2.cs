using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Example.Context;
using MusicKG.Scheduler.Example.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Example.Executors
{
    public class ExampleExecutor2 : ActionExecutor<ExampleContext, JobActionOptions, ExampleAction2Data, object>
    {
        public ExampleExecutor2(ILogger<ExampleExecutor2> logger) : base(logger)
        {
            ExecutorType = nameof(ExampleExecutor2);
        }

        protected async override Task<object> ExecuteInternalAsync(string actionId, ExampleContext context, JobActionOptions options, ExampleAction2Data data)
        {
            logger.LogActionInfo(actionId, $"Do something in example action2 with data {data.ExampleData}.");
            return new object();
        }

        protected override Task RevertInternalAsync(string actionId, ExampleContext context)
        {
            throw new NotImplementedException();
        }
    }
}
