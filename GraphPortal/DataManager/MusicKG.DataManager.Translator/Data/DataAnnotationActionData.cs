using MusicKG.DataManager.Models;
using System.Collections.Generic;

namespace MusicKG.DataManager.Translator.Data
{
    public class DataAnnotationActionData
    {
        public IEnumerable<List<AnnotationItemModel>> RawData { get; set; }
    }
}
