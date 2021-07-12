using MusicKG.HondaPlugins.WarningCalculator.Contexts;
using MusicKG.HondaPlugins.WarningCalculator.Data;
using MusicKG.HondaPlugins.WarningCalculator.Options;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.WarningCalculator.Executors
{
    public class WarningAnnouncer : ActionExecutor<WarningCalculatorContext, WarningAnnouncementOptions, WarningAnnouncementActionData, object>
    {
        public WarningAnnouncer(ILogger<WarningAnnouncer> logger) : base(logger)
        {
            ExecutorType = WarningCalculatorExecutors.WarningAnnouncer.ToString();
        }

        protected async override Task<object> ExecuteInternalAsync(string actionId, WarningCalculatorContext context, WarningAnnouncementOptions options, WarningAnnouncementActionData data)
        {
            //TODO: Send email.
            return new object();
        }

        protected async override Task RevertInternalAsync(string actionId, WarningCalculatorContext context)
        {
            //TODO: Recall email.
        }
    }
}
