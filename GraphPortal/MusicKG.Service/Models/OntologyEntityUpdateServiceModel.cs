using MusicKG.DataAccess.Models;
using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class OntologyEntityUpdateServiceModel
    {
        public string Name { get; set; }
        public bool IsNameAssigned { get; set; }
        
        public List<OntologyEntityPropertyServiceModel> Properties { get; set; }
        public bool IsPropertiesAssigned { get; set; }
        
        public string Description { get; set; }
        public bool IsDescriptionAssigned { get; set; }

        public string Color { get; set; }
        public bool IsColorAssigned { get; set; }
    }
}
