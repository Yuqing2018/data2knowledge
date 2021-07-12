using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
using MusicKG.WebApi.Contract.BindingModels;

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
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService dictionaryService;

        /// <summary>
        /// Dictionary controller constructor.
        /// </summary>
        public DictionaryController(IDictionaryService dictionaryService)
        {
            this.dictionaryService = dictionaryService;
        }

        /// <summary>
        /// Get dictionary list.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Dictionary object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DictionaryListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<PaginationViewModel<DictionaryListItemViewModel>> GetDictionaries(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, dictionaries) = await dictionaryService.GetDictionariesAsync(workspaceId, from, size);

            return new PaginationViewModel<DictionaryListItemViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = dictionaries.Count(),
                Items = dictionaries.Select(u => new DictionaryListItemViewModel()
                {
                    WorkspaceId = u.WorkspaceId,
                    Id = u.Id,
                    Name = u.Name,
                    EntityId = u.EntityId,
                    EntityName = u.EntityName,
                    EntriesCount = u.EntriesCount
                })
            };
        }

        /// <summary>
        /// Get dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary ID.</param>
        /// <param name="from">Pagination start index of dictionary entries.</param>
        /// <param name="size">Pagination size of dictionary entries.</param>
        /// <returns>Dictionary object.</returns>
        [HttpGet]
        [Route("{dictionaryId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DictionaryDetailViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<DictionaryDetailViewModel> GetDictionary(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string dictionaryId,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var dictionary = await dictionaryService.GetDictionaryAsync(workspaceId, dictionaryId);

            return ServiceModelToDictionaryDetailViewModel(dictionary, from, size);
        }

        private DictionaryDetailViewModel ServiceModelToDictionaryDetailViewModel(DictionaryServiceModel serviceModel, int from, int? size)
        {
            var totalCount = serviceModel.Vocabularies.Count();
            var entries = serviceModel.Vocabularies.Skip(from).Take(size ?? int.MaxValue);

            var result = new DictionaryDetailViewModel()
            {
                WorkspaceId = serviceModel.WorkspaceId,
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                EntityId = serviceModel.EntityId,
                EntityName = null,
                CreatedAt = serviceModel.CreatedAt,
                CreatedBy = serviceModel.CreatedBy,
                Entries = new PaginationViewModel<string>
                {
                    TotalCount = totalCount,
                    From = from,
                    Count = entries.Count(),
                    Items = entries
                }
            };

            return result;
        }

        /// <summary>
        /// Create dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Dictionary create binding object.</param>
        /// <returns>Dictionary object.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DictionaryViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<DictionaryViewModel> CreateDictionary(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromBody] [RequiredLocalized] DictionaryCreateBindingModel bindingModel)
        {
            var currentUser = HttpContextHelper.GetCurrentUser(HttpContext);

            var dictionary = await dictionaryService.CreateDictionaryAsync(new DictionaryCreateServiceModel
            {
                WorkspaceId = workspaceId,
                Name = bindingModel.Name,
                EntityId = bindingModel.EntityId,
                Vocabularies = bindingModel.Vocabularies,
                CreatedBy = currentUser.Item1
            });

            return ServiceModelToDictionaryViewModel(dictionary);
        }

        private DictionaryViewModel ServiceModelToDictionaryViewModel(DictionaryServiceModel serviceModel)
        {
            var result = new DictionaryViewModel()
            {
                WorkspaceId = serviceModel.WorkspaceId,
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                EntityId = serviceModel.EntityId,
                EntityName = null,
                CreatedAt = serviceModel.CreatedAt,
                CreatedBy = serviceModel.CreatedBy,
                Vocabularies = serviceModel.Vocabularies,
            };

            return result;
        }

        /// <summary>
        /// Update dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary ID.</param>
        /// <param name="bindingModel">Dictionary update binding object.</param>
        /// <returns>Dictionary object.</returns>
        [HttpPut]
        [Route("{dictionaryId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DictionaryViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<DictionaryViewModel> UpdateDictionary(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string dictionaryId,
            [FromBody] [RequiredLocalized] DictionaryUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrWhiteSpace(bindingModel.Name))
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryNameEmtpyMessage, HttpStatusCode.BadRequest);

            var dictionary = await dictionaryService.UpdateDictionaryAsync(workspaceId, dictionaryId,
                new DictionaryUpdateServiceModel
                {
                    Name = bindingModel.Name,
                    IsNameAssigned = bindingModel.IsNameAssigned,
                    Vocabularies = bindingModel.Vocabularies,
                    IsVocabulariesAssigned = bindingModel.IsVocabulariesAssigned,
                });

            return ServiceModelToDictionaryViewModel(dictionary);
        }

        /// <summary>
        /// Update dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary ID.</param>
        /// <param name="vocabularies">Dictionary entries to add.</param>
        /// <returns>Dictionary object.</returns>
        [HttpPut]
        [Route("{dictionaryId}/UpdateVocabs")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator })]
        public async Task UpdateDictionaryVocabs(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string dictionaryId,
            [FromBody][RequiredLocalized] List<string> vocabularies)
        {
            await dictionaryService.UpdateOneVocabsAsync(workspaceId, dictionaryId,
                vocabularies);
        }

        /// <summary>
        /// Delete dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary ID.</param>
        [HttpDelete]
        [Route("{dictionaryId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteDictionary(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string dictionaryId)
        {
            await dictionaryService.DeleteDictionaryAsync(workspaceId, dictionaryId);
        }

        /// <summary>
        /// Upload dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="name">Dictionary name.</param>
        /// <param name="entityId">Entity ID.</param>
        /// <param name="files">Dictionary files.</param>
        /// <returns>Dictionary file.</returns>
        [HttpPost]
        [Route("Content")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<DictionaryListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<PaginationViewModel<DictionaryListItemViewModel>> UploadDictionary(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            [FromForm] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]   string name,
            [FromForm] string entityId,
            [FromForm] [RequiredLocalized] [MinLengthLocalized(1)] List<IFormFile> files)
        {
            var currentUser = HttpContextHelper.GetCurrentUser(HttpContext);

            List<string> Vocabularies = new List<string>();

            files.ForEach(item =>
            {
                if (item.ContentType.ToLower().Equals(HttpContentTypes.TextPlain))
                {
                    using (Stream Stream = item.OpenReadStream())
                    {
                        var words = ReadTxtStream(Stream);

                        Vocabularies.AddRange(words);
                    }
                }
            });

            if (0 == Vocabularies.Count())
                ErrorHelper.ThrowException(MusicKGMessages.DictionaryEntriesEmptyMessage, HttpStatusCode.BadRequest);

            var dictionary = await dictionaryService.CreateDictionaryAsync(new DictionaryCreateServiceModel
            {
                WorkspaceId = workspaceId,
                Name = name,
                EntityId = entityId,
                Vocabularies = Vocabularies.Distinct().ToList(),
                CreatedBy = currentUser.Item1
            });

            var (totalCount, dictionaries) = await dictionaryService.GetDictionariesAsync(workspaceId, 0, null);

            return new PaginationViewModel<DictionaryListItemViewModel>
            {
                TotalCount = totalCount,
                From = 0,
                Count = dictionaries.Count(),
                Items = dictionaries.Select(u => new DictionaryListItemViewModel()
                {
                    WorkspaceId = u.WorkspaceId,
                    Id = u.Id,
                    Name = u.Name,
                    EntityId = u.EntityId,
                    EntityName = u.EntityName,
                    EntriesCount = u.EntriesCount
                })
            };
        }

        private IEnumerable<string> ReadTxtStream(Stream fileStream)
        {
            List<string> vacabularies = new List<string>();

            using (StreamReader sr = new StreamReader(fileStream))
            {
                var line = String.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    var entry = line.Trim();

                    if (vacabularies.Contains(entry) || String.IsNullOrWhiteSpace(entry))
                        continue;

                    vacabularies.Add(line);
                }
            }

            return vacabularies;
        }

        /// <summary>
        /// Download dictionary.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary ID.</param>
        /// <returns>Dictionary file.</returns>
        [HttpGet]
        [Route("Content/{dictionaryId}")]
        [Produces("text/plain")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<FileResult> DownloadDictionary(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string dictionaryId)
        {
            var dictionary = await dictionaryService.GetDictionaryAsync(workspaceId, dictionaryId);

            string fileName = $"{dictionary.Name}.txt";

            byte[] dataAsBytes = dictionary.Vocabularies
                .SelectMany(s => Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();

            return File(dataAsBytes, HttpContentTypes.TextPlain, fileName);
        }

        /// <summary>
        /// get dictionary entries.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="dictionaryId">Dictionary Id.</param>
        /// <param name="filterString">Filter string.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Dictionary object list.</returns>
        [HttpGet]
        [Route("Entries/{dictionaryId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<PaginationViewModel<String>> GetDictionaryEntries(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string dictionaryId,
            [FromQuery] string filterString,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, entries) = await dictionaryService.GetDictionaryEntriesAsync(workspaceId, dictionaryId, filterString?.Trim(), from, size);

            return new PaginationViewModel<string>
            {
                TotalCount = totalCount,
                From = from,
                Count = entries.Count(),
                Items = entries
            };
        }
    }
}