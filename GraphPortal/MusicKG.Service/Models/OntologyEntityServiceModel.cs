using MongoDB.Bson;
using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class OntologyEntityServiceModel
    {
        public string WorkspaceId { get; set; }
        
        public string Id { get; set; }

        public string Name { get; set; }
        
        public List<OntologyEntityPropertyServiceModel> Properties { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }
    }
}
