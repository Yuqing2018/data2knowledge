using MusicKG.HondaPlugins.DataAccess.Enums;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Labeling
{
    public class AnnotationDocument
    {
        public DataSource DataSource { get; set; }

        public string DocumentTags { get; set; }

        public List<AnnotationDocumentItem> Items { get; set; }
    }
}