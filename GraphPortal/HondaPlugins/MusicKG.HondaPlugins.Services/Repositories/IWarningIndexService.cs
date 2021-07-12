using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IWarningIndexService
    {
        Task<List<WarningIndexDataModel>> GetListAsync(WarningType warningType);
        Task<List<WarningIndexDataModel>> SaveManyAsync(List<WarningIndexDataModel> datas);
    }
}
