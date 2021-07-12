using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class DialogOntologyCreateServiceModel
    {
        public string CurrentId { get; set; }

        public string DialogEntityDocumentId { get; set; }

        public string DialogIntentDocumentId { get; set; }
    }
}
