using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IDocumentService
    {
        Task<string> UploadDocumentAsync(string workspaceId, DocumentUploadServiceModel serviceModel);

        Task UpdateDocumentAsync(string workspaceId, string documentId, DocumentUpdateServiceModel serviceModel);

        Task<Tuple<long, IEnumerable<DocumentServiceModel>>> GetDocumentsAsync(string workspaceId, string keyword, string tag, DateTime? fromUploadedAt, DateTime? toUploadedAt,
            IEnumerable<DocumentStatusEnum> status, int from, int? size);

        Task<DocumentServiceModel> GetDocumentAsync(string workspaceId, string documentId);

        Task DeleteDocumentAsync(string workspaceId, string documentId);

        Task<byte[]> GetDocumentContentAsync(string workspaceId, string documentId);
    }
}