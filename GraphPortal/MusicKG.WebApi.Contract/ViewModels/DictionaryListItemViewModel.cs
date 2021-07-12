using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Dictionary list item view model.
    /// </summary>
    public class DictionaryListItemViewModel
    {
        /// <summary>
        /// Workspace ID.
        /// </summary>
        [Display(Name = "Workspace ID")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// Dictionary ID.
        /// </summary>
        [Display(Name = "Dictionary ID")]
        public string Id { get; set; }

        /// <summary>
        /// Dictionary name.
        /// </summary>
        [Display(Name = "Dictionary name")]
        public string Name { get; set; }

        /// <summary>
        /// Ontology entity ID.
        /// </summary>
        [Display(Name = "Ontology entity ID")]
        public string EntityId { get; set; }

        /// <summary>
        /// Ontology entity name.
        /// </summary>
        [Display(Name = "Ontology entity name")]
        public string EntityName { get; set; }

        /// <summary>
        /// Dictionary entries count.
        /// </summary>
        [Display(Name = "Dictionary entries count")]
        public int EntriesCount { get; set; }
    }
}
