using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using MusicKG.Service.Settings;
using System.Threading.Tasks;
using MusicKG.Service.Helpers;
using MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.Models;
using Newtonsoft.Json;
using MusicKG.Service.Constants;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations.Processors
{
    public class TokenizationProcessor : ISyncProcessorProvider
    {
        private TokenizationProcessorSettings settings;
        private ILogger logger;
        private IHttpClientFactory httpClientFactory;
        private IDictionaryService dictionaryService;

        public TokenizationProcessor()
        {
        }

        public void Initialize(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
        {
            this.settings = new TokenizationProcessorSettings();
            this.settings.ParseFrom(configuration);
            this.logger = logger;
            this.httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            this.dictionaryService = serviceProvider.GetService<IDictionaryService>();
        }

        public async Task<(byte[] content, string contentType, long itemCount)> ProcessAsync(string workspaceId, byte[] content, string contentType)
        {
            var httpClient = httpClientFactory.CreateClient();

            HttpContent httpContent;

            switch (contentType)
            {
                case HttpContentTypes.ApplicationJson:
                    //Do not process json file.
                    return (content, contentType, -1);
                case HttpContentTypes.TextPlain:
                case HttpContentTypes.TextHtml:
                    httpContent = new StringContent(JsonConvert.SerializeObject(await CreateServiceModelAsync(workspaceId, content)));
                    break;
                default:
                    httpContent = new ByteArrayContent(content);
                    break;
            }

            if (httpContent.Headers.ContentType != null)
            {
                httpContent.Headers.ContentType.MediaType = contentType;
            }
            try
            {
                var respond = await httpClient.PostAsync(settings.ServiceUrl, httpContent);

                respond.EnsureSuccessStatusCode();

                return await CreateServiceResultAsync(respond);
            }
            catch (Exception ex)
            {
                logger?.LogError($"Tokenization process failed, details: {ex.ToString()}");
                ErrorHelper.ThrowException(Resources.MusicKGMessages.TokenizationProcessFailed, System.Net.HttpStatusCode.InternalServerError, ex.Message);
                return (null, null, -1);
            }
        }

        private async Task<PreprocessServiceModel> CreateServiceModelAsync(string workspaceId, byte[] content)
        {
            return new PreprocessServiceModel
            {
                LabelingTaskName = settings.LabelingTaskName,
                Paragraphs = new string[] { Encoding.UTF8.GetString(content) },
                TokenizationDict = settings.UseDictionary ? await dictionaryService.GetAllDictionaryEntriesAsync(workspaceId) : new List<string>()
            };
        }

        private async Task<(byte[] content, string contentType, long itemCount)> CreateServiceResultAsync(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<List<TokenizationPreprocessResultModel>>(responseContent);

            return (Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resultObject.First())), response.Content.Headers.ContentType.MediaType, resultObject.Sum(o => o.Items.LongCount()));
        }
    }
}
