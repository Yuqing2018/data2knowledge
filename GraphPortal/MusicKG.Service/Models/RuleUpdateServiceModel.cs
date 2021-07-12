using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class RuleUpdateServiceModel
    {
        public string Name { get; set; }

        public bool IsNameAssigned { get; set; }

        public string EntityId { get; set; }

        public bool IsEntityIdAssigned { get; set; }

        public List<string> Regexs { get; set; }

        public bool IsRegexAssigned { get; set; }
    }
}
