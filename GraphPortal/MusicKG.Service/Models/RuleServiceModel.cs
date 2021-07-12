using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class RuleServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public OntologyEntityServiceModel OntologyEntity { get; set; }

        public List<string> Regexs { get; set; }

        public string WorkspaceId { get; set; }
    }
}
