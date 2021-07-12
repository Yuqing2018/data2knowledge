using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IWorkspaceService
    {
        Task<Tuple<long, IEnumerable<WorkspaceServiceModel>>> GetWorkspacesAsync(LanguageEnum? language = null, string userId = null, int from = 0, int? size = null);

        Task<WorkspaceServiceModel> GetWorkspaceAsync(string id);

        Task<string> CreateWorkspaceAsync(WorkspaceCreateServiceModel serviceModel);

        Task UpdateWorkspaceAsync(string id, WorkspaceUpdateServiceModel serviceModel);

        Task DeleteWorkspaceAsync(string id);
    }
}
