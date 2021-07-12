using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Models.Overall;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    [Route("api/honda/v1/vehiclefault/overall")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class OverallController : ControllerBase
    {
        private readonly ILogger<OverallController> logger;
        private readonly IOverallMetricsService overallMetricsService;

        /// <summary>
        /// over all struct.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="overallMetricsService"></param>
        public OverallController(ILogger<OverallController> logger,
            IOverallMetricsService overallMetricsService)
        {
            this.logger = logger;
            this.overallMetricsService = overallMetricsService;
        }

        /// <summary>
        ///  各渠道近一周的不良件数统计
        /// </summary>
        /// <param name="datasource">渠道</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="frequency">统计时间类型</param>
        /// <returns></returns>
        [HttpGet]
        [Route("overall/vehiclefault/lastweek")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<MonthMetric>), (int)HttpStatusCode.OK)]
        public async Task<List<MonthMetric>> LastWeekFaultAsync(
            [Required] DataSource datasource,
            [Required] DateTime start, 
            [Required] DateTime end,
            [Required] StatisticalFrequency frequency)
        {
            var duration = end - start;

            switch (frequency)
            {
                case StatisticalFrequency.日:
                    if (duration.TotalDays > ConstantSettings.OverallMaxQueryDaysByDay)
                        ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.ExceedOverallMaxDays, HttpStatusCode.BadRequest);
                    break;
                case StatisticalFrequency.月:
                    if (duration.TotalDays > ConstantSettings.OverallMaxQueryDaysByMonth)
                        ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.ExceedOverallMaxMonth, HttpStatusCode.BadRequest);
                    break;
                case StatisticalFrequency.周:
                    if (duration.TotalDays > ConstantSettings.OverallMaxQueryDaysByWeek)
                        ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.ExceedOverallMaxWeek, HttpStatusCode.BadRequest);
                    break;
            }

            var results = await overallMetricsService.GetVehicleFaultMetrics(start, end, datasource, frequency);

            return results;
        }

        /// <summary>
        /// 各渠道的Top10 统计
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("overall/vehiclefault/top")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<TopFaultCountMetricsModel>), (int)HttpStatusCode.OK)]
        public async Task<List<TopFaultCountMetricsModel>> TopMetricsAsync(
            DateTime? start,
            DateTime? end)
        {
            var results = await overallMetricsService.GetLastMonthTopMetrics(start, end);
            return results;
        }

        /// <summary>
        /// 重点关注项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("overall/warning/focus")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<FocusedProjectMetricsModel>), (int)HttpStatusCode.OK)]
        public async Task<List<FocusedProjectMetricsModel>> FocusedMetricsAsync()
        {
            var results = await overallMetricsService.GetFocusedProjectList();
            return results;
        }

        /// <summary>
        /// 获取近一周预警统计
        /// </summary>
        /// <param name="start">可选</param>
        /// <param name="end">可选</param>
        /// <returns></returns>
        [HttpGet]
        [Route("overall/warning/metrics")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dictionary<string, List<DayMetric>>), (int)HttpStatusCode.OK)]
        public async Task<Dictionary<string, List<DayMetric>>> LastWeekWarningMetrics(DateTime? start,
            DateTime? end)
        {
            var results = await overallMetricsService.GetLastWeekWarningMetrics(start, end);
            return results;
        }

        /// <summary>
        /// 根据具体的预警类型获取近一周统计数
        /// </summary>
        /// <param name="warningType">预警类型</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("overall/warning/bywarningtype")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dictionary<string, List<DayMetric>>), (int)HttpStatusCode.OK)]
        public async Task<Dictionary<string, List<DayMetric>>> LastWeekWarningMetricsByWarningType(WarningType warningType, 
            DateTime? start,
            DateTime? end)
        {
            var results = await overallMetricsService.GetLastWeekWarningMetricsByCarType(start, end, warningType);
            return results;
        }

    }
}
