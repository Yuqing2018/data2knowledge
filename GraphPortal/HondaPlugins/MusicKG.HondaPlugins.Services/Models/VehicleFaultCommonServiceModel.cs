using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Enums;
using System;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class VehicleFaultCommonServiceModel
    {
        public DataSource DataSource { get; set; }

        public DataSourceType DataSourceType { get; set; }

        /// <summary>
        /// 源数据Id
        /// </summary>
        public string RawId { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 车款
        /// 来自车架号
        /// </summary>
        public string CarModel { get; set; }

        /// <summary>
        /// 年款
        /// 来自车架号
        /// </summary>
        public string ModelYear { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string FrameNo { get; set; }

        /// <summary>
        /// 里程数
        /// 总局数据无此字段
        /// </summary>
        public int MileAge { get; set; } = -1;

        /// <summary>
        /// 生产日期
        /// 如没有根据车架号从销售表中取
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// 销售日期
        /// 如没有根据车架号从销售表中取
        /// </summary>
        public DateTime InitialRegistDate { get; set; }

        /// <summary>
        /// 故障日期
        /// </summary>
        public DateTime FaultDate { get; set; }

        /// <summary>
        /// 特约店
        /// </summary>
        public string DealerCD { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 故障零件名
        /// 标注字段
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 不良症状
        /// 标注字段
        /// </summary>
        public string Syndrome { get; set; }

        public string PartNo { get; set; }
    }
}
