using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.Settings
{
    public class VehicleDataConstructorSettings
    {
        public Dictionary<string, VehicleDataSettings> VehicleDataSettings { get; set; }
    }

    public class VehicleIgnoreData
    {
        public string CarModel { get; set; }

        public List<string> CarType { get; set; }
    }

    public class VehicleDataSettings
    {
        public string RawIdFeature { get; set; } = "";

        public string CarModelFeature { get; set; }

        public string CarTypeFeature { get; set; }

        public string ModelYearFeature { get; set; }

        public string VINFeature { get; set; }

        public string MileAgeFeature { get; set; }

        public string RegionFeature { get; set; }

        public string ProvinceFeature { get; set; }

        public string DealerCDFeature { get; set; }

        public string DealerNameFeature { get; set; }

        public string ProductionDateFeature { get; set; }

        public string InitialRegistDateFeature { get; set; }

        public string FaultDateFeature { get; set; }

        public string PartNoFeature { get; set; }

        public string CostRepairFeature { get; set; }

        public string PartNameFeature { get; set; }

        public string SyndromeFeature { get; set; }

        public List<string> KeyFeatures { get; set; }
    }
}
