using System.Collections.Generic;

namespace MusicKG.DataManager.Models
{
    public class DefaultAnnotationDocument<TAnnotationItem> where TAnnotationItem : AnnotationItemModel
    {
        public string DataSource { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<TAnnotationItem> Items { get; set; }
    }
}
