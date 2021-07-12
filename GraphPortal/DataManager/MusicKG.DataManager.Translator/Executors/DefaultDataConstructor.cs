using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Models;
using MusicKG.DataManager.Translator.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultDataConstructor : DataConstructor<DefaultAnnotationItem>
    {
        public DefaultDataConstructor(ILogger<DefaultDataConstructor> logger) : base(logger)
        {
        }

        protected override IEnumerable<DefaultAnnotationItem> ConstructData(string actionId, 
            DataTranslatorContext context, 
            DataConstructionOptions options, 
            IEnumerable<Dictionary<string, object>> data)
        {
            return data?.Select(data => new DefaultAnnotationItem
            {
                Id = Guid.NewGuid().ToString(),
                Features = data
            });
        }
    }
}
