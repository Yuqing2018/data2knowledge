using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Models;
using System.Collections.Generic;

namespace MusicKG.DataManager.Translator.Options
{
    public class DataHandlingOptions : JobActionOptions
    {
        public List<DataConsumers> DataConsumers { get; set; } 
    }
}