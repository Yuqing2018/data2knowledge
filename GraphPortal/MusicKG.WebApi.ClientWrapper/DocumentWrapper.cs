using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.ClientWrapper.Extensions;
using MusicKG.DataAccess.Enums;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace MusicKG.WebApi.ClientWrapper
{
    public class DocumentWrapper
    {
        public static async Task<List<DocumentViewModel>> ListAsync(HttpClient httpClient, string url, string workspaceId, string token, List<string> documentIds)
        {
            var result = new List<DocumentViewModel>();

            foreach (var documentId in documentIds)
            {
                var documentUrl = $"{url}/api/Workspace/{workspaceId}/Document/{documentId}";

                var document = await httpClient.GetAsync<DocumentViewModel>(documentUrl, null, token);

                if (document != null)
                    result.Add(document);
            }

            return result;
        }

        public static async Task<List<DocumentViewModel>> GetAsync(HttpClient httpClient,
            string url,
            string token,
            string workspaceId,
            List<DocumentStatusEnum> status = null,
            string keyword = "",
            string tag = "",
            DateTime? fromUploadedAt = null,
            DateTime? toUploadedAt = null)
        {
            var documentUrl = $"{url}/api/Workspace/{workspaceId}/Document";

            var queryString = new Dictionary<string, string>
            {
                { "from",  "0" },
                { "statuses", string.Join(",", status) },
                { "keyword", keyword },
                { "tag", tag },
                { "fromUploadedAt", fromUploadedAt?.ToString()  },
                { "toUploadedAt", toUploadedAt?.ToString() }
            };

            var result = await httpClient.GetAsync<PaginationViewModel<DocumentViewModel>>(
                documentUrl, queryString, token);

            return result?.Items?.ToList();
        }

        public static async Task<byte[]> DownloadAsync(HttpClient httpClient, string url, string workspaceId, string documentId, string token)
        {
            var downloadUrl = $"{url}/api/Workspace/{workspaceId}/Document/Content/{documentId}";
            
            return await httpClient.DownloadAsync(downloadUrl, null, token);
        }

        public static async Task DeleteAsync(HttpClient httpClient, string url, string workspaceId, string documentId, string token)
        {
            var deleteUrl = $"{url}/api/Workspace/{workspaceId}/Document/{documentId}";

            httpClient.AddToken(token);

            var result = await httpClient.DeleteAsync(deleteUrl);

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException("Delete document failed.");
        }

        public static async Task<List<string>> UploadAsync(HttpClient httpClient,
            string url,
            string token,
            string workspaceId,
            List<string> tags,
            long itemCount,
            string fileName,
            string contentType,
            byte[] content,
            string parentDocumentId = null)
        {
            var documentUrl = $"{url}/api/Workspace/{workspaceId}/Document/Content";
            var formData = new List<KeyValuePair<string, string>>();
            var formFiles = new List<KeyValuePair<string, (string fileName, string contentType, byte[] content)>>();
            tags.ForEach(tag =>
            {
                formData.Add(new KeyValuePair<string, string>("tags", tag));
            });

            formData.Add(new KeyValuePair<string, string>("documentItems", itemCount.ToString()));
            formFiles.Add(new KeyValuePair<string, (string fileName, string contentType, byte[] content)>("files", (fileName, contentType, content)));

            var result = await httpClient.PostAsFormAsync<List<string>>(
               documentUrl, formData, token, formFiles);

            return result;
        }
    }
}
