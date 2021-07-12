using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Rule list item view model.
    /// </summary>
    public class RuleListItemViewModel
    {
        /// <summary>
        /// Workspace ID.
        /// </summary>
        [Display(Name = "Workspace ID")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// Rule ID.
        /// </summary>
        [Display(Name = "Rule ID")]
        public string Id { get; set; }

        /// <summary>
        /// Rule name.
        /// </summary>
        [Display(Name = "Rule name")]
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
    }
}
