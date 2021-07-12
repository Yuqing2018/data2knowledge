using MusicKG.DataAccess;
using MusicKG.Service.Models;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Resources;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace MusicKG.Service.Implementations
{
    public class TaskCreationRuleService : ITaskCreationRuleService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<TaskCreationRuleService> logger;

        public TaskCreationRuleService(IMusicKGContext context, ILogger<TaskCreationRuleService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<TaskCreationRuleServiceModel> GetTaskCreationRuleAsync(string workspaceId)
        {
            var dataModel = await context.AutoTaskCreationRules.AsQueryable()
                .Where(rule => rule.WorkspaceId == new ObjectId(workspaceId))
                .FirstOrDefaultAsync();

            if (dataModel == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.AutoTaskCreationRuleNotExistsMessage, HttpStatusCode.BadRequest);
            }

            return new TaskCreationRuleServiceModel
            {
                WorkspaceId = workspaceId,
                OnlyCreateWhanMatchDocumentCount = dataModel.OnlyCreateWhanMatchDocumentCount,
                Rules = dataModel.Rules?.Select(rule => new AutoTaskRuleServiceModel
                {
                    Annotators = rule.Annotators?.Select(a => a.ToString())?.ToList(),
                    DocumentTags = rule.DocumentTags,
                    Name = rule.Name,
                    MaxFinishDays = rule.MaxFinishDays,
                    IsAutoApproved = rule.IsAutoApproved,
                    IsAutoMerged = rule.IsAutoMerged,
                    Overlap = rule.Overlap
                })?.ToList()
            };
        }

        public async Task UpdateRuleAsync(string workspaceId, TaskCreationRuleUpdateServiceModel updateModel)
        {
            var originRules = await context.AutoTaskCreationRules.AsQueryable().FirstOrDefaultAsync(rule => rule.WorkspaceId == new ObjectId(workspaceId));

            if (originRules == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.AutoTaskCreationRuleNotExistsMessage, HttpStatusCode.BadRequest);
            }

            originRules.CreatedUser = await ValidateUserAsync(updateModel.CreateUser) ?  new ObjectId(updateModel.CreateUser) : originRules.CreatedUser;

            originRules.Rules = updateModel.Rules.Select(rule => new AutoTaskRuleDataModel
            {
                Annotators = rule.Annotators?.Select(a => new ObjectId(a))?.ToList(),
                MaxFinishDays = rule.MaxFinishDays,
                DocumentCount = rule.DocumentCount,
                DocumentTags = rule.DocumentTags,
                IsAutoApproved = rule.IsAutoApproved,
                IsAutoMerged = rule.IsAutoMerged,
                Name = rule.Name,
                Overlap = rule.Overlap
            })?.ToList();

            var ruleSet = new HashSet<string>();

            originRules.Rules.ForEach(rule =>
            {
                rule.DocumentTags.ForEach(tag =>
                {
                    if (ruleSet.Contains(tag))
                        ErrorHelper.ThrowException(MusicKGMessages.DifferentRulesCannotHaveSameTagMessage, HttpStatusCode.BadRequest);
                    else
                        ruleSet.Add(tag);
                });
            });

            try
            {
                var result = await context.AutoTaskCreationRules.ReplaceOneAsync(rule => rule.WorkspaceId == new ObjectId(workspaceId), originRules);
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGMessages.AuotTaskCreationRulesUpdateFailedMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.InternalServerError);
            }
        }

        private async Task<bool> ValidateUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            return await context.Users.AsQueryable().AnyAsync(u => u.Id == new ObjectId(userId));
        }
    }
}
