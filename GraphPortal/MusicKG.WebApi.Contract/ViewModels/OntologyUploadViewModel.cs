using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    public class OntologyUploadViewModel
    {
        public List<OntologyEntityViewModel> Entities { get; set; }

        public List<OntologyRelationViewModel> Relations { get; set; }
    }
}
