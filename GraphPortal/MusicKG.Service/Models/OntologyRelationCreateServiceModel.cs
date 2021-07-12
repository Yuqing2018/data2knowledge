using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class OntologyRelationCreateServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Name { get; set; }

        public string FirstEntityId { get; set; }

        public string SecondEntityId { get; set; }

        public string Description { get; set; }
    }
}
