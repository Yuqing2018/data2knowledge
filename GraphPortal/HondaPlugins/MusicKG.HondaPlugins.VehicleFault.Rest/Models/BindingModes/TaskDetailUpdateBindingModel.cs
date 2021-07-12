using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class TaskDetailUpdateBindingModel
    {
        [Display(Name = "报警频率")]
        [Required]
        public WarningFrequency Frequency { get; set; }

        [Display(Name = "具体指定日期")]
        public DateTime? SpecifiedDate { get; set; }

        [Display(Name = "是否重点关注")]
        public bool IsFocused { get; set; }
    }
}
