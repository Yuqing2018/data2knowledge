using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Models.Overall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IOverallMetricsService
    {
        Task<List<TopFaultCountMetricsModel>> GetLastMonthTopMetrics(DateTime? start, DateTime? end);

        Task<List<MonthMetric>> GetVehicleFaultMetrics(DateTime? start, DateTime? end, DataSource datasource, StatisticalFrequency frequency);

        Task<List<FocusedProjectMetricsModel>> GetFocusedProjectList();

        Task<Dictionary<string, List<DayMetric>>> GetLastWeekWarningMetrics(DateTime? start, DateTime? end);

        Task<Dictionary<string, List<DayMetric>>> GetLastWeekWarningMetricsByCarType(DateTime? start, DateTime? end, WarningType warningType);
    }
}
