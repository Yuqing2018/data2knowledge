using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class WarningTaskDetailSettingDataModel
    {
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
        /// <summary>
        /// 不良症状
        /// </summary>
        public string Syndroms { get; set; }
        /// <summary>
        /// 零件名
        /// </summary>
        public string PartName { get; set; }
    }
}
