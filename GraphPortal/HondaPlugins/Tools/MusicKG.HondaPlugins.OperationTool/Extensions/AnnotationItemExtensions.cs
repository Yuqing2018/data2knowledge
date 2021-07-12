using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.DataManager.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MusicKG.HondaPlugins.OperationTool.Extensions
{
    public static class AnnotationItemExtensions
    {
        public static ModelTrainingData ToModelTrainingData(this AnnotationDocumentItem item,
            DataSource dataSource, DateTime runTime)
        {
            var result = new ModelTrainingData();

            result.DataSource = dataSource.ToString();
            result.Id = string.IsNullOrWhiteSpace(item.RawId) ? Guid.NewGuid().ToString() : item.RawId;
            result.ModelName = HondaModelNames.TextClassificationModel.ToString();
            result.Timestamp = runTime;

            result.InputFeatures = item.Features?.Where(feature => feature.Value.KeyFeature)?
                .Select(feature =>
                {
                    return KeyValuePair.Create(feature.Key, feature.Value.Value);
                })?.ToDictionary(k => k.Key, v => v.Value);

            var partName = item.Result?.PartName?.Value ?? item?.OriginalResult?.PartName?.Value;

            var syndrome = item.Result?.Syndrome?.Id ?? item?.OriginalResult?.Syndrome?.Id;

            result.OutputFeatures = new Dictionary<string, string>
            {
                { nameof(item.Result.Syndrome), item.Result?.Syndrome?.Id },
                { nameof(item.Result.PartName), partName },
            };

            return result;
        }
    }
}
