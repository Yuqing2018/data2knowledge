using MusicKG.DataManager.Models;
using System.Collections.Generic;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Settings;
using System.Linq;

namespace MusicKG.HondaPlugins.OperationTool.Extensions
{
    public static class ModelTrainingDataExtensions
    {
        public static string ToLine(this ModelTrainingData data)
        {
            const string inputJoinString = "|", featuresJoinString = "@@@@";

            var partName = GetOutputValue(data.OutputFeatures, nameof(VehicleFaultDataModel.PartName));

            var syndrome = GetOutputValue(data.OutputFeatures, nameof(VehicleFaultDataModel.Syndrome));

            List<string> features = new List<string> { data.Id };
            features.Add(string.Join(inputJoinString, data.InputFeatures.Values.Select(v => v.Replace("\r\n", "")?.Replace("\n", "").TrimStart('@').TrimEnd('@'))));

            features.Add(syndrome);
            features.Add(partName);

            return string.Join(featuresJoinString, features);
        }

        private static string GetOutputValue(Dictionary<string, string> output, string key)
        {
            if (output.TryGetValue(key, out var result))
            {
                result = result?.Replace("\r\n", "")?.Replace("\n", "");

                if (result == null || result == ConstantSettings.UnknownString)
                    result = "";
            }
            else
                result = "";

            return result;
        }
    }
}
