using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class DictionaryCreateServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Name { get; set; }

        public string EntityId { get; set; }

        public string CreatedBy { get; set; }

        public List<string> Vocabularies { get; set; }
    }
}
