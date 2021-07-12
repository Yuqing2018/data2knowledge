using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class DictionaryListItemServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string EntityId { get; set; }

        public string EntityName { get; set; }

        public int EntriesCount { get; set; }
    }
}
