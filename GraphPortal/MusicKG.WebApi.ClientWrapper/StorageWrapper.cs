using MusicKG.WebApi.ClientWrapper.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.WebApi.ClientWrapper
{
    public class StorageWrapper
    {
        public async static Task UploadAsync(HttpClient httpClient, string url, string token, string fileName, string contentType, byte[] content)
        {
            var storageUrl = $"{url}/api/Storage";
            var formData = new List<KeyValuePair<string, string>>();
            var formFiles = new List<KeyValuePair<string, (string fileName, string contentType, byte[] content)>>();

            formFiles.Add(new KeyValuePair<string, (string fileName, string contentType, byte[] content)>("files", (fileName, contentType, content)));

            await httpClient.PostAsFormAsync(storageUrl, formData, token, formFiles);
        }
    }
}
