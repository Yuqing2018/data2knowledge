using MusicKG.Scheduler.Engine.Models;

namespace MusicKG.DataManager.Translator.Options
{
    public class DataConstructionOptions : JobActionOptions
    {
        public bool IgnoreManualAnnotation { get; set; } = false;
    }
}
