using MusicKG.HondaPlugins.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IRawSalesService
    {
        Task<long> GetSalesCount(List<string> carModels, List<string> carTypes, List<string> yearModels, string frameNo, DateTime? start, DateTime? end);
        
        Task<DateTime> GetSalesInitRegisterDate(List<string> carModels, List<string> carTypes, List<string> yearModels, string frameNo, DateTime? start, DateTime? end);

        Task<List<RawSalesDataServiceModel>> GetSalesDatas(List<string> carModels, List<string> carTypes, List<string> yearModels, string frameNo, DateTime? start, DateTime? end);
    }
}
