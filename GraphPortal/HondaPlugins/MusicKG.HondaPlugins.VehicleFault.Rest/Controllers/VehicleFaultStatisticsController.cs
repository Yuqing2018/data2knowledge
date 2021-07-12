using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.Services.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Vehicle Fault Statistics Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/statistics")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public partial class VehicleFaultStatisticsController : ControllerBase
    {
        private readonly ILogger<VehicleFaultStatisticsController> logger;
        private readonly IVehicleFaultStatisticsService statisticsService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="statisticsService"></param>
        public VehicleFaultStatisticsController(ILogger<VehicleFaultStatisticsController> logger, IVehicleFaultStatisticsService statisticsService)
        {
            this.logger = logger;
            this.statisticsService = statisticsService;
        }

        /// <summary>
        /// 某一零件下不良症状统计
        /// </summary>
        /// <param name="carModel">车款.</param>
        /// <param name="carType">车型.</param>
        /// <param name="yearModels">年款</param>
        /// <param name="partName">零件名</param>
        /// <param name="datasource">渠道</param>
        /// <returns></returns>
        [HttpGet("syndrome")]
        [ProducesResponseType(typeof(List<CircularChartMetrics<SyndromeMetric>>), (int)HttpStatusCode.OK)]
        public async Task<List<CircularChartMetrics<SyndromeMetric>>> StatisticSyndromes(
            [FromQuery] [Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] List<DataSource> datasource)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels, string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName }, null, datasource);
            var syndromeMetrics = await statisticsService.GetSyndromeMetrics(searchModel);
            
            return syndromeMetrics;
        }

        /// <summary>
        /// 不良症状下零件统计
        /// </summary>
        /// <param name="carModel">车款.</param>
        /// <param name="carType">车型.</param>
        /// <param name="yearModels">年款，可多选</param>
        /// <param name="syndrome">不良症状</param>
        /// <param name="datasource"></param>
        /// <returns></returns>
        [HttpGet("partname")]
        [ProducesResponseType(typeof(List<PartNameMetric>), (int)HttpStatusCode.OK)]
        public async Task<List<PartNameMetric>> StatisticPartNames(
            [FromQuery] [Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string syndrome,
            [FromQuery] List<DataSource> datasource)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels, null, string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome }, datasource);
            var partNameMetrics = await statisticsService.GetPartNameMetrics(searchModel);
            return partNameMetrics;
        }

        /// <summary>
        /// 除扇形图外的其他统计图表（按月统计）
        /// </summary>
        /// <param name="carModel">车款.</param>
        /// <param name="carType">车型.</param>
        /// <param name="yearModels">年款，可多选</param>
        /// <param name="partName">零件名</param>
        /// <param name="syndrome">不良症状</param>
        /// <param name="datasource"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(StatisticServiceModel), (int)HttpStatusCode.OK)]
        public async Task<StatisticServiceModel> GetStatistics(
            [FromQuery] [Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome,
            [FromQuery] List<DataSource> datasource)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels, 
                string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName }, 
                string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome },
                datasource);
            var result = await statisticsService.CalculateMetrics(searchModel);
            return result;
        }

        /// <summary>
        /// 按日统计(生产,故障,经过,渠道)
        /// </summary>
        /// <param name="carModel">车款.</param>
        /// <param name="carType">车型.</param>
        /// <param name="yearModels">年款，可多选</param>
        /// <param name="partName">零件名</param>
        /// <param name="syndrome">不良症状</param>
        /// <param name="month">chartType 为经过，格式 eg."8个月",其他图表类型，格式"yyyy-MM"</param>
        /// <param name="chartType">图表类型</param>
        /// <param name="datasource">渠道</param>
        /// <returns></returns>
        [HttpGet("days")]
        [ProducesResponseType(typeof(List<DayMetric>), (int)HttpStatusCode.OK)]
        public async Task<List<DayMetric>> GetDayMetrics(
            [FromQuery] [Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome,
            [FromQuery] [Required] string month,
            [FromQuery] [Required] ChartType chartType,
            [FromQuery] List<DataSource> datasource)
        {
            if (chartType == ChartType.经过)
                ValidateMonthSpanFormat(month);
            else
                ValidateSpecificMonthFormat(month);
            var searchModel = new BaseSearchModel(carModel, carType, yearModels, 
                string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName },
                string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome }, datasource);
            var result = await statisticsService.MetricsInOneMonth(searchModel, month, chartType);
            return result;
        }

        /// <summary>
        /// 按经过时间不良率，按日统计
        /// </summary>
        /// <param name="carModel">车款.</param>
        /// <param name="carType">车型.</param>
        /// <param name="yearModels">年款，可多选</param>
        /// <param name="partName">零件名</param>
        /// <param name="syndrome">不良症状</param>
        /// <param name="month">具体某一月表示</param>
        /// <param name="datasource">渠道</param>
        /// <returns></returns>
        [HttpGet("defectrate/days")]
        [ProducesResponseType(typeof(Task<Dictionary<string, List<DayRateMetric>>>), (int)HttpStatusCode.OK)]
        public async Task<Dictionary<string, List<DayRateMetric>>> GetDayRateMetrics(
            [FromQuery] [Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome,
            [FromQuery] [Required] string month, 
            [FromQuery] List<DataSource> datasource)
        {
            ValidateMonthSpanFormat(month);
            var searchModel = new BaseSearchModel(carModel, carType, yearModels,
               string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName },
               string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome },
               datasource);
            var result = await statisticsService.RateMetricsInOneMonth(searchModel, month);
            return result;
        }
        /// <summary>
        /// 按对策时间的不良率，按日统计
        /// </summary>
        /// <param name="carModel">车款.</param>
        /// <param name="carType">车型.</param>
        /// <param name="yearModels">年款，可多选</param>
        /// <param name="partName">零件名</param>
        /// <param name="syndrome">不良症状</param>
        /// <param name="month">具体某一月表示</param>
        /// <param name="datasource"></param>
        /// <returns></returns>
        [HttpGet("cntrrate/days")]
        [ProducesResponseType(typeof(Task<Dictionary<string, List<DayRateMetric>>>), (int)HttpStatusCode.OK)]
        public async Task<Dictionary<string, List<DayRateMetric>>> GetDayCntrRateMetrics(
            [FromQuery][Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome,
            [FromQuery][Required] string month,
            [FromQuery] List<DataSource> datasource)
        {
            ValidateMonthSpanFormat(month);
            var searchModel = new BaseSearchModel(carModel, carType, yearModels,
                string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName },
                string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome },
                datasource);
            var result = await statisticsService.CntrRateMetricsInOneMonth(searchModel, month);
            return result;
        }

        private void ValidateMonthSpanFormat(string month)
        {
            if (!month.EndsWith("个月"))
            {
                var message = MusicKGHondaPluginsMessage.MonthFormatIsWrong;
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }
        private void ValidateSpecificMonthFormat(string month)
        {
            if (!DateTime.TryParse($"{month}-01", out _))
            {
                var message = $"{MusicKGHondaPluginsMessage.MonthFormatIsWrong},month:{month}";
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
        }
    }
}
