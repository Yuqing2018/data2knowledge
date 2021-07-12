using System.Collections.Generic;

namespace MusicKG.DataManager.Models
{
    public class DataPreservationModel
    {
        public List<string> Tags { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ItemCount { get; set; }

        public byte[] Content { get; set; }
    }
}
