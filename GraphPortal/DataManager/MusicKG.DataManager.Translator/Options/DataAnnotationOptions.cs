using MusicKG.Scheduler.Engine.Models;

namespace MusicKG.DataManager.Translator.Options
{
    public class DataAnnotationOptions : JobActionOptions
    {
        public int BatchSize { get; set; } = 100;
    }
}