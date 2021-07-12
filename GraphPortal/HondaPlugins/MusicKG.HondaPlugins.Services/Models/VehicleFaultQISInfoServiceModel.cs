using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;

namespace MusicKG.HondaPlugins.Services.Models
{
     public class VehicleFaultQISInfoServiceModel
    {
        //"渠道","不良症状","故障日期","不良等级","QIC号","QIS号","原因区分","原因","对策","对策日期","关联零件","数据来源","更新日期"
        public string Id { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string FrameNo { get; set; }
        /// <summary>
        /// 零件名
        /// </summary>
        public string PartNo { get; set; }
        /// <summary>
        ///MQI Raw Id
        /// </summary>
        public string RawId { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public DataSource DataSource { get; set; }
        //不良症状","不良等级"
        public SyndromeServiceModel SyndromeModel { get; set; }
        /// <summary>
        /// 故障日期
        /// </summary>
        public DateTime FaultDate { get; set; }

        /// <summary>
        /// QIC编号
        /// </summary>
        public string QICNo { get; set; }

        /// <summary>
        /// QIS编号
        /// </summary>
        public string QISNo { get; set; }

        /// <summary>
        /// 原因区分
        /// </summary>
        public int? CntrMesrType { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string CntrMesrReasonDesc { get; set; }

        /// <summary>
        /// 对策
        /// </summary>
        public string PermanentCntr { get; set; }

        /// <summary>
        /// 对策日期
        /// </summary>
        public DateTime? PermanentCntrTime { get; set; }

        /// <summary>
        /// 关联零件信息（手动）
        /// </summary>
        public RelatedPart RelatedPartName { get; set; }

        /// <summary>
        /// 数据来源（MQI自动关联QIS为系统匹配，其他手动输入）
        /// </summary>
        public string DataFrom { get; set; }

        public string DataFromDesc { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastModifiedAt { get; set; }

        public bool HasRelated { get; set; }
    }
}
