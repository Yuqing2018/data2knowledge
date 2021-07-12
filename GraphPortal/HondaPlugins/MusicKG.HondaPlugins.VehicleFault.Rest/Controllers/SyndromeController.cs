using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Syndrome controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/syndrome")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class SyndromeController : ControllerBase
    {
        private readonly ISyndromeService syndromeService;
        private readonly ILogger<SyndromeController> logger;
        /// <summary>
        /// Syndrome controller constructor.
        /// </summary>
        public SyndromeController(ILogger<SyndromeController> logger,
            ISyndromeService syndromeService)
        {
            this.logger = logger;
            this.syndromeService = syndromeService;
        }
        /// <summary>
        /// 更新 不良症状
        /// </summary>
        /// <param name="id">不良症状id.</param>
        /// <param name="binding">syndrome bingding model.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SyndromeDataModel), (int)HttpStatusCode.OK)]
        public async Task<SyndromeDataModel> UpdateAsync(
            [FromRoute][Required][StringLength(24)] string id, 
            [FromBody] SyndromeBindingModel binding)
        {
            var result = await syndromeService.UpdateAsync(id, binding.Name, binding.BadGrade);
            return result;
        }

        /// <summary>
        /// 新增不良症状
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SyndromeDataModel), (int)HttpStatusCode.OK)]
        public async Task<SyndromeDataModel> SaveAsync([FromBody] SyndromeBindingModel binding)
        {
            var result = await syndromeService.CreateAsync(binding.Name, binding.BadGrade ?? BadGrade.C);
            return result;
        }

        /// <summary>
        /// 获取不良症状列表
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<SyndromeListViewModel>), (int)HttpStatusCode.OK)]
        public async Task<PaginationViewModel<SyndromeListViewModel>> ListAsync(string  keyword, int from, int? size)
        {
            var (totalCount, datas) = await syndromeService.ListAsync(keyword,from, size);

            return new PaginationViewModel<SyndromeListViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = datas.Count(),
                Items = datas.Select(x => new SyndromeListViewModel()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name.ToString(),
                    BadGrade = x.BadGrade,
                })
            };
        }
    }
}
