using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Task creation rules controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class TaskCreationRuleController : ControllerBase
    {
        private readonly ITaskCreationRuleService taskCreationRuleService;

        public TaskCreationRuleController(ITaskCreationRuleService taskCreationRuleService)
        {
            this.taskCreationRuleService = taskCreationRuleService;
        }

        /// <summary>
        /// Get TaskCreationRule.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <returns>TaskCreationRule object.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskCreationRuleViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.Administrator })]
        public async Task<TaskCreationRuleViewModel> GetAutoTaskCreationRuleAsync(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId)
        {
            var serviceModel = await taskCreationRuleService.GetTaskCreationRuleAsync(workspaceId);

            return new TaskCreationRuleViewModel
            {
                OnlyCreateWhanMatchDocumentCount = serviceModel.OnlyCreateWhanMatchDocumentCount,
                Rules = serviceModel.Rules?.Select(rule => new AutoTaskRuleViewModel
                {
                    Annotators = rule.Annotators,
                    MaxFinishDays = rule.MaxFinishDays,
                    DocumentCount = rule.DocumentCount,
                    DocumentTags = rule.DocumentTags,
                    IsAutoApproved = rule.IsAutoApproved,
                    IsAutoMerged = rule.IsAutoMerged,
                    Name = rule.Name,
                    Overlap = rule.Overlap
                })?.ToList()
            };
        }

        /// <summary>
        /// Update TaskCreationRule.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Binding model.</param>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.Administrator })]
        public async Task UpdateAutoTaskCreationRuleAsync(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody][RequiredLocalized] TaskCreationRuleUpdateBindingModel bindingModel)
        {
            var defaultRule = bindingModel.Rules.FirstOrDefault(rule => rule.Name == "Default");

            if (defaultRule == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.MustHaveDefaultRuleForTaskCreation, HttpStatusCode.BadRequest);
            }

            if (defaultRule?.DocumentTags != null && defaultRule?.DocumentTags?.Count > 0)
            {  
                ErrorHelper.ThrowException(MusicKGMessages.DefaultRuleMustNotContainAnyTag, HttpStatusCode.BadRequest);
            }

            if (bindingModel.Rules.Any(rule => rule.Name != "Default" && rule.DocumentTags?.Count == 0))
            {
                ErrorHelper.ThrowException(MusicKGMessages.NonDefaultRulesMustHaveTags, HttpStatusCode.BadRequest);
            }

            await taskCreationRuleService.UpdateRuleAsync(workspaceId, new TaskCreationRuleUpdateServiceModel
            {
                CreateUser = bindingModel.CreateUser,
                Rules = bindingModel.Rules?.Select(rule => new AutoTaskRuleServiceModel
                {
                    Annotators = rule.Annotators,
                    MaxFinishDays = rule.MaxFinishDays,
                    DocumentCount = rule.DocumentCount,
                    DocumentTags = rule.DocumentTags,
                    IsAutoApproved = rule.IsAutoApproved,
                    IsAutoMerged = rule.IsAutoMerged,
                    Name = rule.Name,
                    Overlap = rule.Overlap
                })?.ToList()
            });
        }
    }
}
