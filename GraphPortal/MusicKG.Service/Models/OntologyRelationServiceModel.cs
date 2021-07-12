using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class OntologyRelationServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string FirstEntityId { get; set; }

        public string SecondEntityId { get; set; }

        public List<OntologyRelationPropertyServiceModel> Properties { get; set; }

        public string Description { get; set; }
    }
}