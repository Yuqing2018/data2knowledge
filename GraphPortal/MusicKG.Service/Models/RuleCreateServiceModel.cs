using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class RuleCreateServiceModel
    {
        public string Name { get; set; }

        public string EntityId { get; set; }

        public List<string> Regexs { get; set; }

        public string WorkspaceId { get; set; }
    }
}
