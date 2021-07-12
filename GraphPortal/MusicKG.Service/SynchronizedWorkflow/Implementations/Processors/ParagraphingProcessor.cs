using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using System.Linq;
using MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.Models;
using Newtonsoft.Json;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations.Processors
{
    public class ParagraphingProcessor : ISyncProcessorProvider
    {
        private IDictionaryService dictionaryService;

        public void Initialize(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
        {
            this.dictionaryService = serviceProvider.GetService<IDictionaryService>();
        }

        public async Task<(byte[] content, string contentType, long itemCount)> ProcessAsync(string workspaceId, byte[] content, string contentType)
        {
            switch (contentType)
            {
                case HttpContentTypes.TextPlain:
                    return await this.ProcessTextAsync(Encoding.UTF8.GetString(content));
                default:
                    return (content, contentType, -1);
            }
        }

        private async Task<(byte[] content, string contentType, long itemCount)> ProcessTextAsync(string textContent)
        {
            var paragraph = textContent.Split(Environment.NewLine)?.Where(p => !string.IsNullOrWhiteSpace(p));

            var resultData = new TextSimilarityModel
            {
                Items = new List<TextSimilarityItemModel>
                {
                    new TextSimilarityItemModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        TextSource = textContent,
                        TextTargets = new List<string>()
                    }
                },
                Guideline = Resources.MusicKGMessages.TextClassifyGuidlineMessage
            };

            resultData.Items.AddRange(paragraph.Select(p => new TextSimilarityItemModel
            {
                Id = Guid.NewGuid().ToString(),
                TextSource = p.Trim(),
                TextTargets = new List<string>()
            }));

            return await Task.FromResult((Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resultData)), HttpContentTypes.ApplicationJson, resultData.Items.Count));
        }
    }
}
