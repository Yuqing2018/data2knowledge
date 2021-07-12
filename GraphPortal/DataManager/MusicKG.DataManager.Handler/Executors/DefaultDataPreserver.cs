using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Models;
using MusicKG.DataManager.Translator.Options;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.WebApi.ClientWrapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultDataPreserver : ActionExecutor<DataTranslatorContext, DataPreservationOptions, DataPreservationActionData, object>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DefaultDataPreserver(IHttpClientFactory httpClientFactory,
            ILogger<DefaultDataPreserver> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
        }

        protected override Task<object> ExecuteInternalAsync(string actionId,
            DataTranslatorContext context,
            DataPreservationOptions options,
            DataPreservationActionData data)
        {
            if (data?.Documents == null)
            {
                logger.LogActionInfo("There is no data to be preserved.", actionId);
                return null;
            }

            var httpClient = httpClientFactory.CreateClient();

            var user = UserWrapper.LoginAsync(httpClient, context.Parameters.DataPreserverSetttings.ServiceUrl,
                context.Task.Executor, context.Task.ExecutorPassword).GetAwaiter().GetResult();

            if (string.IsNullOrWhiteSpace(user?.Token))
            {
                var message = $"Executor {context.Task.Executor} log in failed.";

                logger.LogActionError(actionId, message);

                throw new JobExecuteException(message, actionId);
            }

            var token = user.Token;

            var uploadResults = new List<string>();

            var succeedData = new List<DataPreservationModel>();

            try
            {
                foreach (var item in data.Documents)
                {
                    var updateResult = DocumentWrapper.UploadAsync(httpClient, 
                        context.Parameters.DataPreserverSetttings.ServiceUrl, token,
                        context.Parameters.WorkspaceId, 
                        item.Tags, item.ItemCount, item.FileName, item.ContentType, item.Content).GetAwaiter().GetResult();
                    
                    uploadResults.AddRange(updateResult);
                    
                    succeedData.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                data.Documents = data.Documents.Except(succeedData);
                UserWrapper.LogoutAsync(httpClient, context.Parameters.DataPreserverSetttings.ServiceUrl, token).Wait();
            }

            return null;
        }
    }
}
