using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// Rule Service.
    /// </summary>
    public class RuleService : IRuleService
    {
        private readonly IMusicKGContext context;

        public RuleService(IMusicKGContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get All By WorkSpaceId.
        /// </summary>
        /// <param name="workspaceId">WorkSpace Id.</param>
        /// <param name="from">Start Index.</param>
        /// <param name="size">Size.</param>
        /// <returns></returns>
        public async Task<Tuple<long, IEnumerable<RuleServiceModel>>> GetRulesAsync(string workspaceId, int from, int? size)
        {
            var ruleDatas = context.Rules.AsQueryable().Where(m => m.WorkspaceId == new ObjectId(workspaceId));

            var totalCount = await ruleDatas.CountAsync();

            var rules = await (from r in ruleDatas
                               join e in context.OntologyEntities.AsQueryable() on r.EntityId equals e.Id 
                               select new RuleResult
                               {
                                   ObjectId = r.Id,
                                   Name = r.Name,
                                   Regexs = r.Regexs,
                                   OntologyEntityResult = new OntologyEntityResult
                                   {
                                       ObjectId = e.Id,
                                       Name = e.Name
                                   },
                                   WorkspaceIdResult = r.WorkspaceId
                               }).Skip(from).Take(size ?? int.MaxValue).ToListAsync();

            return new Tuple<long, IEnumerable<RuleServiceModel>>(totalCount, rules);
        }

        /// <summary>
        /// Get One Rule.
        /// </summary>
        /// <param name="id">Rule Id.</param>
        /// <param name="workspaceId">WorkSpace Id.</param>
        /// <returns></returns>
        public async Task<RuleServiceModel> GetRuleAsync(string workspaceId, string id)
        {
            var ruleData = context.Rules.AsQueryable().Where(m => m.WorkspaceId == new ObjectId(workspaceId) && m.Id == new ObjectId(id));
            var rule = await (from r in ruleData
                              join e in context.OntologyEntities.AsQueryable() on r.EntityId equals e.Id 
                              select new RuleResult
                              {
                                  ObjectId = r.Id,
                                  Name = r.Name,
                                  Regexs = r.Regexs,
                                  OntologyEntityResult = new OntologyEntityResult
                                  {
                                      ObjectId = e.Id,
                                      Name = e.Name
                                  },
                                  WorkspaceIdResult = r.WorkspaceId
                              }).FirstOrDefaultAsync();

            if (rule == null)
                ErrorHelper.ThrowException(MusicKGMessages.RuleNotExistMessage, HttpStatusCode.BadRequest);

            return rule;
        }

        /// <summary>
        /// Create Rule.
        /// </summary>
        /// <param name="ruleCreate">RuleCreateServiceModel.</param>
        /// <returns></returns>
        public async Task<string> CreateRuleAsync(RuleCreateServiceModel ruleCreate)
        {
            var count = await context.Rules.CountDocumentsAsync(u => u.Name == ruleCreate.Name);

            if (count > 0)
                ErrorHelper.ThrowException(MusicKGMessages.RuleNameExistMessage, HttpStatusCode.BadRequest);

            var rule = new RuleDataModel()
            {
                Name = ruleCreate.Name,
                EntityId = new ObjectId(ruleCreate.EntityId),
                Regexs = ruleCreate.Regexs,
                WorkspaceId = new ObjectId(ruleCreate.WorkspaceId)
            };

            await context.Rules.InsertOneAsync(rule);

            return rule.Id.ToString();
        }

        /// <summary>
        /// Update Rule.
        /// </summary>
        /// <param name="ruleId">Rule Id.</param>
        /// <param name="ruleService">RuleUpdateServiceModel.</param>
        /// <returns></returns>
        public async Task UpdateRuleAsync(string ruleId, RuleUpdateServiceModel ruleService)
        {
            var update = Builders<RuleDataModel>.Update.Set(u => u.Id, new ObjectId(ruleId));

            if (ruleService.IsEntityIdAssigned)
                update = update.Set(u => u.EntityId, new ObjectId(ruleService.EntityId));
            if (ruleService.IsNameAssigned)
                update = update.Set(m => m.Name, ruleService.Name);
            if (ruleService.IsRegexAssigned)
                update = update.Set(m => m.Regexs, ruleService.Regexs);

            var result = await context.Rules.UpdateOneAsync(u => u.Id == new ObjectId(ruleId), update);

            if (result.MatchedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.RuleNotExistMessage, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Delete Rule.
        /// </summary>
        /// <param name="id">Rule Id.</param>
        /// <returns></returns>
        public async Task DeleteRuleAsync(string id)
        {
            var result = await context.Rules.DeleteOneAsync(u => u.Id == new ObjectId(id));

            if (result.DeletedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.RuleNotExistMessage, HttpStatusCode.BadRequest);
        }
    }

    [BsonIgnoreExtraElements]
    public class RuleResult : RuleServiceModel
    {
        public ObjectId ObjectId { set { Id = value.ToString(); } }

        public OntologyEntityResult OntologyEntityResult { set { OntologyEntity = value; } }

        public ObjectId WorkspaceIdResult { set { WorkspaceId = value.ToString(); } }
    }

    [BsonIgnoreExtraElements]
    public class OntologyEntityResult : OntologyEntityServiceModel
    {
        public ObjectId ObjectId { set { Id = value.ToString(); } }
    }
}
