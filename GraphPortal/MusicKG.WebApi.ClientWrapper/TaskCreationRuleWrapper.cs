using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.ClientWrapper.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicKG.WebApi.ClientWrapper
{
    public static class TaskCreationRuleWrapper
    {
        public static async Task<TaskCreationRuleViewModel> GetAsync(HttpClient httpClient, string url, string workspaceId, string token)
        {
            var taskCreationUrl = $"{url}/api/Workspace/{workspaceId}/TaskCreationRule";

            var result = await httpClient.GetAsync<TaskCreationRuleViewModel>(taskCreationUrl, token: token);

            return result;
        }
    }
}
