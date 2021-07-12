using MusicKG.HondaPlugins.DataAccess.Enums;
using System;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class VehicleFaultListServiceModel
    {
        /// <summary>
        /// 源数据Id
        /// </summary>
        public string RawId { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public DataSource DataSource { get; set; }

        /// <summary>
        /// 车款
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 车型
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
        /// 特约店名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 总维修费用
        /// </summary>
        public float CostRepair { get; set; } = 0f;

        /// <summary>
        /// 零件号
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// 故障零件名
        /// 标注字段
        /// </summary>
        public string PartName { get; set; }    

        /// <summary>
        /// 不良症状名称
        /// 标注字段
        /// </summary>
        public SyndromeServiceModel SyndromeModel { get; set; }

        /// <summary>
        /// 不良症状Id
        /// 标注字段
        /// </summary>
        public string Syndrome { get; set; }
    }
}
