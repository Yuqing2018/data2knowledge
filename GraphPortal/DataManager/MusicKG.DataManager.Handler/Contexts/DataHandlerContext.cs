using MusicKG.DataManager.Handler.Tasks;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.WebApi.ClientWrapper;
using System.Collections.Generic;
using System.Net.Http;

namespace MusicKG.DataManager.Translator.Contexts
{
    /// <summary>
    /// The context used for storing configuration or data across all actions in this task.
    /// </summary>
    public class DataHandlerContext : JobTaskContext<DataHandlingTaskDefine>
    {
        public List<string> MergedTaskIds { get; set; }

        public long FinishedTaskCount { get; set; }

        public HttpClient HttpClient { get; set; }

        public string ServiceUrl { get; set; }

        public string Token { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            FinishedTaskCount = -1;

            MergedTaskIds = new List<string>();

            if (HttpClient != null && !string.IsNullOrWhiteSpace(ServiceUrl))
                Token = GetUserToken();
        }

        public override void Dispose()
        {
            if (HttpClient != null && !string.IsNullOrWhiteSpace(Token) && !string.IsNullOrWhiteSpace(ServiceUrl))
                UserWrapper.LogoutAsync(HttpClient, ServiceUrl, Token).GetAwaiter().GetResult();

            HttpClient = null;
            Token = null;
            ServiceUrl = null;

            base.Dispose();
        }

        private string GetUserToken()
        {
            var user = UserWrapper.LoginAsync(HttpClient, ServiceUrl,
                Task.Executor, Task.ExecutorPassword).GetAwaiter().GetResult();

            if (string.IsNullOrWhiteSpace(user?.Token))
            {
                var message = $"Executor {Task.Executor} log in failed.";

                throw new JobExecuteException(message, Unknown);
            }

            return user.Token;
        }
    }
}
