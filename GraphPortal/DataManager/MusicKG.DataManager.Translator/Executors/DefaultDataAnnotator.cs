using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultDataAnnotator : DataAnnotator<DefaultAnnotationItem>
    {
        public DefaultDataAnnotator(ILogger<DefaultDataAnnotator> logger) : base(logger)
        {
            ExecutorType = DataTranslationDefaultExecutors.DefaultDataAnnotator.ToString();
        }

        protected override IEnumerable<DefaultAnnotationItem> AnnotateData(string actionId, DataTranslatorContext context, DataAnnotationOptions options, IEnumerable<List<DefaultAnnotationItem>> data)
        {
            return data?.SelectMany(d => d);
        }
    }
}
