using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Dialog Ontology View Model.
    /// </summary>
    public class DialogOntologyViewModel
    {
        /// <summary>
        /// Intent document Id.
        /// </summary>
        public string IntentDocumentId { get; set; }

        /// <summary>
        /// Entity document Id.
        /// </summary>
        public string EntityDocumentId { get; set; }
    }
}
