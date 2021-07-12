using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Raws
{
    [Table("rawqicdata")]
    public class RawQICData
    {
        public long Id { get; set; }

        /// <summary>
        /// 推进日期 RELEASE_DATE
        /// </summary>
        public DateTime? RELEASE_DATE { get; set; }

        /// <summary>
        /// 项目识别码：QIC_NO
        /// </summary>
        public string QIC_NO { get; set; }

        /// <summary>
        /// qic标题 防错 THEME
        /// </summary>
        public string THEME { get; set; }

        /// <summary>
        /// mc编号:MC_NO
        /// </summary>
        public string MC_NO { get; set; }

        /// <summary>
        /// 等级RANK
        /// </summary>
        public string RANK { get; set; }

        /// <summary>
        /// 年款： YEAR_CD
        /// </summary>
        public string YEAR_CD { get; set; }

        /// <summary>
        /// 机种 MODEL_CD
        /// </summary>
        public string MODEL_CD { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string FRAME_NO { get; set; }

        /// <summary>
        /// QIC接收日 RECEIVE_DATE
        /// </summary>
        public DateTime? RECEIVE_DATE { get; set; }

        /// <summary>
        /// 零件接收日(ST)DAM_PART_ARR_DATE
        /// </summary>
        public DateTime? DAM_PART_ARR_DATE { get; set; }

        /// <summary>
        /// 主零件号PART_NO
        /// </summary>
        public string PART_NO { get; set; }

        /// <summary>
        /// 广本/东本 GD_FLAG    G：广本件，D：东本件
        /// </summary>
        public string GD_FLAG { get; set; }

        /// <summary>
        /// KD/LP件IS_IMPORT_FLAG    KD(Y)：进口件，LP(N)：国产件;
        /// </summary>
        public string IS_IMPORT_FLAG { get; set; }

        /// <summary>
        /// 零件名称
        /// </summary>
        public string PART_NAME { get; set; }

        /// <summary>
        /// 处置判断码1	DEAL_1ST_CODE
        /// </summary>
        public string DEAL_1ST_CODE { get; set; }

        /// <summary>
        /// 一次判断说明	DEAL_1ST_DESC
        /// </summary>
        public string DEAL_1ST_DESC { get; set; }

        /// <summary>
        /// 处置判断码2	DEAL_2ND_CODE
        /// </summary>
        public string DEAL_2ND_CODE { get; set; }

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
