using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Enums;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Extensions;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Ontology controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public partial class OntologyEntityController : ControllerBase
    {
        private readonly IOntologyService ontologyEntityService;

        /// <summary>
        /// Ontology controller constructor.
        /// </summary>
        public OntologyEntityController(IOntologyService ontologyEntityService)
        {
            this.ontologyEntityService = ontologyEntityService;
        }

        /// <summary>
        /// Get ontology entity list.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Ontology enity object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<OntologyEntityListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task<PaginationViewModel<OntologyEntityListItemViewModel>> GetEntities(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, entities) = await ontologyEntityService.GetOntologyEntitiesAsync(workspaceId, from, size);

            return new PaginationViewModel<OntologyEntityListItemViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = entities.Count(),
                Items = entities.Select(u => new OntologyEntityListItemViewModel()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Color = u.Color
                })
            };
        }

        /// <summary>
        /// Get ontology entity.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="entityId">Ontology entity ID.</param>
        /// <returns>Ontology entity object.</returns>
        [HttpGet]
        [Route("{entityId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task<OntologyEntityViewModel> GetEntity(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string entityId)
        {
            var entity = await ontologyEntityService.GetOntologyEntityAsync(workspaceId, entityId);

            return ServiceModelToOntologyEntityViewModel(entity);
        }

        /// <summary>
        /// Create ontology entity.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Ontology entity create binding object.</param>
        /// <returns>Ontology object.</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(OntologyEntityViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<OntologyEntityViewModel> CreateEntity(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody] [RequiredLocalized] OntologyEntityCreateBindingModel bindingModel)
        {
            var entity = await ontologyEntityService.CreateOntologyEntityAsync(new OntologyEntityCreateServiceModel
            {
                WorkspaceId = workspaceId,
                Name = bindingModel.Name,
                Description = bindingModel.Description,
                Color = bindingModel.Color ?? RandomStringHelper.RandomColorString()
            });

            return ServiceModelToOntologyEntityViewModel(entity);
        }

        /// <summary>
        /// Update ontology entity.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="entityId">Ontology entity ID.</param>
        /// <param name="bindingModel">Ontology entity update binding object.</param>
        /// <returns>Ontology object.</returns>
        [HttpPut]
        [Route("{entityId}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(OntologyEntityViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<OntologyEntityViewModel> UpdateEntity(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string entityId,
            [FromBody] [RequiredLocalized] OntologyEntityUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrWhiteSpace(bindingModel.Name))
            {
                ErrorHelper.ThrowException(MusicKGMessages.OntologyEntityNameEmptyMessage, HttpStatusCode.BadRequest);
            }

            if (bindingModel.IsPropertiesAssigned)
            {
                var propertyNames = bindingModel.Properties.Select(x => x.Name.ToLower());

                if (propertyNames.Distinct().Count() != bindingModel.Properties.Count)
                {
                    ErrorHelper.ThrowException(MusicKGMessages.OntologyEntityDuplicatePropertyNameMessage, HttpStatusCode.BadRequest);
                }

                foreach (var propertyName in propertyNames)
                {
                    if (!Regex.IsMatch(propertyName, OntologyConstant.PropertyAndRelationNamePattern))
                    {
                        ErrorHelper.ThrowException(MusicKGMessages.OntologyEntityPropertyNameWrongMessage, HttpStatusCode.BadRequest);
                    }
                }
            }

            var updateServiceModel = new OntologyEntityUpdateServiceModel
            {
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                Properties = bindingModel.Properties?.Select(x => new OntologyEntityPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type,
                }).ToList(),
                IsPropertiesAssigned = bindingModel.IsPropertiesAssigned,
                Description = bindingModel.Description,
                IsDescriptionAssigned = bindingModel.IsDescriptionAssigned,
                Color = bindingModel.Color,
                IsColorAssigned = bindingModel.IsColorAssigned
            };

            var entity = await ontologyEntityService.UpdateOntologyEntityAsync(workspaceId, entityId, updateServiceModel);

            return ServiceModelToOntologyEntityViewModel(entity);
        }

        /// <summary>
        /// Delete ontology entity.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="entityId">Ontology entity ID.</param>
        [HttpDelete]
        [Route("{entityId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteEntity(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string entityId)
        {
            await ontologyEntityService.DeleteOntologyEntityAsync(workspaceId, entityId);
        }

        /// <summary>
        /// Upload ontology.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="file">Ontology file.</param>
        /// <returns>Ontology entity object list.</returns>
        [HttpPost]
        [Route("Content")]
        [Produces("application/json", "text/plain")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(IEnumerable<OntologyUploadViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<OntologyUploadViewModel> UploadOntology(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [RequiredLocalized] IFormFile file)
        {
            OntologyUploadServiceModel uploadModel = file.ToObject<OntologyUploadServiceModel>();

            if (uploadModel == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.DeserializeJsonFileFailedMessage, HttpStatusCode.BadRequest);
            }

            var serviceModel = await ontologyEntityService.UploadOntologyAsync(workspaceId, uploadModel);

            return OntologyUploadServiceModelToViewModel(serviceModel);
        }

        /// <summary>
        /// Download ontology.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="type">Ontology download file type.</param>
        /// <returns>Ontology file.</returns>
        [HttpGet]
        [Route("Content")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<FileResult> DownloadOntology(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] OntologyDownloadFileTypeEnum type)
        {
            var fileContent = await ontologyEntityService.DownloadOntologyAsync(workspaceId, type);

            if (OntologyDownloadFileTypeEnum.HugeGraphSchema == type)
                return File(fileContent, HttpContentTypes.ApplicationJson, String.Format(OntologyConstant.HugeGraphOntologyFileFormat, DateTime.UtcNow.ToString()));

            return File(fileContent, HttpContentTypes.ApplicationJson, String.Format(OntologyConstant.OntologyJsonFileFormat, DateTime.UtcNow.ToString()));
        }

        private OntologyEntityViewModel ServiceModelToOntologyEntityViewModel(OntologyEntityServiceModel serviceModel)
        {
            var result = new OntologyEntityViewModel()
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                Color = serviceModel.Color,
                Properties = serviceModel.Properties.Select(x => new OntologyPropertyViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type
                }).ToList()
            };

            return result;
        }

        private OntologyUploadViewModel OntologyUploadServiceModelToViewModel(OntologyUploadServiceModel serviceModel)
        {
            var result = new OntologyUploadViewModel()
            {
                Entities = serviceModel.Entities?.Select(x => ServiceModelToOntologyEntityViewModel(x)).ToList(),
                Relations = serviceModel.Relations?.Select(x => ServiceModelToOntologyRelationViewModel(x)).ToList(),
            };

            return result;
        }

        private OntologyRelationViewModel ServiceModelToOntologyRelationViewModel(OntologyRelationDownloadServiceModel serviceModel)
        {
            var result = new OntologyRelationViewModel()
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                FirstEntityId = serviceModel.FirstEntityId,
                SecondEntityId = serviceModel.SecondEntityId,
                Description = serviceModel.Description,
                Properties = serviceModel.Properties.Select(x => new OntologyPropertyViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type
                }).ToList()
            };

            return result;
        }
    }
}