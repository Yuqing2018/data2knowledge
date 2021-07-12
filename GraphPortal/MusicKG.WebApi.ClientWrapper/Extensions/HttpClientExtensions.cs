using MusicKG.WebApi.Contract.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicKG.WebApi.ClientWrapper.Extensions
{
    public static class HttpClientExtensions
    {
        public const int HttpPostRetryTimes = 3;

        public async static Task<TResult> PostAsBodyAsync<TRequest, TResult>(this HttpClient httpClient, string requestUrl, TRequest requestData, string token = null)
        {
            TResult result = default;

            bool isSucceed = false;

            httpClient.AddToken(token);

            try
            {
                var response = await httpClient.PostAsJsonAsync(requestUrl, requestData);
                result = await response.ToObjectAsync<TResult>();
                isSucceed = true;
            }
            catch
            {
                isSucceed = false;
            }

            var retryTime = 0;

            while (!isSucceed)
            {
                try
                {
                    var response = await httpClient.PostAsJsonAsync(requestUrl, requestData);
                    result = await response.ToObjectAsync<TResult>();
                    isSucceed = true;
                    break;
                }
                catch
                {
                    isSucceed = false;
                    retryTime++;
                    if (retryTime == HttpPostRetryTimes)
                        throw;
                }
            }

            return result;
        }

        public async static Task PutAsBodyAsync<TRequest>(this HttpClient httpClient, string requestUrl, TRequest requestData, string token = null)
        {
            httpClient.AddToken(token);

            var response = await httpClient.PutAsJsonAsync(requestUrl, requestData);

            var retryTime = 0;

            while (!response.IsSuccessStatusCode && retryTime < HttpPostRetryTimes)
            {
                response = await httpClient.PutAsJsonAsync(requestUrl, requestData);
                retryTime++;
            }
        }

        public async static Task<TResult> PostAsFormAsync<TResult>(this HttpClient httpClient, string requestUrl, List<KeyValuePair<string, string>> formData, string token = null, List<KeyValuePair<string, (string fileName, string contentType, byte[] content)>> formFiles = null)
        {
            httpClient.AddToken(token);
            
            var requestContent = new MultipartFormDataContent();
            formData.ForEach(item =>
            {
                requestContent.Add(new StringContent(item.Value), item.Key);
            });

            formFiles?.ForEach(file =>
            {
                requestContent.Add(CreateFileContent(file.Key, file.Value.fileName, file.Value.contentType, file.Value.content));
            });

            var response = await httpClient.PostAsync(requestUrl, requestContent);

            var retryTime = 0;

            while (!response.IsSuccessStatusCode && retryTime < HttpPostRetryTimes)
            {
                response = await httpClient.PostAsync(requestUrl, requestContent);
                retryTime++;
            }

            return await response?.ToObjectAsync<TResult>();
        }

        public async static Task PostAsFormAsync(this HttpClient httpClient, string requestUrl, List<KeyValuePair<string, string>> formData, string token = null, List<KeyValuePair<string, (string fileName, string contentType, byte[] content)>> formFiles = null)
        {
            httpClient.AddToken(token);

            var requestContent = new MultipartFormDataContent();
            formData.ForEach(item =>
            {
                requestContent.Add(new StringContent(item.Value), item.Key);
            });

            formFiles?.ForEach(file =>
            {
                requestContent.Add(CreateFileContent(file.Key, file.Value.fileName, file.Value.contentType, file.Value.content));
            });

            var response = await httpClient.PostAsync(requestUrl, requestContent);

            var retryTime = 0;

            while (!response.IsSuccessStatusCode && retryTime < HttpPostRetryTimes)
            {
                response = await httpClient.PostAsync(requestUrl, requestContent);
                retryTime++;
            }
        }

        public async static Task<TResult> PutAsFormAsync<TResult>(this HttpClient httpClient, string requestUrl, List<KeyValuePair<string, string>> formData, string token = null)
        {
            httpClient.AddToken(token);

            var formContent = new FormUrlEncodedContent(formData.ToArray());
            var response = await httpClient.PutAsync(requestUrl, formContent);

            var retryTime = 0;

            while (!response.IsSuccessStatusCode && retryTime < HttpPostRetryTimes)
            {
                response = await httpClient.PutAsync(requestUrl, formContent);
                retryTime++;
            }

            return await response?.ToObjectAsync<TResult>();
        }

        public async static Task<TResult> GetAsync<TResult>(this HttpClient httpClient, string requestUrl, Dictionary<string, string> queryString = null, string token = null)
        {
            httpClient.AddToken(token);

            string query = string.Empty;

            if (queryString != null && queryString.Count > 0)
            {
                query = QueryString.Create(queryString).ToUriComponent();
            }

            var response = await httpClient.GetAsync($"{requestUrl}{query}");

            return await response?.ToObjectAsync<TResult>();
        }

        public async static Task<byte[]> DownloadAsync(this HttpClient httpClient, string requestUrl, Dictionary<string, string> queryString = null, string token = null)
        {
            httpClient.AddToken(token);

            string query = string.Empty;

            if (queryString != null && queryString.Count > 0)
            {
                query = QueryString.Create(queryString).ToUriComponent();
            }

            var response = await httpClient.GetAsync($"{requestUrl}{query}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<ErrorViewModel>();

                throw new HttpRequestException(error?.Message);
            }
        }

        private async static Task<TResult> ToObjectAsync<TResult>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(content);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorViewModel>(content);

                throw new HttpRequestException(error?.Message);
            }
        }

        public static void AddToken(this HttpClient httpClient, string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private static ByteArrayContent CreateFileContent(string name, string fileName, string contentType, byte[] content)
        {
            var fileContent = new ByteArrayContent(content, 0, content.Length);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"" + name + "\"",
                FileName = "\"" + fileName + "\""
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? "application/json");
            return fileContent;
        }
    }
}
