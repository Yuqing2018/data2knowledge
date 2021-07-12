using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultDataConsumer : DataConsumer<DefaultAnnotationItem>
    {
        public DefaultDataConsumer(ILogger<DefaultDataConsumer> logger) : base(logger)
        {
            ExecutorType = DataTranslationDefaultExecutors.DefaultDataConsumer.ToString();
        }

        protected override TaskCreationActionData ConsumeData(string actionId, DataTranslatorContext context, DataConsumptionOptions options, IEnumerable<DefaultAnnotationItem> items)
        {
            return new TaskCreationActionData { Documents = null };
        }

        protected async override Task RevertInternalAsync(string actionId, DataTranslatorContext context)
        {
            //Do nothing.
        }
    }
}
