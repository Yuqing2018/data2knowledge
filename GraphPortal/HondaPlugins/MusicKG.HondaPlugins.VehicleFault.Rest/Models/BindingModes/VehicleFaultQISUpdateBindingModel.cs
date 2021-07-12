using System;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class VehicleFaultQISUpdateBindingModel
    {
        /// <summary>
        /// QIC 号
        /// </summary>
        public string QICNo { get; set; }

        /// <summary>
        /// QIS 号
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
    }
}
