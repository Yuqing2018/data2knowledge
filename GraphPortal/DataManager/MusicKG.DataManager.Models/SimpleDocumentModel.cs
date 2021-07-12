using System.Collections.Generic;

namespace MusicKG.DataManager.Models
{
    public class SimpleDocumentModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> Tags { get; set; }
    }
}
