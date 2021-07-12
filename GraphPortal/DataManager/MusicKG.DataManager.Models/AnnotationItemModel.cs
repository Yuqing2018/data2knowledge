using System.Collections.Generic;

namespace MusicKG.DataManager.Models
{
    public abstract class AnnotationItemModel
    {
        public string Id { get; set; }

        public Dictionary<string, object> AnnotationFeatures { get; set; }
    }
}
