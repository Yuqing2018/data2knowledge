using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataManager.Helpers
{
    public static class VINHelper
    {
        public static (string, string) ParseVIN(string vin)
        {
            if (ValidateVIN(vin))
                return (GetCarType(vin), GetModelYear(vin).ToString());
            return (null, null);
        }

        public static string GetCarType(string vin)
        {
            return vin.Substring(3, 3);
        }

        public static int GetModelYear(string vin, int startYear = 2010)
        {
            return startYear + ModelYearMapping[vin[9]];
        }

        public static bool ValidateVIN(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin) || vin.Length < 17 || !ModelYearMapping.ContainsKey(vin[9]))
            {
                return false;
            }
            return true;
        }

        public static readonly Dictionary<char, int> ModelYearMapping = new Dictionary<char, int>
        {
            { 'A', 0 },
            { 'B', 1 },
            { 'C', 2 },
            { 'D', 3 },
            { 'E', 4 },
            { 'F', 5 },
            { 'G', 6 },
            { 'H', 7 },
            { 'J', 8 },
            { 'K', 9 },
            { 'L', 10 },
            { 'M', 11 },
            { 'N', 12 },
            { 'P', 13 },
            { 'R', 14 },
            { 'S', 15 },
            { 'T', 16 },
            { 'V', 17 },
            { 'W', 18 },
            { 'X', 19 },
            { 'Y', 20 },
            { '1', 21 },
            { '2', 22 },
            { '3', 23 },
            { '4', 24 },
            { '5', 25 },
            { '6', 26 },
            { '7', 27 },
            { '8', 28 },
            { '9', 29 },
        };
    }
}
