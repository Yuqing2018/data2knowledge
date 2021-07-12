using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels
{
    public class WarningIndexViewModel
    {
        /// <summary>
        /// 预警指标Id
        /// </summary>
        public string Id { get; set; }

        public WarningType WarningType { get; set; }
        /// <summary>
        /// 预警指标名称
        /// </summary>
        public WarningIndexNames IndexName { get; set; }
        /// <summary>
        /// 预警指标设置值
        /// </summary>
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}
