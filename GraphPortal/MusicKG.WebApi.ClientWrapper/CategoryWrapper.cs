using MusicKG.WebApi.ClientWrapper.Extensions;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicKG.WebApi.ClientWrapper
{
    public class CategoryWrapper
    {
        public static async Task<CategoryViewModel> CreateAsync(HttpClient httpClient, string url, string workspaceId, string categoryId, string name, string token)
        {
            var taskUrl = $"{url}/api/Workspace/{workspaceId}/Category/One";

            var result = await httpClient.PostAsBodyAsync<CategoryBindingModel, CategoryViewModel>(taskUrl, new CategoryBindingModel() { Id = categoryId, Name = name }, token);

            return result;
        }

        public static async Task<CategoryViewModel> UpdateAsync(HttpClient httpClient, string url, string workspaceId, string categoryId, string name, string token)
        {
            var taskUrl = $"{url}/api/Workspace/{workspaceId}/Category/{categoryId}";
            var formData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("name", name) };
            var result = await httpClient.PutAsFormAsync<CategoryViewModel>(taskUrl, formData, token);

            return result;
        }

    }
}
