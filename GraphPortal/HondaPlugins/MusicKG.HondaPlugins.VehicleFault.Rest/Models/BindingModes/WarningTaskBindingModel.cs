using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class WarningTaskBindingModel
    {
        [Display(Name = "Warning Task Name")]
        public string Name { get; set; }
        /// <summary>
        /// 预警单元
        /// </summary>
        [Display(Name = "Warning Unit")]
        public WarningUnit WarningUnit { get; set; }
        /// <summary>
        /// 车款
        /// </summary>
        [Display(Name = "CarModel")]
        public List<string> CarModel { get; set; }
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
        [Display(Name = "WarningIndex")]
        [Required]
        public List<WarningIndexViewModel> WarningIndex { get; set; }
    }
}
