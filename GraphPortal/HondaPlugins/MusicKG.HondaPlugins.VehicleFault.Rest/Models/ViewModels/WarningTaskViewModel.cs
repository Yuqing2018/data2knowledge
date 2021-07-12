using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels
{
    public class WarningTaskViewModel
    {
        [Display(Name = "Warning Task ID")]
        public string Id { get; set; }

        [Display(Name = "Warning Task Name")]
        public string Name { get; set; }

        [Display(Name = "Warning Status")]
        public WarningTaskStatus WarningStatus { get; set; }

        [Display(Name = "Warning Unit")]
        public WarningUnit WarningUnit { get; set; }
        /// <summary>
        /// 车款
        /// </summary>
        [Display(Name = "CarModel")]
        public List<string> CarModels { get; set; }
        /// <summary>
        /// 车型，根据车款多选
        /// </summary>
        [Display(Name = "CarTypes")]

        public List<string> CarTypes { get; set; }
        /// <summary>
        /// 年款：根据车款车型多选
        /// </summary>
        [Display(Name = "YearModels")]

        public List<string> YearModels { get; set; }
        /// <summary>
        /// 预警指标名称，预警值
        /// </summary>
        [Display(Name = "WarningIndexSetting")]
        public List<WarningIndexViewModel> WarningIndexSetting { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }
    }

    
}
