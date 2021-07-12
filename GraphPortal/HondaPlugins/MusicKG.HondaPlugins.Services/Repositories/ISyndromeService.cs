using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface ISyndromeService
    {
        Task<List<SyndromeServiceModel>> GetSameSyndromeAsync(List<string> syndromeId);

        Task<List<SyndromeServiceModel>> GetDistinctSyndromeAsync(List<string> syndromeIds);

        Task LinkSyndromeAsync<T>(List<T> data) where T : VehicleFaultListServiceModel;

        Task<bool> ExistsAsync(string syndromeId);

        Task<SyndromeDataModel> UpdateAsync(string id, string name, BadGrade? grade);

        Task<SyndromeDataModel> CreateAsync(string name, BadGrade grade);

        Task<Tuple<long, IEnumerable<SyndromeServiceModel>>> ListAsync(string keyword, int from, int? size);
    }
}
