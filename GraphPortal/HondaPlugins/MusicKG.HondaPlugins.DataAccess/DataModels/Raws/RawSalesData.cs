using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Raws
{
    [Table("rawsalesdata")]
    public class RawSalesData
    {
        public long Id { get; set; }

        public string CarModel { get; set; }

        public string CarType { get; set; }

        public string ModelYear { get; set; }

        public string FrameNo { get; set; }

        public string ProductionDate { get; set; }

        public string InitialRegistDate { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
