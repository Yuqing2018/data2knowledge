using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class OntologyRelationDownloadServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string FirstEntityId { get; set; }

        public string FirstEntityName { get; set; }

        [JsonIgnore]
        public string SecondEntityId { get; set; }

        public string SecondEntityName { get; set; }

        public List<OntologyRelationPropertyServiceModel> Properties { get; set; }

        public string Description { get; set; }
    }
}