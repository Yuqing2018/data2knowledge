using MusicKG.Service.Models;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface ITaskCreationRuleService
    {
        Task<TaskCreationRuleServiceModel> GetTaskCreationRuleAsync(string workspaceId);

        Task UpdateRuleAsync(string workspaceId, TaskCreationRuleUpdateServiceModel updateModel);
    }
}
