using System.Collections.Generic;

namespace MusicKG.DataManager.Translator.Data
{
    public class DataNormalizationActionData
    {
        public IEnumerable<List<Dictionary<string, object>>> RawData { get; set; }
    }
}
