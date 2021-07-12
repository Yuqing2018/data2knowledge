using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models.Enums;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultDataHandler : DataHandler<DefaultAnnotationItem>
    {
        public DefaultDataHandler(ILogger<DefaultDataHandler> logger) : base(logger)
        {
            ExecutorType = DataHandlingDefaultExecutors.DefaultDataHandler.ToString();
        }

        protected override void HandleDataAsBusiness(string actionId, DataHandlerContext context, DataHandlingOptions options, DefaultAnnotationItem annotationItem)
        {
            //Do nothing.
        }

        protected override void HandleDataAsModelTraning(string actionId, DataHandlerContext context, DataHandlingOptions options, DefaultAnnotationItem annotationItem)
        {
            //Do nothing.
        }
    }
}
