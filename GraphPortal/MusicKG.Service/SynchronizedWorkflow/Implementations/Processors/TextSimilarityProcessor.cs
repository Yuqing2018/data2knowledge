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
using MusicKG.DataAccess.Enums;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations.Processors
{
    public class TextSimilarityProcessor : ISyncProcessorProvider
    {
        private ITagService tagService { get; set; }

        public void Initialize(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
        {
            this.tagService = serviceProvider.GetService<ITagService>();
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
            char[] separator = { '.', '。', '!', '？', '?', '！' };

            var sentences = textContent.Split(separator)?.Where(p => !string.IsNullOrWhiteSpace(p));

            var resultData = new TextSimilarityModel
            {
                Items = sentences.Select(s => new TextSimilarityItemModel
                {
                    Id = Guid.NewGuid().ToString(),
                    TextSource = s.Trim(),
                    TextTargets = new List<string>()
                }).ToList(),
                Guideline = Resources.MusicKGMessages.TextSimilarityGuidlineMessage
            };

            return await Task.FromResult((Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resultData)), HttpContentTypes.ApplicationJson, resultData.Items.Count));
        }
    }
}
