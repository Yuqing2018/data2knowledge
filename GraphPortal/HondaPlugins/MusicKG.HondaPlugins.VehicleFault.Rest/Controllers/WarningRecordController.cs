using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models;
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
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Warning Record Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/warningrecord")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public partial class WarningRecordController : ControllerBase
    {
        private readonly ILogger<WarningRecordController> logger;
        private readonly IWarningRecordService warningRecordService;

        /// <summary>
        /// Warning record controller construct function.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="warningRecordService"></param>
        public WarningRecordController(ILogger<WarningRecordController> logger,
                IWarningRecordService warningRecordService)
        {
            this.logger = logger;
            this.warningRecordService = warningRecordService;
        }

        /// <summary>
        /// 预警详情列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="detailId"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<WarningRecordServiceModel>), StatusCodes.Status200OK)]
        public async Task<PaginationViewModel<WarningRecordServiceModel>> GetWarningRecords(
            ProcessStatus? status,
            [Required] string detailId, 
            int from, int? size)
        {
            var (totalCount, datas) = await warningRecordService.GetPageListAsync(status, detailId, from, size);

            return new PaginationViewModel<WarningRecordServiceModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = datas.Count(),
                Items = datas.ToList()
            };
        }

        /// <summary>
        /// 确认预警记录
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{recordId}/confirm")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> ConfirmRecord(
            [FromRoute][Required] string recordId, TaskConfirmRecordBindingModel binding)
        {
            if(string.IsNullOrWhiteSpace(binding.ConfirmedMessage) && !binding.PushStatus.HasValue && !binding.PermanentCntrStatus.HasValue && binding.IsExcessive.HasValue)
            {
                logger.LogError(MusicKGHondaPluginsMessage.ConfirmRecordCannotBeNull);
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.ConfirmRecordCannotBeNull, HttpStatusCode.BadRequest);
            }

            var userName = this.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

            var confirm = new WarningConfirmRecordDataModel()
            {
                ConfirmedMessage = binding.ConfirmedMessage,
                PushStatus = binding.PushStatus,
                PermanentCntrStatus = binding.PermanentCntrStatus,
                IsExcessive = binding.IsExcessive,
                LastConfirmdTime = DateTime.UtcNow,
                LastConfirmdUser = userName
            };

            var result = await warningRecordService.UpdateConfirmRecord(recordId, confirm);
            return Ok(result);
        }

        /// <summary>
        /// 更新AI风险等级
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{recordId}/aiRiskLevel")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateAIRiskLevel(
            [FromRoute][Required] string recordId, TaskRecordAIRiskLevelBindingModel binding)
        {
            var result = await warningRecordService.UpdateAIRiskLevel(recordId, binding.AIRiskLevel, binding.UsedForModel);
            return Ok(result);
        }
        /// <summary>
        /// 获取风险等级
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="carType"></param>
        /// <param name="yearModels"></param>
        /// <param name="partName"></param>
        /// <param name="syndrome"></param>
        /// <param name="warningUnit">预警单元</param>
        /// <returns></returns>
        [HttpGet]
        [Route("riskLevel")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<string> GetRiskLevel(
            [FromQuery][Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome,
            [FromQuery] WarningUnit warningUnit)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels,
                string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName },
                string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome });
            var result = await warningRecordService.GetRiskLevel(searchModel, warningUnit);
            return result.Item1.HasValue ? result.Item1.Value.ToString() : "";
        }

        /// <summary>
        /// 获取确认记录列表
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="carType"></param>
        /// <param name="yearModels"></param>
        /// <param name="partName"></param>
        /// <param name="syndrome"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recordList")]
        [ProducesResponseType(typeof(List<WarningTaskConfirmRecordServiceModel>), StatusCodes.Status200OK)]
        public async Task<List<WarningTaskConfirmRecordServiceModel>> GetConfirmRecordList(
            [FromQuery][Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels,
                string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName },
                string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome });
            var result = await warningRecordService.ListConfirmRecordAsync(searchModel);
            return result;
        }

        /// <summary>
        /// 导出确认记录
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="carType"></param>
        /// <param name="yearModels"></param>
        /// <param name="partName"></param>
        /// <param name="syndrome"></param>
        /// <returns></returns>
        [HttpGet("confirm/report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Export(
            [FromQuery][Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels,
                string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName },
                string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome });
            var table = await warningRecordService.ExportAsync(searchModel);
            var excelsheets = new List<ExcelSheet>()
            {
                new ExcelSheet()
                {
                    SheetName = "确认记录",
                    Table = table,
                    MergeCells = null,
                    AddFilter = true
                }
            };
            var content = ExcelHelper.ConvertTableToExcelBytes(excelsheets);

            var nowTimeStr = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var fileName = FormattableString.Invariant($@"确认记录_{nowTimeStr}.xlsx");
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content, contentType, fileName);
        }

        /// <summary>
        /// 获取 warning task email messages.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("emailMessages")]
        [ProducesResponseType(typeof(List<WarningMessageServiceModel>), StatusCodes.Status200OK)]
        public async Task<List<WarningMessageServiceModel>> GetEmailMessages()
        {
            var result = await warningRecordService.MessagesToSend();
            return result;
        }
    }
}
