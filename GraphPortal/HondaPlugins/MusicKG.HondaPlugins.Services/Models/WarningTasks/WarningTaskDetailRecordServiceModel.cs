using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Models.WarningTasks
{
    public class WarningTaskConfirmRecordServiceModel
    {
        public WarningTaskConfirmRecordServiceModel() { }
        public WarningTaskConfirmRecordServiceModel(WarningRecordServiceModel data)
        {
            Id = data.Id.ToString();
            PartNo = data.PartNo;
            PartName = data.PartName;
            Syndrome = data.Syndrome;
            WarningTime = data.WarningTime;
            ConfirmRecord = data.ConfirmRecord;
            WarningUnit = data.WarningTask.WarningUnit;
            CarModels = data.WarningTask.CarModels;
            CarTypes = data.WarningTask.CarTypes;
            YearModels = data.WarningTask.YearModels;
        }

        public string Id { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Syndrome { get; set; }

        public DateTime WarningTime { get; set; }

        public WarningConfirmRecordDataModel ConfirmRecord { get; set; }

        public WarningUnit WarningUnit { get; set; }

        /// <summary>
        /// 车款
        /// </summary>
        public List<string> CarModels { get; set; }
        /// <summary>
        /// 车型，根据车款多选
        /// </summary>
        public List<string> CarTypes { get; set; }
        /// <summary>
        /// 年款：根据车款车型多选
        /// </summary>
        public List<string> YearModels { get; set; }
    }
}
