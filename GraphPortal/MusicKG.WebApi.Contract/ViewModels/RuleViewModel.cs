using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Rule view model.
    /// </summary>
    public class RuleViewModel
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

        /// <summary>
        /// Regular expressions.
        /// </summary>
        [Display(Name = "Regular expressions")]
        public IEnumerable<RegexViewModel> Regexes { get; set; }
    }
}
