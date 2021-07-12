using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class OntologyDownloadServiceModel
    {
        public List<OntologyEntityServiceModel> Entities { get; set; }

        public List<OntologyRelationDownloadServiceModel> Relations { get; set; }
    }
}
