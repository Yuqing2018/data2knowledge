using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Raws
{
    [Table("rawregiondata")]
    public class RawRegionData
    {
        public long Id { get; set; }

        public string Region { get; set; }

        public string DealerCD { get; set; }

        public string DealerName { get; set; }

        public string Province { get; set; }

        public DateTime Timestamp { get; set; }
    }
}