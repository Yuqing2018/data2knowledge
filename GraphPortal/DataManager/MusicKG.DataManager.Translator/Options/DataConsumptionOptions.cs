using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Models;

namespace MusicKG.DataManager.Translator.Options
{
    public class DataConsumptionOptions : JobActionOptions
    {
        public DataConsumers Consumers { get; set; }
    }
}
