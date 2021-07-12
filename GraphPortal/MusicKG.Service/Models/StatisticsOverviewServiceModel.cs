using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class StatisticsOverviewServiceModel
    {
        public UserServiceModel User { get; set; }

        public StatisticsServiceModel Statistics { get; set; }
    }
}
