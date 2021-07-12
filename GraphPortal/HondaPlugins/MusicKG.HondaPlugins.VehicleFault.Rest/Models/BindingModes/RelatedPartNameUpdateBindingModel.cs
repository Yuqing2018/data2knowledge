using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class RelatedPartNameUpdateBindingModel
    {
        /// <summary>
        /// 零件号
        /// </summary>
        public string PartNo { get; set; }
        
        /// <summary>
        /// 车型
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 数据来源描述
        /// </summary>
        public string DataFromDesc { get; set; }
    }
}
