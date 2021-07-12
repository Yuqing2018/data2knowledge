using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IStatisticService
    {
        Task<IEnumerable<StatisticsOverviewServiceModel>> GetStatisticsOverviewAsync(DateTime fromDate, DateTime toDate, string workspaceTypeId);

        Task<StatisticsDetailsServiceModel> GetStatisticsDetailsAsync(string userId, DateTime fromDate, DateTime toDate, string workspaceTypeId);
    }
}
