using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;

namespace MusicKG.Service
{
    public interface ITaskService
    {
        Task<string> CreateTaskAsync(TaskCreateServiceModel taskCreate);

        Task<Tuple<long, IEnumerable<TaskServiceModel>>> GetTasksAsync(string workspaceId, string annotatorId, string keyword, IEnumerable<TaskStatusEnum> statuses, int from, int? size, string taskType = null);

        Task<TaskServiceModel> GetTaskAsync(string workspaceId, string taskId, string annotatorId = null, bool isIncludingDocuments = true);

        Task UpdateTaskAsync(string workspaceId, string taskId, TaskUpdateServiceModel taskUpdate);

        Task ApproveTaskAsync(string workspaceId, string taskId);

        Task RejectTaskAsync(string workspaceId, string taskId, string annotatorId = null);

        Task HandleTaskResultAsync(string workspaceId, string taskId);

        Task DeleteTaskAsync(string workspaceId, string taskId);

        Task<IEnumerable<TaskDocumentResultServiceModel>> GetTaskDocumentResultAsync(string workspaceId, string taskId, string annotatorId, string documentId, TaskDocumentResultTypeEnum? resultType);

        Task SaveTaskDocumentResultAsync(string workspaceId, string taskId, string annotatorId, string documentId, TaskDocumentResultSaveServiceModel result);

        Task SaveBatchTaskDocumentResults(string workspaceId, string annotatorId, List<string> taskIdList);

        Task<List<string>> GetTaskResultsAsync(string workspaceId, string taskId, TaskDocumentResultTypeEnum? resultType);
    }
}
