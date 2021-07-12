using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes;
using MusicKG.HondaPlugins.DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using System.Globalization;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Vehiclefault Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/data")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class VehicleFaultDataController : ControllerBase
    {
        private readonly ILogger<VehicleFaultDataController> logger;
        private readonly IVehicleFaultDataService vehicleFaultDataService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="vehicleFaultDataService"></param>
        public VehicleFaultDataController(ILogger<VehicleFaultDataController> logger, IVehicleFaultDataService vehicleFaultDataService)
        {
            this.logger = logger;
            this.vehicleFaultDataService = vehicleFaultDataService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<VehicleFaultListViewModel>), (int)HttpStatusCode.OK)]
        public async Task<PaginationViewModel<VehicleFaultListViewModel>> ListAsync(
            [FromQuery] List<string> carModel = null,
            [FromQuery] List<DataSource> dataSource = null,
            [FromQuery] List<string> carType = null,
            [FromQuery] List<string> yearModels = null,
            [FromQuery] List<string> partName = null,
            [FromQuery] List<string> syndrome = null,
            [FromQuery] DateTime? faultDateStart = null,
            [FromQuery] DateTime? faultDateEnd = null,
            [FromQuery] int from = 0,
            [FromQuery] int? size = null,
            [FromQuery] int maxSize = 500)
        {
            if (dataSource != null && dataSource.Contains(DataSource.MEDIA_SUB) && !dataSource.Contains(DataSource.MEDIA_MAIN))
            {
                dataSource.Add(DataSource.MEDIA_MAIN);
            }

            var (totalCount, data) = await vehicleFaultDataService.ListAsync(
                carModel, dataSource, carType, yearModels, partName, syndrome, faultDateStart, faultDateEnd, from, size, maxSize);

            return new PaginationViewModel<VehicleFaultListViewModel>
            {
                From = from,
                Count = data.Count(),
                TotalCount = totalCount,
                Items = data.Select(d => new VehicleFaultListViewModel
                {
                    Id = d.RawId,
                    CarModel = d.CarModel,
                    CarType = d.CarType,
                    DataSource = d.DataSource == DataSource.MEDIA_MAIN ? DataSource.MEDIA_SUB.ToString() : d.DataSource.ToString(),
                    FaultDate = d.FaultDate,
                    PartName = d.PartName,
                    PartNo = d.PartNo,
                    Syndrome = d.Syndrome,
                    SyndromeName = d.SyndromeModel.Name,
                    YearModel = d.ModelYear
                })
            };
        }

        [HttpGet("{rawId}")]
        [ProducesResponseType(typeof(Dictionary<string, string>), (int)HttpStatusCode.OK)]
        public async Task<Dictionary<string, string>> DetailAsync(
            [FromRoute] [Required] string rawId)
        {
            return await vehicleFaultDataService.DetailAsync(rawId);
        }

        [HttpPut("{rawId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task UpdateAsync(
            [FromRoute] string rawId, 
            [FromBody][Required] VehicleFaultUpdateBindingModel bindingModel)
        {
            await vehicleFaultDataService.UpdateAsync(rawId, bindingModel.PartName, bindingModel.Syndrome, bindingModel.IsAddToTraining);
        }

        [HttpGet("carmodels")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<List<string>> ListCarModelsAsync(
            [FromQuery] DataSource? dataSource = null)
        {
            return await vehicleFaultDataService.ListCarModelsAsync(dataSource);
        }

        [HttpGet("carmodels/cartypes")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<List<string>> ListCarTypeAsync(
            [FromQuery] [Required] List<string> carModel,
            [FromQuery] DataSource? dataSource = null)
        {
            return await vehicleFaultDataService.ListCarTypesAsync(dataSource, carModel);
        }

        [HttpGet("carmodels/modelyears")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<List<string>> ListModelYearsAsync(
            [FromQuery] [Required] List<string> carModel,
            [FromQuery] List<string> carType = null,
            [FromQuery] DataSource? dataSource = null)
        {
            return await vehicleFaultDataService.ListModelYearsAsync(dataSource, carModel, carType);
        }

        [HttpGet("carmodels/partnames")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<List<string>> ListPartNamesAsync(
            [FromQuery] List<string> carModel,
            [FromQuery] List<string> carType = null,
            [FromQuery] List<string> modelYears = null,
            [FromQuery] DataSource? dataSource = null,
            [FromQuery] List<string> syndrome = null)
        {
            return await vehicleFaultDataService.ListPartNamesAsync(dataSource, carModel, carType, modelYears, syndrome);
        }

        [HttpGet("carmodels/syndromes")]
        [ProducesResponseType(typeof(List<SyndromeListViewModel>), (int)HttpStatusCode.OK)]
        public async Task<List<SyndromeListViewModel>> ListSyndromesAsync(
            [FromQuery] List<string> carModel,
            [FromQuery] List<string> carType = null,
            [FromQuery] List<string> modelYears = null,
            [FromQuery] DataSource? dataSource = null,
            [FromQuery] List<string> partName = null)
        {
            var result = await vehicleFaultDataService.ListSyndromesAsync(dataSource, carModel, carType, modelYears, partName);
             
            return result?.Select(r => new SyndromeListViewModel
            {
                Id = r.Id,
                Name = r.Name
            })?.ToList();
        }

        /// <summary>
        /// 源数据导出
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="dataSource"></param>
        /// <param name="carType"></param>
        /// <param name="yearModels"></param>
        /// <param name="partName"></param>
        /// <param name="syndrome"></param>
        /// <param name="faultDateStart"></param>
        /// <param name="faultDateEnd"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        [HttpGet("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Export(
            [FromQuery] List<string> carModel = null,
            [FromQuery] List<DataSource> dataSource = null,
            [FromQuery] List<string> carType = null,
            [FromQuery] List<string> yearModels = null,
            [FromQuery] List<string> partName = null,
            [FromQuery] List<string> syndrome = null,
            [FromQuery] DateTime? faultDateStart = null,
            [FromQuery] DateTime? faultDateEnd = null,
            [FromQuery] int maxCount = 60000)
        {
            var tableDict = await vehicleFaultDataService.ExportAsync(carModel, dataSource, carType, yearModels, partName, syndrome, faultDateStart, faultDateEnd, maxCount);
            var excelsheets = tableDict.Select(table => new ExcelSheet()
            {
                SheetName = table.Key.ToString(),
                Table = table.Value,
                MergeCells = null,
                AddFilter = true
            }).ToList();
            var content = ExcelHelper.ConvertTableToExcelBytes(excelsheets);

            var nowTimeStr = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var fileName = FormattableString.Invariant($@"源数据_{nowTimeStr}.xlsx");
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content, contentType, fileName);
        }
        #region QIS信息维护
        /// <summary>
        /// 获取零件的QIS信息列表
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="carType"></param>
        /// <param name="modelYears"></param>
        /// <param name="dataSource"></param>
        /// <param name="partName"></param>
        /// <param name="frameNo"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("qis/list")]
        [ProducesResponseType(typeof(PaginationViewModel<VehicleFaultQISInfoServiceModel>), (int)HttpStatusCode.OK)]
        public async Task<PaginationViewModel<VehicleFaultQISInfoServiceModel>> ListQisInfoAsync(
            [FromQuery] List<string> carModel = null,
            [FromQuery] List<string> carType = null,
            [FromQuery] List<string> modelYears = null,
            [FromQuery] List<DataSource> dataSource = null,
            [FromQuery] List<string> partName = null, 
            [FromQuery] string frameNo = null,
            [FromQuery] int from = 0,
            [FromQuery] int? size = null)
        {
            var (totalCount, datas) = await vehicleFaultDataService.ListQisAsync(dataSource, carModel, carType, modelYears, partName, frameNo, from, size);

            return  new PaginationViewModel<VehicleFaultQISInfoServiceModel>
            {
                From = from,
                Count = datas.Count(),
                TotalCount = totalCount,
                Items = datas
            };
        }


        /// <summary>
        /// 人工添加/修改
        /// </summary>
        /// <param name="rawId"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPut("qis/{rawId}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<bool> UpdateQISAsync(
           [FromRoute][Required] string rawId,
           [FromBody][Required] VehicleFaultQISUpdateBindingModel binding)
        {
            var flag = !binding.CntrMesrType.HasValue
                && string.IsNullOrWhiteSpace(binding.CntrMesrReasonDesc)
                && string.IsNullOrWhiteSpace(binding.PermanentCntr);

            if (flag)
            {
                logger.LogError("the qis info to update can not be null or empty!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.VehicleFaultQISInfoToUpdateCannotBeNull, HttpStatusCode.BadRequest);
            }

            var update = new VehicleFaultRelatedDataModel()
            {
                CntrMesrType = binding.CntrMesrType,
                CntrMesrReasonDesc = binding.CntrMesrReasonDesc,
                PermanentCntr = binding.PermanentCntr,
                PermanentCntrTime = binding.PermanentCntrTime
            };

            var result = await vehicleFaultDataService.UpdateQISAsync(rawId, update);

            return result;
        }
        
        /// <summary>
        /// qis自动关联
        /// </summary>
        /// <param name="rawId"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPost("qis/{rawId}/link")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<bool> AutoLinkQIS(
            [FromRoute][Required] string rawId,
            [FromBody] VehicleFaultQISUpdateBindingModel binding)
        {
            if (string.IsNullOrWhiteSpace(binding.QICNo) && string.IsNullOrWhiteSpace(binding.QISNo))
            {
                logger.LogError("the qis info to update can not be null or empty!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.VehicleFaultQISInfoToUpdateCannotBeNull, HttpStatusCode.BadRequest);
            }

            var result = await vehicleFaultDataService.AutoLinkQISAsync(rawId, binding.QICNo, binding.QISNo);

            return result;
        }

        /// <summary>
        /// 更新对策时间
        /// </summary>
        /// <param name="rawId"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPut("qis/{rawId}/cntrTime")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<bool> UpdateCntrTime(
            [FromRoute][Required] string rawId,
            [FromBody] VehicleFaultQISUpdateBindingModel binding)
        {
            if (!binding.PermanentCntrTime.HasValue)
            {
                logger.LogError("the qis info to update can not be null or empty!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.VehicleFaultQISInfoToUpdateCannotBeNull, HttpStatusCode.BadRequest);
            }
            var result = await vehicleFaultDataService.UpdateLastPermanentCntrTimeAsync(rawId, binding.PermanentCntrTime.Value);

            return result;
        }

        /// <summary>
        /// 删除qis信息
        /// </summary>
        /// <param name="rawId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("qis/{rawId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Delete([FromRoute][Required] string rawId)
        {
            await vehicleFaultDataService.DeleteRelatedInfo(rawId);
        }

        /// <summary>
        /// 修改零件关联信息
        /// </summary>
        /// <param name="rawId"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPut("relatedPartName/{rawId}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<bool> UpdateRelatedPartAsync(
            [FromRoute][Required] string rawId,
            [FromBody][Required] RelatedPartNameUpdateBindingModel binding)
        {
            var result = await vehicleFaultDataService.UpdateRelatedPartName(rawId, binding.CarType, binding.PartNo);

            return result;
        }

        /// <summary>
        /// 修改数据来源
        /// </summary>
        /// <param name="rawId"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPut("dataFrom/{rawId}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<bool> UpdateDataFromAsync([FromRoute][Required] string rawId, 
            [FromBody][Required] RelatedPartNameUpdateBindingModel binding)
        {
            var result = await vehicleFaultDataService.UpdateDataFromAsync(rawId, binding.DataFromDesc);

            return result;
        }

        #endregion
    }
}
