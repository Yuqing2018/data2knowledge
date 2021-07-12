using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class TaskRecordAIRiskLevelBindingModel
    {
        [Display(Name ="AI 风险等级")]
        public RiskLevel AIRiskLevel { get; set; }

        [Display(Name = "是否为训练数据")]
        public bool UsedForModel { get; set; }
    }
}
