using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels
{
    public class WarningTaskListViewModel
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public WarningTaskStatus WarningStatus { get; set; }

        public WarningUnit WarningUnit { get; set; }

        public string WarningType { get; set; }

        public List<string> CarModels { get; set; }

        public List<string> CarTypes { get; set; }

        public List<string> YearModels { get; set; }

        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
    }
}
