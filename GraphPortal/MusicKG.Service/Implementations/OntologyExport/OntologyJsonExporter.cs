using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicKG.DataAccess;
using MusicKG.Service.Models;
using Newtonsoft.Json;

namespace MusicKG.Service.Implementations.OntologyExport
{
    public class OntologyJsonExporter : IOntologyExportProvider
    {
        /// <summary>
        /// Constructor of exporter for ontology export as json.
        /// </summary>
        public OntologyJsonExporter()
        {
        }

        public async Task<byte[]> ExportAsync(OntologyDownloadServiceModel downloadModel)
        {
            return await Task.Run(() =>
            {
                return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(downloadModel));
            });
        }

    }
}