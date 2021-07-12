using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MusicKG.Service.SynchronizedWorkflow.Implementations.Processors
{
    public class PhrasingProcessor : ISyncProcessorProvider
    {
        public void Initialize(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
        {

        }

        public async Task<(byte[] content, string contentType, long itemCount)> ProcessAsync(string workspaceId,byte[] content, string contentType)
        {
            switch (contentType)
            {
                case HttpContentTypes.ApplicationJson:
                    //Do not process json file.
                    return (content, contentType, -1);
                case HttpContentTypes.TextPlain:
                    return await this.ProcessTextAsync(
                        Encoding.UTF8.GetString(content));
                default:
                    return (content, contentType, -1);
            }
        }

        private async Task<(byte[] content, string contentType, long itemCount)> ProcessTextAsync(string textContent)
        {
            char[] separator =  { '.', '。', '!', '？', '?', '！'};

            var sentences = textContent.Split(separator)?.Where(p => !string.IsNullOrWhiteSpace(p));

            var resultData = new PhrasingProcessModel
            {
                Items = sentences.Select(p => new PhrasingItemProcessModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = p.Trim(),
                    SpanItems = new List<string>()
                }).ToList(),
                Guideline = Resources.MusicKGMessages.OntologyEntityAndRelationGuidlineMessage
            };

            return await Task.FromResult((Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resultData)), HttpContentTypes.ApplicationJson, resultData.Items.Count));
        }
    }
}
