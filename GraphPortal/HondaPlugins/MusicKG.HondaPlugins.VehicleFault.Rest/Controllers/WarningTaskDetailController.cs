using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Warning Track Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/warningtrack")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public partial class WarningTaskDetailController : ControllerBase
    {
        private readonly ILogger<WarningTaskDetailController> logger;
        private readonly IWarningTaskDetailService warningTaskDetailService;

        /// <summary>
        /// Warning TaskDetail controller construct function.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="warningTaskDetailService"></param>
        public WarningTaskDetailController(ILogger<WarningTaskDetailController> logger,
                IWarningTaskDetailService warningTaskDetailService)
        {
            this.logger = logger;
            this.warningTaskDetailService = warningTaskDetailService;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="warningUnit"></param>
        /// <param name="createdBy"></param>
        /// <param name="carModels"></param>
        /// <param name="carTypes"></param>
        /// <param name="yearModels"></param>
        /// <param name="partNames"></param>
        /// <param name="syndromes"></param>
        /// <param name="focusType"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<WarningTaskDetailServiceModel>), StatusCodes.Status200OK)]
        public async Task<PaginationViewModel<WarningTaskDetailServiceModel>> GetWarningTaskDetails(
            [FromQuery] List<string> taskIds,
            [FromQuery] WarningUnit? warningUnit,
            [FromQuery] string createdBy,
            [FromQuery] List<string> carModels,
            [FromQuery] List<string> carTypes,
            [FromQuery] List<string> yearModels,
            [FromQuery] List<string> partNames,
            [FromQuery] List<string> syndromes,
            [FromQuery] FocusType? focusType,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, datas) = await warningTaskDetailService.GetPageListAsync(taskIds, warningUnit, createdBy, carModels, carTypes, yearModels, partNames, syndromes, focusType, from, size);

            return new PaginationViewModel<WarningTaskDetailServiceModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = datas.Count(),
                Items = datas.ToList()
            };
        }

        /// <summary>
        /// 更新报警频率，是否重点关注
        /// </summary>
        /// <param name="id"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/frequency")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateFrequency(
            [FromRoute][Required] string id,
            TaskDetailUpdateBindingModel binding)
        {
            var result = await warningTaskDetailService.UpdateAsync(id, binding.Frequency, binding.IsFocused, binding.SpecifiedDate);
            return Ok(result);
        }

        /// <summary>
        /// 保存或更新跳转设置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/settings")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateSettings(
            [FromRoute][Required] string id,
            WarningTaskDetailSettingDataModel binding)
        {
            var result = await warningTaskDetailService.UpdateSettingsAsync(id, binding);
            return Ok(result);
        }
        /// <summary>
        ///获取单个预警跟踪信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(WarningTaskDetailServiceModel), StatusCodes.Status200OK)]
        public async Task<WarningTaskDetailServiceModel> GetAsync([FromRoute][Required] string id)
        {
            var result = await warningTaskDetailService.GetAsync(id);
            return result;
        }

        /// <summary>
        /// warning detail list export.
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="warningUnit"></param>
        /// <param name="createdBy"></param>
        /// <param name="carModels"></param>
        /// <param name="carTypes"></param>
        /// <param name="yearModels"></param>
        /// <param name="partNames"></param>
        /// <param name="syndromes"></param>
        /// <param name="focusType"></param>
        /// <returns></returns>
        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Export(
            [FromQuery] List<string> taskIds,
            [FromQuery] WarningUnit? warningUnit,
            [FromQuery] string createdBy,
            [FromQuery] List<string> carModels,
            [FromQuery] List<string> carTypes,
            [FromQuery] List<string> yearModels,
            [FromQuery] List<string> partNames,
            [FromQuery] List<string> syndromes,
            [FromQuery] FocusType? focusType)
        {
            var table = await warningTaskDetailService.ExportAsync(taskIds, warningUnit, createdBy, carModels, carTypes, yearModels, partNames, syndromes, focusType);
            var excelsheets = new List<ExcelSheet>()
            {
                new ExcelSheet()
                {
                    SheetName = "预警跟踪信息",
                    Table = table,
                    MergeCells = null,
                    AddFilter = true
                }
            };
            var content = ExcelHelper.ConvertTableToExcelBytes(excelsheets);

            var nowTimeStr = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var fileName = FormattableString.Invariant($@"预警跟踪_{nowTimeStr}.xlsx");
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content, contentType, fileName);
        }

        private string JoinString(List<string> data)
        {
            if (data == null || data.Count == 0)
                return "";

            return string.Join(",", data);
        }

        private string GetCellString(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return "";

            return data;
        }

        private List<string> GetMetrics(Dictionary<string, DataSourceMetricsDataModel> data, DataSource dataSource)
        {
            if (data == null || !data.ContainsKey(dataSource.ToString()))
                if (dataSource == DataSource.MQI)
                    return new List<string> { "", "", "" };
                else
                    return new List<string> { "", "" };

            var metrics = data[dataSource.ToString()];
            var result = new List<string>();

            result.Add(metrics.TotalCount.ToString());
            if (dataSource == DataSource.MQI)
                result.Add(metrics.DefectRate.ToString("P2"));
            result.Add(metrics.LastThreeMonthAscentRate.ToString("P2"));

            return result;
        }

        private string GetStringFromDouble(double? data, string format = null)
        {
            if (data == null || data.Value == 0d)
                return "";

            return format == null ? data.Value.ToString() : data.Value.ToString(format);
        }

        private string GetStringFromBoolean(bool? data)
        {
            if (data == null)
                return "";

            return data.Value ? "是" : "否";
        }

        private string GetString<T>(T data, string defaultValue = null)
        {
            if (defaultValue == null)
                defaultValue = "";

            if (data == null)
                return defaultValue;

            return data.ToString();
        }

        private List<(string cell1Name, string cell2Name)> GetMergeCellsOfTableHeader(List<List<string>> table)
        {
            var headerRow = table.First();

            var mergeCells_header = new List<(string cell1Name, string cell2Name)>();

            for (int i = 0; i < headerRow.Count; i++)
            {
                var col = i + 1;
                if (i == headerRow.IndexOf("MQI渠道") || i == headerRow.IndexOf("风险特征"))
                {
                    var cell1 = ExcelHelper.GetCellReference(0, col);
                    var cell2 = ExcelHelper.GetCellReference(0, col + 2);
                    mergeCells_header.Add((cell1, cell2));
                    i += 2;
                }
                else if (i == headerRow.IndexOf("技术咨询") || i == headerRow.IndexOf("800") || i == headerRow.IndexOf("总局") || i == headerRow.IndexOf("网络媒体"))
                {
                    var cell1 = ExcelHelper.GetCellReference(0, col);
                    var cell2 = ExcelHelper.GetCellReference(0, col + 1);
                    mergeCells_header.Add((cell1, cell2));
                    i += 1;
                }
                else if (i == headerRow.IndexOf("推进情况"))
                {
                    var cell1 = ExcelHelper.GetCellReference(0, col);
                    var cell2 = ExcelHelper.GetCellReference(0, col + 3);
                    mergeCells_header.Add((cell1, cell2));
                    i += 3;
                }
                else
                {
                    mergeCells_header.Add((ExcelHelper.GetCellReference(0, col), ExcelHelper.GetCellReference(1, col)));
                }
            }
            return mergeCells_header;
        }

        [HttpGet("riskreport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportRiskRecordList(
            [FromQuery] List<string> taskIds,
            [FromQuery] WarningUnit? warningUnit,
            [FromQuery] string createdBy,
            [FromQuery] List<string> carModels,
            [FromQuery] List<string> carTypes,
            [FromQuery] List<string> yearModels,
            [FromQuery] List<string> partNames,
            [FromQuery] List<string> syndromes,
            [FromQuery] FocusType? focusType)
        {
            var data = await warningTaskDetailService.ExportRiskRecordListAsync(taskIds, warningUnit, createdBy, carModels, carTypes, yearModels, partNames, syndromes, focusType);

            var table = new List<List<string>>();

            table.Add(new List<string> { "车款", "车型", "年款", "所属任务", "零件号", "零件名", "不良症状", "不良等级",
                "MQI渠道", "", "", "技术咨询", "", "800", "", "总局", "", "网络媒体", "",
                "推进情况", "", "", "", "风险特征", "", "",
                "风险得分", "风险等级（计算）", "风险等级（AI）", "最近一次确认日期", "确认记录"});

            table.Add(new List<string> { "车款", "车型", "年款", "所属任务", "零件号", "零件名", "不良症状", "不良等级",
                "件数", "不良率", "近3个月上升率", "件数", "近3个月上升率", "件数", "近3个月上升率", "件数", "近3个月上升率", "件数", "近3个月上升率",
                "推进分类", "不良原因", "对策内容", "对策时间", "保修金额", "超标影响", "对策状态",
                "风险得分", "风险等级（计算）", "风险等级（AI）", "最近一次确认日期", "确认记录"});

            var contentTable = data?.Select(d =>
            {
                var line = new List<string> { JoinString(d.CarModels), JoinString(d.CarTypes), JoinString(d.YearModels),
                    GetCellString(d.TaskName), GetCellString(d.PartNo), GetCellString(d.PartName), GetCellString(d.Syndrome), GetCellString(d.BadGrade.ToString()) };

                line.AddRange(GetMetrics(d.RiskMetrics.DataSourceMetrics, DataSource.MQI));
                line.AddRange(GetMetrics(d.RiskMetrics.DataSourceMetrics, DataSource.TECH_CONSULTING));
                line.AddRange(GetMetrics(d.RiskMetrics.DataSourceMetrics, DataSource.HOTLINE));
                line.AddRange(GetMetrics(d.RiskMetrics.DataSourceMetrics, DataSource.GOV));
                line.AddRange(GetMetrics(d.RiskMetrics.DataSourceMetrics, DataSource.MEDIA_MAIN));
                line.Add(GetString(d.ConfirmRecord?.PushStatus ?? d.RiskMetrics?.PushStatus));
                line.Add(GetString(d.RiskMetrics?.CntrMesrReasonDesc));
                line.Add(GetString(d.RiskMetrics?.PermanentCntr));
                line.Add(GetString(d.RiskMetrics?.PermanentCntrTime));
                line.Add(GetStringFromDouble(d.RiskMetrics?.MeanCostRepair, "f3"));
                line.Add(GetStringFromBoolean(d.ConfirmRecord?.IsExcessive));
                line.Add(GetString(d.ConfirmRecord?.PermanentCntrStatus));
                line.Add(GetStringFromDouble(d.RiskMetrics?.RiskScore, "f3"));
                line.Add(GetString(d.RiskMetrics?.RiskLevel));
                line.Add(GetString(d.RiskMetrics?.AIRiskLevel));
                line.Add(GetString(d.ConfirmRecord?.LastConfirmdTime));
                line.Add(GetString(d.ConfirmRecord?.ConfirmedMessage));

                return line;
            })?.ToList();

            if (contentTable != null && contentTable.Count > 0)
                table.AddRange(contentTable);

            var excelSheets = new List<ExcelSheet>
            {
                new ExcelSheet
                {
                    SheetName = "风险台账",
                    Table = table,
                    AddFilter = true,
                    MergeCells = GetMergeCellsOfTableHeader(table)
                }
            };

            var content = ExcelHelper.ConvertTableToExcelBytes(excelSheets);

            var nowTimeStr = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var fileName = FormattableString.Invariant($@"风险台账_{nowTimeStr}.xlsx");
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content, contentType, fileName);
        }

        /// <summary>
        /// 获取当前用户的待处理报警记录数
        /// </summary>
        /// <returns></returns>
        [HttpGet("warningCount")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<int> WarningCountAsync()
        {
            var userName = this.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var count = await warningTaskDetailService.UnhandledCountAsync(userName);
            return count;
        }
    }
}
