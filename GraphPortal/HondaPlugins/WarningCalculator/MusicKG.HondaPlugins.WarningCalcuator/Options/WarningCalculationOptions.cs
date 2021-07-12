using MusicKG.Scheduler.Engine.Models;

namespace MusicKG.HondaPlugins.WarningCalculator.Options
{
    public class WarningCalculationOptions : JobActionOptions
    {
        public bool IgnoreModel { get; set; } = false;

        public int BatchSize { get; set; } = 50;
    }
}
