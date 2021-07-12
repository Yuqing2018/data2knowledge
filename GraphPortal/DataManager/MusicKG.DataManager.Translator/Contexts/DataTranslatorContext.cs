using MusicKG.DataManager.Translator.Tasks;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.WebApi.ClientWrapper;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MusicKG.DataManager.Translator.Contexts
{
    /// <summary>
    /// The context used for storing configuration or data across all actions in this task.
    /// </summary>
    public class DataTranslatorContext : JobTaskContext<DataTranslationTaskDefine>
    {
        public Queue<TimeRange> TimeRanges { get; set; }

        public TimeRange CurrentTimeRange { get; set; }

        public long DataCount { get; set; }

        public List<string> UploadedDocuments { get; set; }

        public List<string> CreatedTasks { get; set; }

        public List<string> SavedItems { get; set; }

        public HttpClient HttpClient { get; set; }

        public string ServiceUrl { get; set; }

        public string Token { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            UploadedDocuments = new List<string>();

            CreatedTasks = new List<string>();
            
            SavedItems = new List<string>();

            TimeRanges = new Queue<TimeRange>();

            DataCount = -1;

            var startDate = base.From;

            while (startDate < base.To)
            {
                var endDate = startDate.AddDays(1);

                if (base.To - startDate <= TimeSpan.FromDays(1))
                {
                    endDate = base.To;
                }

                TimeRanges.Enqueue(new TimeRange
                {
                    Start = startDate,
                    End = endDate
                });

                startDate = endDate;
            }

            if (HttpClient != null && !string.IsNullOrWhiteSpace(ServiceUrl) && !string.IsNullOrWhiteSpace(Task.Executor))
                Token = GetUserToken();
        }

        public override void Dispose()
        {
            TimeRanges.Clear();
            DataCount = -1;
            UploadedDocuments.Clear();
            CreatedTasks.Clear();
            SavedItems.Clear();
            CurrentTimeRange = null;

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
