using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.Settings
{
    public class ConstantSettings
    {
        public static string DefaultWorkSpaceId { get; set; }
        
        public static string LabelToolUrl { get; set; }
        
        public static string DefaultManagerName { get; set; }

        public static string DefaultManagerPassword { get; set; }

        public static List<string> CarModelsOdBrandA { get; set; } = new List<string>() { "CDX", "RDX", "TLX" };

        public static string JoinString { get; set; } = "**Join**";

        public static string UnknownString { get; set; } = "未知";

        public static string WarningCalculationJobId { get; set; } = "2a8b8a89-8e2f-4895-b5df-25379b24696d";

        public static string DataReannotationJobId { get; set; } = "14f999fb-3378-46fe-81ec-19d510202ae9";

        public static int MaxExportLine { get; set; } = 60000;

        public static int OverallMaxQueryDaysByDay { get; set; } = 30;

        public static int OverallMaxQueryDaysByWeek { get; set; } = 180;

        public static int OverallMaxQueryDaysByMonth { get; set; } = 730;


    }
}
