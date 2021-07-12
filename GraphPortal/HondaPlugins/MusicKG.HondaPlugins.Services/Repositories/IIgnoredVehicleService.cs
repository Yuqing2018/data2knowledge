using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IIgnoredVehicleService
    {
        Task<Dictionary<string, List<string>>> ListAsync(string keyword);

        Task AddAsync(string carModel, List<string> carTypes);

        Task UpdateAsync(string carModel, List<string> carTypes);

        Task DeleteAsync(string carModel);
    }
}
