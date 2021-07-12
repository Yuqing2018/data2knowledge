using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IWorkspaceTypeService
    {
        Task<Tuple<long, IEnumerable<WorkspaceTypeServiceModel>>> GetWorkspaceTypesAsync(int from, int? size, WorkspaceTypeStatusEnum? status = WorkspaceTypeStatusEnum.Enabled);

        Task<WorkspaceTypeServiceModel> GetWorkspaceTypeAsync(string id);

        Task<WorkspaceTypeServiceModel> CreateWorkspaceTypeAsync(WorkspaceTypeCreateServiceModel serviceModel);

        Task<WorkspaceTypeServiceModel> UpdateWorkspaceTypeAsync(string id, WorkspaceTypeUpdateServiceModel serviceModel);
    }
}
