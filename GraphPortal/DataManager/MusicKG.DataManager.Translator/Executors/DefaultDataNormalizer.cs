using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultDataNormalizer : DataNormalizer<DefaultAnnotationItem>
    {
        public DefaultDataNormalizer(ILogger<DefaultDataNormalizer> logger) : base(logger)
        {
            ExecutorType = DataTranslationDefaultExecutors.DefaultDataNormalizer.ToString();
        }

        protected override IEnumerable<List<DefaultAnnotationItem>> NormalizeData(string actionId, 
            DataTranslatorContext context, 
            DataNormalizationOptions options,
            IEnumerable<List<Dictionary<string, object>>> items)
        {
            return items?.Select(data => data.Select(d => new DefaultAnnotationItem
            {
                Id = Guid.NewGuid().ToString(),
                AnnotationFeatures = d
            }).ToList());
        }
    }
}
