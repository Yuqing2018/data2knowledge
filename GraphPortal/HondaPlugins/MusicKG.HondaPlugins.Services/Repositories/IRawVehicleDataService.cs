using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IRawVehicleDataService
    {
        Task<List<string>> ListCarModelsAsync();

        Task<List<string>> ListCarTypesAsync(List<string> carModel = null);

        Task<List<string>> ListModelYearsAsync(List<string> carModel = null, List<string> carType = null);
    }
}
