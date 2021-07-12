using MusicKG.Scheduler.Engine.Models;

namespace MusicKG.DataManager.Translator.Options
{
    public class DataCollectionOptions : JobActionOptions
    {
        public int BatchSize { get; set; } = 500;
    }
}