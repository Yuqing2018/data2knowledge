using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Example.Context;
using MusicKG.Scheduler.Example.Data;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.Scheduler.Example.Actions
{
    public class ExampleAction2 : JobAction<ExampleContext, JobActionOptions, ExampleAction2Data>
    {
        public ExampleAction2(Func<string, IActionExecutor> executorFactory, ILogger<ExampleAction1> logger) : 
            base(executorFactory, logger)
        {
            ActionType = nameof(ExampleAction2);
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, JobActionOptions options, string message, object nextActionData)
        {
            return JobActionResult.Finish(message);
        }
    }
}
