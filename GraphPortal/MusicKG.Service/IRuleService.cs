using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicKG.Service.Models;

namespace MusicKG.Service
{
    public interface IRuleService
    {
        Task<Tuple<long, IEnumerable<RuleServiceModel>>> GetRulesAsync(string workspaceId, int from, int? size);

        Task<RuleServiceModel> GetRuleAsync(string workspaceId, string id);

        Task<string> CreateRuleAsync(RuleCreateServiceModel ruleCreate);

        Task UpdateRuleAsync(string ruleId, RuleUpdateServiceModel ruleService);

        Task DeleteRuleAsync(string id);
    }
}
