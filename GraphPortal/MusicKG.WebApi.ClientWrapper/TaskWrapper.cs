using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.ClientWrapper.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MusicKG.DataAccess.Enums;
using System;
using System.Linq;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.ClientWrapper
{
    public class TaskWrapper
    {
        public static async Task<string> CreateAsync(HttpClient httpClient, string url, string workspaceId, TaskCreateBindingModel bindingModel, string token)
        {
            var taskUrl = $"{url}/api/Workspace/{workspaceId}/Task";

            var result = await httpClient.PostAsBodyAsync<TaskCreateBindingModel, string>(taskUrl, bindingModel, token);

            return result;
        }

        public static async Task<List<string>> ListAsync(HttpClient httpClient, string url, string workspaceId, TaskStatusEnum? status, string token)
        {
            var taskUrl = $"{url}/api/Workspace/{workspaceId}/Task";

            Dictionary<string, string> query = null;

            if (status != null)
            {
                query = new Dictionary<string, string>
                {
                    { "statuses", status.Value.ToString() }
                };
            }

            var result = await httpClient.GetAsync<PaginationViewModel<TaskListItemViewModel>>(taskUrl, query, token);

            return result?.Items?.Select(i => i.Id).ToList();
        }

        public static async Task<List<string>> GetTaskResultsAsync(HttpClient httpClient, string url, string workspaceId, string taskId, string token, TaskDocumentResultTypeEnum? resultType)
        {
            var taskResultUrl = $"{url}/api/Workspace/{workspaceId}/Task/{taskId}/Results";

            var query = resultType.HasValue ? new Dictionary<string, string> { { "resultType", resultType.Value.ToString() } } : null;

            return await httpClient.GetAsync<List<string>>(taskResultUrl, query, token);
        }

        public static async Task HandleResultAsync(HttpClient httpClient, string url, string workspaceId, string taskId, string token)
        {
            var taskResultHandleUrl = $"{url}/api/Workspace/{workspaceId}/Task/{taskId}/Results/Merge";

            var result = await httpClient.PostAsync(taskResultHandleUrl, null);

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException($"Handle result of task {taskId} failed, details: {result.Content?.ReadAsStringAsync()?.Result}");
        }

        public static async Task DeleteAsync(HttpClient httpClient, string url, string workspaceId, string taskId, string token)
        {
            var deleteUrl = $"{url}/api/Workspace/{workspaceId}/Task/{taskId}";

            httpClient.AddToken(token);

            var result = await httpClient.DeleteAsync(deleteUrl);

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException("Delete task failed.");
        }
    }
}
