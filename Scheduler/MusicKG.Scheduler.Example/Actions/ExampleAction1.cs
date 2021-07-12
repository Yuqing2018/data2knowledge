using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Example.Context;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.Scheduler.Example.Actions
{
    public class ExampleAction1 : JobAction<ExampleContext, JobActionOptions, DefaultActionData>
    {
        public ExampleAction1(Func<string, IActionExecutor> executorFactory, ILogger<ExampleAction1> logger) : 
            base(executorFactory, logger)
        {
            ActionType = nameof(ExampleAction1);
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, JobActionOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(nameof(ExampleAction2), nextActionData, message);
        }
    }
}
