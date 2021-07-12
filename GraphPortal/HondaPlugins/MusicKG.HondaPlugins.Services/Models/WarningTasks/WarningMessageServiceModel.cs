using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Models.WarningTasks
{
    public class WarningMessageServiceModel
    {
        public string TaskId { get; set; }
        public List<string> CarModels { get; set; }
        public List<string> CarTypes { get; set; }
        public List<string> YearModels { get; set; }
        public int Count { get; set; }
        public Dictionary<int, int> LastWeek { get; set; }
    }
}
