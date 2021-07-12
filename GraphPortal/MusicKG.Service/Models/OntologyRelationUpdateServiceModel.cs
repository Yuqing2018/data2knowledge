using MusicKG.DataAccess.Models;
using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class OntologyRelationUpdateServiceModel
    {
        public string Name { get; set; }
        public bool IsNameAssigned { get; set; }
        
        public List<OntologyRelationPropertyServiceModel> Properties { get; set; }
        public bool IsPropertiesAssigned { get; set; }

        public string SecondEntityId { get; set; }
        public bool IsSecondEntityAssigned { get; set; }

        public string Description { get; set; }
        public bool IsDescriptionAssigned { get; set; }
    }
}
