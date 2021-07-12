using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using MusicKG.HondaPlugins.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Warning Task Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/warningindex")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class WarningIndexController : ControllerBase
    {
        private readonly ILogger<WarningIndexController> _logger;
        private readonly IWarningIndexService _warningIndexService;
        /// <summary>
        /// Warning task controller construct function.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="warningIndexService"></param>
        public WarningIndexController(ILogger<WarningIndexController> logger,
            IWarningIndexService warningIndexService)
        {
            _logger = logger;
            _warningIndexService = warningIndexService;
        }

        /// <summary>
        /// get warning index by warning type.
        /// </summary>
        /// <param name="warningType">warning type</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(WarningIndexViewModel), (int)HttpStatusCode.OK)]
        public async Task<List<WarningIndexViewModel>> ListAsync([FromQuery] [Required] WarningType warningType)
        {
            var results = await _warningIndexService.GetListAsync(warningType);
            return results.Select(x=>DataModelToViewModel(x)).ToList();
        }

        /// <summary>
        /// save many warning index.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Index")]
        [ProducesResponseType(typeof(List<WarningIndexDataModel>), (int)HttpStatusCode.OK)]
        public async Task<List<WarningIndexDataModel>> PostWarningIndex()
        {
            var results = await _warningIndexService.GetListAsync(WarningType.多发预警);
            if (results.Count > 0)
                return new List<WarningIndexDataModel>();

            var saveDatas = new List<WarningIndexDataModel>()
            {
                new WarningIndexDataModel()
                {
                    WarningType = WarningType.多发预警,
                    IndexName = WarningIndexNames.近三个月相对上升率,
                    Value = "200",
                    Unit = "%",
                },
                new WarningIndexDataModel()
                {
                    WarningType = WarningType.多发预警,
                    IndexName = WarningIndexNames.近三个月发生件数,
                    Value = "4"
                },
                new WarningIndexDataModel()
                {
                    WarningType = WarningType.多发预警,
                    IndexName = WarningIndexNames.年款不良率,
                    Value = "2019",
                    Unit = "%"
                },
                new WarningIndexDataModel()
                {
                    WarningType = WarningType.风险预警,
                    IndexName = WarningIndexNames.风险等级,
                    Value = "重点关注,高风险,潜在高风险,中风险,其他关注,低风险,一般"
                },
                new WarningIndexDataModel()
                {
                    WarningType = WarningType.再发预警,
                    IndexName = WarningIndexNames.对策后生产车辆的MQI件数,
                    Value = "4"
                },
                new WarningIndexDataModel()
                {
                    WarningType = WarningType.再发预警,
                    IndexName = WarningIndexNames.对策后再发不良率,
                    Value = "对策前不良率",
                    Unit = "%"
                }
            };
            var datas = await _warningIndexService.SaveManyAsync(saveDatas);
            return datas;
        }

        private WarningIndexViewModel DataModelToViewModel(WarningIndexDataModel data)
        {
            return new WarningIndexViewModel()
            {
                Id = data.Id.ToString(),
                IndexName = data.IndexName,
                WarningType = data.WarningType,
                Value = data.Value,
                Unit = data.Unit
            };
        }
    }
}
