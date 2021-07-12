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
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Rule controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class RuleController : ControllerBase
    {
        private readonly IRuleService ruleService;

        /// <summary>
        /// Rule controller constructor.
        /// </summary>
        public RuleController(IRuleService ruleService)
        {
            this.ruleService = ruleService;
        }

        /// <summary>
        /// Get rule list.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Rule object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<RuleListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<PaginationViewModel<RuleListItemViewModel>> GetRules(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, rules) = await ruleService.GetRulesAsync(workspaceId, from, size);
            return new PaginationViewModel<RuleListItemViewModel>()
            {
                TotalCount = totalCount,
                From = from,
                Count = rules.Count(),
                Items = rules.Select(m => new RuleListItemViewModel()
                {
                    EntityId = m.OntologyEntity.Id,
                    Id = m.Id,
                    Name = m.Name,
                    WorkspaceId = m.WorkspaceId,
                    EntityName = m.OntologyEntity.Name
                })
            };
        }

        /// <summary>
        /// Get rule.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="ruleId">Rule ID.</param>
        /// <returns>Rule object.</returns>
        [HttpGet]
        [Route("{ruleId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RuleViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<RuleViewModel> GetRule(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string ruleId)
        {
            var rule = await ruleService.GetRuleAsync(workspaceId, ruleId);

            return new RuleViewModel()
            {
                Id = rule.Id,
                EntityId = rule.OntologyEntity.Id,
                EntityName = rule.OntologyEntity.Name,
                Name = rule.Name,
                Regexes = rule.Regexs.Select(m => new RegexViewModel()
                {
                    Id = rule.Regexs.IndexOf(m).ToString(),
                    Regex = m
                }),
                WorkspaceId = rule.WorkspaceId
            };
        }

        /// <summary>
        /// Create rule.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Rule create binding object.</param>
        /// <returns>Rule Id.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RuleViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<string> CreateRule(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody] RuleCreateBindingModel bindingModel)
        {
            var ruleId = await ruleService.CreateRuleAsync(new RuleCreateServiceModel()
            {
                EntityId = bindingModel.EntityId,
                Name = bindingModel.Name,
                Regexs = bindingModel.Regexes,
                WorkspaceId = workspaceId
            });

            return ruleId;
        }

        /// <summary>
        /// Update rule.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="ruleId">Rule ID.</param>
        /// <param name="bindingModel">Rule update binding object.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{ruleId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RuleViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task UpdateRule(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string ruleId,
            [FromBody] [RequiredLocalized] RuleUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrEmpty(bindingModel.Name))
                ErrorHelper.ThrowException(MusicKGMessages.RuleNameEmptyMessage, HttpStatusCode.BadRequest);

            if (bindingModel.IsEntityIdAssigned && string.IsNullOrEmpty(bindingModel.EntityId))
                ErrorHelper.ThrowException(MusicKGMessages.RuleEntityIdEmptyMessage, HttpStatusCode.BadRequest);

            if (bindingModel.IsRegexesAssigned && bindingModel.Regexes.Count == 0)
                ErrorHelper.ThrowException(MusicKGMessages.RuleRegexsEmptyMessage, HttpStatusCode.BadRequest);

            await ruleService.UpdateRuleAsync(ruleId, new RuleUpdateServiceModel()
            {
                EntityId = bindingModel.EntityId,
                IsEntityIdAssigned = bindingModel.IsEntityIdAssigned,
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                Regexs = bindingModel.Regexes,
                IsRegexAssigned = bindingModel.IsRegexesAssigned
            });
        }

        /// <summary>
        /// Delete rule.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="ruleId">Rule ID.</param>
        [HttpDelete]
        [Route("{ruleId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteRule(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string ruleId)
        {
            await ruleService.DeleteRuleAsync(ruleId);
        }
    }
}