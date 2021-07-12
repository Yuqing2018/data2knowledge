using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class AgainWarningIndexMetricsDataModel
    {
        /// <summary>
        /// 发生原因
        /// </summary>
        public string CntrMesrReasonDesc { get; set; }
        /// <summary>
        /// 发生对策
        /// </summary>
        public string PermanentCntr { get; set; }
        /// <summary>
        /// 对策时间
        /// </summary>
        public DateTime? PermanentCntrTime { get; set; }
        /// <summary>
        /// 对策后再发件数
        /// </summary>
        public int CountAfterCntr { get; set; }
        /// <summary>
        /// 对策前不良率
        /// </summary>
        public double DefectRateBeforeCntr { get; set; }
        /// <summary>
        /// 对策后再发不良率
        /// </summary>
        public double DefectRateAfterCntr { get; set; }
    }
}
