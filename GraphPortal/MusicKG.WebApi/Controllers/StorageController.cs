using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Helpers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using MusicKG.WebApi.Contract.BindingModels;
using Microsoft.AspNetCore.Http;
using MusicKG.WebApi.Extensions;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Category  controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService storageService;

        /// <summary>
        /// Category controller constructor.
        /// </summary>
        public StorageController(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        /// <summary>
        /// Download file from storage service by content md5.
        /// </summary>
        /// <param name="contentMD5"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{contentMD5}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(byte[]), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<byte[]> Get(string contentMD5)
        {
            var content = await storageService.Read(contentMD5);

            return content;
        }

        /// <summary>
        /// Upload files to storage serivce.
        /// </summary>
        /// <param name="files">Files to be uploaded.</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator })]
        public async Task Upload([FromForm] [RequiredLocalized] [MinLengthLocalized(1)] List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var content = file.GetContent();
                await storageService.Create(new DocumentStoreServiceModel
                {
                    Content = content,
                    ContentMd5 = HashHelper.GetMD5Hash(content),
                    ContentType = file.ContentType
                });
            }
        }
    }
}
