using System;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Business
{
    public class VehicleFaultRelatedDataModel
    {
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
        /// 最后更新时间
        /// </summary>
        public DateTime LastModifiedAt { get; set; }

        #region QIC Data

        /// <summary>
        /// 推进日期 RELEASE_DATE
        /// </summary>
        public DateTime? QicReleaseDate { get; set; }
        
        /// <summary>
        /// qic标题 防错 THEME
        /// </summary>
        public string QicTheme { get; set; }

        /// <summary>
        /// 等级RANK
        /// </summary>
        public string QicRank { get; set; }

        /// <summary>
        /// QIC接收日 RECEIVE_DATE
        /// </summary>
        public DateTime? QicReceiveDate { get; set; }

        /// <summary>
        /// 零件接收日(ST)DAM_PART_ARR_DATE
        /// </summary>
        public DateTime? QicDamPartArrDate { get; set; }

        /// <summary>
        /// 广本/东本 GD_FLAG    G：广本件，D：东本件
        /// </summary>
        public string QicGDFlag { get; set; }

        /// <summary>
        /// KD/LP件IS_IMPORT_FLAG    KD(Y)：进口件，LP(N)：国产件;
        /// </summary>
        public string QicIsImportFlag { get; set; }

        /// <summary>
        /// 处置判断码1 - 推进进度 
        /// DEAL_1ST_CODE
        /// </summary>
        public string QicDeal1STCode { get; set; }

        /// <summary>
        /// 一次判断说明	DEAL_1ST_DESC
        /// </summary>
        public string  QicDeal1STDesc { get; set; }

        /// <summary>
        /// 处置判断码2	DEAL_2ND_CODE
        /// </summary>
        public string QicDeal2NDCode { get; set; }

        #endregion
    }

    public class RelatedPart
    {
        /// <summary>
        /// 车型
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 零件号
        /// </summary>
        public string No { get; set; }
    }
}
