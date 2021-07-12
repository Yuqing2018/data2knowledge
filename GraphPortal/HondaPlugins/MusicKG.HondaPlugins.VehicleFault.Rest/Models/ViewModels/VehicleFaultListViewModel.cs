using System;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels
{
    public class VehicleFaultListViewModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 车款
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarModel { get; set; }

        /// <summary>
        /// 年款
        /// </summary>
        public string YearModel { get; set; }

        /// <summary>
        /// 零件号
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// 零件名
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 不良症状
        /// </summary>
        public string Syndrome { get; set; }
        public string SyndromeName { get; set; }

        /// <summary>
        /// 故障日期
        /// </summary>
        public DateTime FaultDate { get; set; }
    }
}
