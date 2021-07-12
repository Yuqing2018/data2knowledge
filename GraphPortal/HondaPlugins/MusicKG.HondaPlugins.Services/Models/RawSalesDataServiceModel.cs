using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class RawSalesDataServiceModel
    {
        public string CarModel { get; set; }

        public string CarType { get; set; }

        public string ModelYear { get; set; }

        public string FrameNo { get; set; }

        public DateTime? ProductionDate { get; set; }

        public DateTime? InitialRegistDate { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
