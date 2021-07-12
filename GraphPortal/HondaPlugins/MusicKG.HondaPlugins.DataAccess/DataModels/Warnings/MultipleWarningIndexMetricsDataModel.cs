using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class MultipleWarningIndexMetricsDataModel
    {
        /// <summary>
        /// Top排序
        /// </summary>
        public int TopOrder { get; set; }

        /// <summary>
        /// 近三月不良件数统计
        ///     key:yyyy-MM 
        ///     Value：当月件数
        /// </summary>
        public Dictionary<string, int> LastThreeMonthCount { get; set; }

        /// <summary>
        /// 累计件数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 不良率
        /// </summary>
        public double DefectRate { get; set; }
        /// <summary>
        /// 近三月相对上升率
        /// </summary>
        public double LastThreeMonthAscentRate { get; set; }
        /// <summary>
        /// 年款不良率
        /// </summary>
        public Dictionary<string,double> DefectRateByYearModel { get; set; }
        /// <summary>
        /// 是否发生报警
        /// </summary>
        public bool IsWarning { get; set; }
    }
}
