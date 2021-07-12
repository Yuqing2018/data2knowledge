using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class StatisticsExportServiceModel
    {
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Syndrome { get; set; }
        /// <summary>
        /// 1:单台件数表中的按月累计件数
        /// 2:单台金额表中的按月累计金额
        /// </summary>
        public List<MonthCountAndAmount> AllHistoryMonth { get; set; }
        /// <summary>
        /// 累计件数
        /// </summary>
        public int MQITotalCount { get; set; }
        /// <summary>
        /// 1：单台件数表中的维修单价
        /// </summary>
        public float MaintainUnitPrice { get; set; }
        /// <summary>
        /// 2：单台金额表中的单件金额，累计金额/销量
        /// </summary>
        public float SingleUnitAmount { get; set; }
        /// <summary>
        /// 1：单台件数表中的累计不良率
        /// </summary>
        public float AccumulativeRate { get; set; }
        /// <summary>
        /// 2：单台金额表中的累计金额
        /// </summary>
        public float AccumulativeAmount { get; set; }
        /// <summary>
        /// 对策前不良率
        /// </summary>
        public float LastPermanentCntrBeforeRate { get; set; }
        /// <summary>
        /// 对策
        /// </summary>
        public List<PermanentCounterMove> PermanentCntrs { get; set; }
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
        public int GovCount{get;set;}
        public int HotLineCount{get;set; }
        public int TechConsultingCount { get; set; }
        public int MediaCount { get; set; }
    }

    public class PermanentCounterMove
    {
        public int CountBeforeCntr { get; set; }
        /// <summary>
        /// 对策后不良率
        /// </summary>
        public float? AfterRate { get; set; }

        /// <summary>
        /// 对策日期
        /// </summary>
        public DateTime? PermanentCntrTime { get; set; }
    }

    public class MonthCountAndAmount 
    { 
        public string Month { get; set; }
        /// <summary>
        /// 每月不良件数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 每月累计金额
        /// </summary>
        public float TotalAmount { get; set; }
    }
}
