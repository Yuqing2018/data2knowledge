using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class OntologyRelationListItemServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string FirstEntityId { get; set; }

        public string FirstEntityName { get; set; }

        public string SecondEntityId { get; set; }

        public string SecondEntityName { get; set; }

        public string Description { get; set; }
    }
}
