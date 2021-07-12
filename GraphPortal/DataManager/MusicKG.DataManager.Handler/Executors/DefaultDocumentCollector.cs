using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Models;
using MusicKG.DataManager.Models.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataManager.Translator.Executors
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public class DefaultDocumentCollector : DocumentCollector<DefaultAnnotationItem>
    {
        public DefaultDocumentCollector(ILogger<DefaultDocumentCollector> logger) 
            : base(logger)
        {
            ExecutorType = DataHandlingDefaultExecutors.DefaultDocumentCollector.ToString();
        }

        protected override IEnumerable<DefaultAnnotationItem> ConvertDocumentToItems(byte[] documentContent)
        {
            var document = JsonConvert.DeserializeObject<DefaultAnnotationDocument<DefaultAnnotationItem>>(Encoding.UTF8.GetString(documentContent));

            return document?.Items;
        }
    }
}
