using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Raws
{
    [Table("rawvehicledata")]
    public class RawVehicleData
    {
        public long Id { get; set; }

        public string CarModel { get; set; }

        public string CarType { get; set; }

        public string ModelYear { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
