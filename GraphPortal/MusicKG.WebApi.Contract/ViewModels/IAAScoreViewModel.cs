using MusicKG.WebApi.Contract.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// IAA score view model.
    /// </summary>
    public class IAAScoreViewModel
    {
        /// <summary>
        /// Task ID.
        /// </summary>
        [Display(Name = "Task ID")]
        public string TaskId { get; set; }

        /// <summary>
        /// Task name.
        /// </summary>
        [Display(Name = "Task name")]
        public string TaskName { get; set; }

        /// <summary>
        /// Object type.
        /// </summary>
        [Display(Name = "Object type")]
        public IAAScoreObjectTypeEnum ObjectType;


        /// <summary>
        /// View type.
        /// </summary>
        [Display(Name = "View type")]
        public IAAScoreViewTypeEnum ViewType;

        /// <summary>
        /// Columns.
        /// </summary>
        [Display(Name = "Columns")]
        public IEnumerable<string> Columns { get; set; }

        /// <summary>
        /// Scores.
        /// </summary>
        [Display(Name = "Scores")]
        public IEnumerable<IEnumerable<string>> Scores { get; set; }

        /// <summary>
        /// Total scores.
        /// </summary>
        [Display(Name = "Total scores")]
        public IEnumerable<string> TotalScores { get; set; }
    }
}
