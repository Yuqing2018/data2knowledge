using System;
using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class DictionaryServiceModel
    {
        public string Id { get; set; }

        public string WorkspaceId { get; set; }

        public string Name { get; set; }

        public string EntityId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<string> Vocabularies { get; set; }

    }
}
