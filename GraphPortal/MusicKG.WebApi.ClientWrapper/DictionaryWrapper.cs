using MusicKG.WebApi.ClientWrapper.Extensions;
using MusicKG.WebApi.Contract.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicKG.WebApi.ClientWrapper
{
    public class DictionaryWrapper
    {
        public static async Task UpdateVocabsAsync(HttpClient httpClient, string url, string workspaceId, string dictionaryId, IEnumerable<string> entries, string token)
        {
            var dictionaryUrl = $"{url}/api/Workspace/{workspaceId}/Dictionary/{dictionaryId}/UpdateVocabs";

            await httpClient.PutAsBodyAsync(dictionaryUrl, entries, token);
        }

        public static async Task<IEnumerable<DictionaryListItemViewModel>> ListAsync(HttpClient httpClient, string url, string workspaceId, string token)
        {
            var dictionaryUrl = $"{url}/api/Workspace/{workspaceId}/Dictionary";

            var result = await httpClient.GetAsync<PaginationViewModel<DictionaryListItemViewModel>>(dictionaryUrl, token: token);

            return result?.Items;
        }
    }
}
