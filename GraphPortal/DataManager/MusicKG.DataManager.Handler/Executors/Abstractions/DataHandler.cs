using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicKG.DataManager.Handler.Data;
using System;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class DataHandler<TAnnotationItem> : ActionExecutor<DataHandlerContext, DataHandlingOptions, DataHandlingActionData, TaskStatusUpdatingActionData> where TAnnotationItem : AnnotationItemModel
    {
        public DataHandler(ILogger<DataHandler<TAnnotationItem>> logger) : base(logger)
        {
        }

        protected async override Task<TaskStatusUpdatingActionData> ExecuteInternalAsync(string actionId,
            DataHandlerContext context,
            DataHandlingOptions options,
            DataHandlingActionData data)
        {
            if (options.Ignore || data == null || data.Data == null || context.FinishedTaskCount == 0)
            {
                logger.LogActionInfo("There is no data to be handled.", actionId);
                return new TaskStatusUpdatingActionData { TaskIds = null };
            }

            foreach (var item in data.Data)
            {
                if (options.DataConsumers.Contains(DataConsumers.Business))
                    HandleDataAsBusiness(actionId, context, options, item as TAnnotationItem);

                if (options.DataConsumers.Contains(DataConsumers.Model))
                    HandleDataAsModelTraning(actionId, context, options, item as TAnnotationItem);
            }

            return new TaskStatusUpdatingActionData { TaskIds = context.MergedTaskIds };
        }

        protected abstract void HandleDataAsModelTraning(string actionId, DataHandlerContext context, DataHandlingOptions options, TAnnotationItem annotationItem);

        protected abstract void HandleDataAsBusiness(string actionId, DataHandlerContext context, DataHandlingOptions options, TAnnotationItem annotationItem);

        protected async override Task RevertInternalAsync(string actionId, DataHandlerContext context)
        {
            // DO Nothing
        }
    }
}
