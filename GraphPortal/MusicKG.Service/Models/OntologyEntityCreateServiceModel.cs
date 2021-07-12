using MusicKG.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class OntologyEntityCreateServiceModel
    {
        public string WorkspaceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }
    }
}
