using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicKG.DataAccess;
using MusicKG.Service.Enums;
using MusicKG.Service.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IOntologyExportProvider
    {
        Task<byte[]> ExportAsync(OntologyDownloadServiceModel downloadModel);
    }
}
