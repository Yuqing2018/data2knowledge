using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.Attributes;

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
    public class OntologyRelationController : ControllerBase
    {
        private readonly IOntologyService ontologyRelationService;

        /// <summary>
        /// Ontology controller constructor.
        /// </summary>
        public OntologyRelationController(IOntologyService ontologyRelationService)
        {
            this.ontologyRelationService = ontologyRelationService;
        }
        /// <summary>
        /// Get ontology relation list.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Ontology enity object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<OntologyRelationListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task<PaginationViewModel<OntologyRelationListItemViewModel>> GetRelations(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, relations) = await ontologyRelationService.GetOntologyRelationsAsync(workspaceId, from, size);

            return new PaginationViewModel<OntologyRelationListItemViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = relations.Count(),
                Items = relations.Select(u => new OntologyRelationListItemViewModel()
                {
                    Id = u.Id,
                    Name = u.Name,
                    FirstEntityId = u.FirstEntityId,
                    FirstEntityName = u.FirstEntityName,
                    SecondEntityId = u.SecondEntityId,
                    SecondEntityName = u.SecondEntityName,
                    Description = u.Description
                })
            };
        }

        /// <summary>
        /// Get ontology relation.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="relationId">Ontology relation ID.</param>
        /// <returns>Ontology entity object.</returns>
        [HttpGet]
        [Route("{relationId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task<OntologyRelationViewModel> GetRelation(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string relationId)
        {
            var relation = await ontologyRelationService.GetOntologyRelationAsync(workspaceId, relationId);

            return ServiceModelToOntologyRelationViewModel(relation);
        }

        /// <summary>
        /// Create ontology relation.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Ontology relation create binding object.</param>
        /// <returns>Ontology object.</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(OntologyRelationViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<OntologyRelationViewModel> CreateRelation(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody] [RequiredLocalized] OntologyRelationCreateBindingModel bindingModel)
        {
            if (!Regex.IsMatch(bindingModel.Name, OntologyConstant.PropertyAndRelationNamePattern))
            {
                ErrorHelper.ThrowException(MusicKGMessages.OntologyRelationNameWrongMessage, HttpStatusCode.BadRequest);
            }

            var relation = await ontologyRelationService.CreateOntologyRelationAsync(new OntologyRelationCreateServiceModel
            {
                WorkspaceId = workspaceId,
                Name = bindingModel.Name,
                FirstEntityId = bindingModel.FirstEntityId,
                SecondEntityId = bindingModel.SecondEntityId,
                Description = bindingModel.Description,
            });

            return ServiceModelToOntologyRelationViewModel(relation);
        }

        /// <summary>
        /// Update ontology relation.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="relationId">Ontology relation ID.</param>
        /// <param name="bindingModel">Ontology relation update binding object.</param>
        /// <returns>Ontology object.</returns>
        [HttpPut]
        [Route("{relationId}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(OntologyRelationViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<OntologyRelationViewModel> UpdateRelation(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string relationId,
            [FromBody] [RequiredLocalized] OntologyRelationUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrWhiteSpace(bindingModel.Name))
            {
                ErrorHelper.ThrowException(MusicKGMessages.OntologyRelationNameEmptyMessage, HttpStatusCode.BadRequest);
            }

            if (bindingModel.IsPropertiesAssigned)
            {
                var propertyNames = bindingModel.Properties.Select(x => x.Name.ToLower());

                if (propertyNames.Distinct().Count() != bindingModel.Properties.Count)
                {
                    ErrorHelper.ThrowException(MusicKGMessages.OntologyRelationDuplicatePropertyNameMessage, HttpStatusCode.BadRequest);
                }

                foreach (var propertyName in propertyNames)
                {
                    if (!Regex.IsMatch(propertyName, OntologyConstant.PropertyAndRelationNamePattern))
                    {
                        ErrorHelper.ThrowException(MusicKGMessages.OntologyRelationPropertyNameWrongMessage, HttpStatusCode.BadRequest);
                    }
                }
            }

            var updateServiceModel = new OntologyRelationUpdateServiceModel
            {
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                Properties = bindingModel.Properties?.Select(x => new OntologyRelationPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type,
                }).ToList(),
                IsPropertiesAssigned = bindingModel.IsPropertiesAssigned,
                SecondEntityId = bindingModel.SecondEntityId,
                IsSecondEntityAssigned = bindingModel.IsSecondEntityAssigned,
                Description = bindingModel.Description,
                IsDescriptionAssigned = bindingModel.IsDescriptionAssigned
            };

            var relation = await ontologyRelationService.UpdateOntologyRelationAsync(workspaceId, relationId, updateServiceModel);

            return ServiceModelToOntologyRelationViewModel(relation);
        }

        private OntologyRelationViewModel ServiceModelToOntologyRelationViewModel(OntologyRelationServiceModel serviceModel)
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

        /// <summary>
        /// Delete ontology relation.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="relationId">Ontology relation Id.</param>
        [HttpDelete]
        [Route("{relationId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteRelation(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string relationId)
        {
            await ontologyRelationService.DeleteOntologyRelationAsync(workspaceId, relationId);
        }
    }
}
