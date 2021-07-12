using MusicKG.DataManager.Handler.Data;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models.Enums;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.WebApi.ClientWrapper;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultTaskStatusUpdater : ActionExecutor<DataHandlerContext, TaskStatusUpdatingOptions, TaskStatusUpdatingActionData, object>
    {
        public DefaultTaskStatusUpdater(ILogger<DefaultTaskStatusUpdater> logger) : base(logger)
        {
            ExecutorType = DataHandlingDefaultExecutors.DefaultTaskStatusUpdater.ToString();
        }

        protected async override Task<object> ExecuteInternalAsync(string actionId,
            DataHandlerContext context,
            TaskStatusUpdatingOptions options,
            TaskStatusUpdatingActionData data)
        {
            var taskIds = data.TaskIds?.ToList();

            if (taskIds?.Count == 0 || context.FinishedTaskCount == 0)
            {
                logger.LogActionInfo("There is no task to be updated.", actionId);
                return new object();
            }

            var httpClient = context.HttpClient;

            var token = context.Token;
            
            taskIds.ForEach(taskId =>
            {
                TaskWrapper.HandleResultAsync(httpClient, context.ServiceUrl,
                    context.Parameters.WorkspaceId, taskId, token).GetAwaiter().GetResult();
            });

            return new object();
        }

        protected async override Task RevertInternalAsync(string actionId, DataHandlerContext context)
        {
        }
    }
}
