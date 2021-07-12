using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.WebApi.Extensions;
using MusicKG.WebApi.Filters;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Dictionary controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class TagController : ControllerBase
    {
        private readonly ITagService tagService;

        /// <summary>
        /// Dictionary controller constructor.
        /// </summary>
        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        /// <summary>
        /// Get tag values by tag type.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="filter">Tag value filter.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Tag value list.</returns>
        [HttpGet("{type}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<TagValueViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly,UserRoleEnum.Annotator })]
        public async Task<PaginationViewModel<TagValueViewModel>> GetTagValuesAsync(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] TagTypeEnum type,
            [FromQuery] string filter,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, tagValues) = await tagService.GetTagValuesAsync(workspaceId, type, filter, from, size);

            return new PaginationViewModel<TagValueViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = tagValues.Count(),
                Items = tagValues.Select(t => new TagValueViewModel
                {
                    Id = t.Id,
                    Value = t.Value,
                    Color = t.Color,
                    Description = t.Description
                })
            };
        }

        /// <summary>
        /// Upload tag values.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="override">Remove current values or not.</param>
        /// <param name="file">Tag values file.</param>
        /// <returns></returns>
        [HttpPost("{type}/Content")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task UploadTagValuesAsync(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] TagTypeEnum type,
            [FromQuery] bool @override,
            [FromForm] [RequiredLocalized] IFormFile file)
        {
            var values = file.SplitByLine();

            var serviceModel = new TagServiceModel
            {
                WorkspaceId = workspaceId,
                Type = type,
                Values = values?.Select(value => new TagValueServiceModel
                {
                    Value = value,
                    Color = RandomStringHelper.RandomColorString()
                })?.ToList()
            };

            if (@override)
            {
                await tagService.ReplaceTagAsync(serviceModel);
            }
            else
            {
                await tagService.AddTagValuesAsync(serviceModel);
            }
        }

        /// <summary>
        /// Download tag values.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <returns>Tag values file.</returns>
        [HttpGet("{type}/Content")]
        [Produces("text/plain")]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<FileResult> DownloadTagValuesAsync(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] TagTypeEnum type)
        {
            var tag = await tagService.GetTagValuesAsync(workspaceId, type, "", 0, null);

            var fileName = $"{workspaceId}_{type}.txt";

            byte[] dataAsBytes = tag.values
                .SelectMany(value => Encoding.UTF8.GetBytes(value.Value + Environment.NewLine)).ToArray();

            return File(dataAsBytes, HttpContentTypes.TextPlain, fileName);
        }

        /// <summary>
        /// Append values to tag.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="bindingModels">Tag values model to be appended.</param>
        /// <returns></returns>
        [HttpPost("{type}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task AppendTagValuesAsync(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] TagTypeEnum type,
            [FromBody] [RequiredLocalized] List<TagValueAppendBindingModel> bindingModels)
        {
            var serviceModel = new TagServiceModel
            {
                WorkspaceId = workspaceId,
                Type = type,
                Values = bindingModels?.Select(value => new TagValueServiceModel
                {
                    Value = value.Value,
                    Color = string.IsNullOrWhiteSpace(value.Color) ? RandomStringHelper.RandomColorString() : value.Color,
                    Description = value.Description
                })?.ToList()
            };

            await tagService.AddTagValuesAsync(serviceModel);
        }

        /// <summary>
        /// Update tag value.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="valueId">Value Id.</param>
        /// <param name="bindingModel">Binding model.</param>
        /// <returns></returns>
        [HttpPut("{type}/Values/{valueId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task UpdateTagValueAsync(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] TagTypeEnum type,
            [FromRoute] [RequiredLocalized] string valueId,
            [FromBody] [RequiredLocalized] TagValueUpdateBindingModel bindingModel)
        {
            await tagService.UpdateTagValueAsync(workspaceId, type, valueId, new TagValueUpdateServiceModel
            {
                IsValueAssigned = bindingModel.IsValueAssigned,
                Value = bindingModel.Value,
                IsColorAssigned = bindingModel.IsColorAssigned,
                Color = bindingModel.Color,
                IsDescriptionAssigned = bindingModel.IsDescriptionAssigned,
                Description = bindingModel.Description
            });
        }

        /// <summary>
        /// Delete tag values.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="type">Tag type.</param>
        /// <param name="valueIds">Value Ids (null indicate delete all).</param>
        /// <returns></returns>
        [HttpDelete("{type}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteTagValuesAsync(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] TagTypeEnum type,
            [FromBody] List<string> valueIds)
        {
            await tagService.DeleteTagValuesAsync(workspaceId, type, valueIds);
        }
    }
}