using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Raws
{
    [Table("rawqisdata")]
    public class RawQISData
    {
        public long Id { get; set; }

        /// <summary>
        /// QIC号 QIC_NO
        /// </summary>
        public string QIC_NO { get; set; }

        /// <summary>
        /// QIS号 QIS_NO
        /// </summary>
        public string QIS_NO { get; set; }

        /// <summary>
        /// 标题 SUBJECT
        /// </summary>
        public string SUBJECT { get; set; }

        /// <summary>
        /// 原级别 RANK
        /// </summary>
        public string RANK { get; set; }

        /// <summary>
        /// 现级别 AGO_RANK
        /// </summary>
        public string AGO_RANK { get; set; }

        /// <summary>
        /// 担当 CURR_RESOLVER_CD
        /// </summary>
        public string CURR_RESOLVER_CD { get; set; }

        /// <summary>
        /// 组别 CURR_RESOLVER_GROUP
        /// </summary>
        public string CURR_RESOLVER_GROUP { get; set; }

        /// <summary>
        /// 起票日 CREATE_TIME
        /// </summary>
        public DateTime? CREATE_TIME { get; set; }

        /// <summary>
        /// 实际完了日 QIS_FINISHED_DATE
        /// </summary>
        public DateTime? QIS_FINISHED_DATE { get; set; }

        /// <summary>
        /// 推进状态 NUMBER_DAYS
        /// </summary>
        public int? NUMBER_DAYS { get; set; }

        /// <summary>
        /// 零件号 CM_PART
        /// </summary>
        public int? CM_PART { get; set; }

        /// <summary>
        /// 零件名
        /// </summary>
        public string PART_NAME { get; set; }

        /// <summary>
        /// 年款  YEAR_CD
        /// </summary>
        public string YEAR_CD { get; set; }

        /// <summary>
        /// 机种  MODEL_CD
        /// </summary>
        public string MODEL_CD { get; set; }

        /// <summary>
        /// 车身号 FRAME_NO
        /// </summary>
        public string FRAME_NO { get; set; }

        /// <summary>
        /// 原因与进度说明 CNTR_MESR_REASON_DESC
        /// </summary>
        public string CNTR_MESR_REASON_DESC { get; set; }

        /// <summary>
        ///  临时对策    INTERIM_CNTR
        /// </summary>
        public string INTERIM_CNTR { get; set; }

        /// <summary>
        /// 对策时间    INTERIM_CNTR_TIME
        /// </summary>
        public DateTime? INTERIM_CNTR_TIME { get; set; }

        /// <summary>
        ///  恒久对策    PERMANENT_CNTR
        /// </summary>
        public string PERMANENT_CNTR { get; set; }

        /// <summary>
        /// 永久对策时间    PERMANENT_CNTR_TIME
        /// </summary>
        public DateTime? PERMANENT_CNTR_TIME { get; set; }
        
        /// <summary>
        /// 原因区分 CNTR_MESR_TYPE
        /// </summary>
        public int? CNTR_MESR_TYPE { get; set; }
        
        /// <summary>
        /// 要求部门    RESP_DEPARTMENT
        /// </summary>
        public string RESP_DEPARTMENT { get; set; }
        
        /// <summary>
        /// 对策内容详细区分    CNTR_MESR_DETAIL_TYPE
        /// </summary>
        public int? CNTR_MESR_DETAIL_TYPE { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
    }
}
