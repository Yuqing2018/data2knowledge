using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaDocumentCollector : DocumentCollector<AnnotationDocumentItem>
    {
        public HondaDocumentCollector(ILogger<HondaDocumentCollector> logger) 
            : base(logger)
        {
            ExecutorType = HondaExecutors.HondaDocumentCollector.ToString();
        }

        protected override IEnumerable<AnnotationDocumentItem> ConvertDocumentToItems(byte[] documentContent)
        {
            var document = JsonConvert.DeserializeObject<AnnotationDocument>(Encoding.UTF8.GetString(documentContent));

            return document?.Items?.Select(item =>
            {
                item.DataSourceName = document.DataSource.ToString();
                return item;
            });
        }
    }
}
