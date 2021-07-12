using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Resources;
using MusicKG.HondaPlugins.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class BaseSearchModel
    {
        public BaseSearchModel() { }

        public BaseSearchModel(List<string> carModels, List<string> carTypes, List<string> yearModels, List<string> partName, List<string> syndrome, List<DataSource> datasource = null)
        {
            CarModel = carModels;
            CarType = carTypes;
            YearModels = yearModels;
            PartName = partName;
            Syndrome = syndrome;
            Datasource = datasource;
        }
        /// <summary>
        /// 车款
        /// </summary>
        public List<string> CarModel { get; set; }
        /// <summary>
        /// 车型，根据车款多选
        /// </summary>

        public List<string> CarType { get; set; }
        /// <summary>
        /// 年款：根据车款车型多选
        /// </summary>

        public List<string> YearModels { get; set; }
        /// <summary>
        /// 零件名
        /// </summary>

        public List<string> PartName { get; set; }
        /// <summary>
        /// 不良症状
        /// </summary>

        public List<string> Syndrome { get; set; }

        public List<DataSource> Datasource { get; set; }
    }
}
