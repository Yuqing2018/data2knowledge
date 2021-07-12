using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Statisitics Overview.
    /// </summary>
    public class StatisiticsOverviewViewModel
    {
        /// <summary>
        /// Annotated Task Count.
        /// </summary>
        [Display(Name = "Annotated task count")]
        public long AnnotatedTaskCount { get; set; }

        /// <summary>
        /// Annotated Document Count.
        /// </summary>
        [Display(Name = "Annotated document count")]
        public long AnnotatedDocumentCount { get; set; }

        /// <summary>
        /// Annotated Item Count.
        /// </summary>
        [Display(Name = "Annotated item count")]
        public long AnnotatedItemCount { get; set; }

        /// <summary>
        /// Statisitics for managers.
        /// </summary>
        [Display(Name = "Statisitics for managers")]
        public IEnumerable<StatisiticsViewModel> Managers { get; set; }

        /// <summary>
        /// Statisitics for annotators.
        /// </summary>
        [Display(Name = "Statisitics for annotators")]
        public IEnumerable<StatisiticsViewModel> Annotators { get; set; }

        /// <summary>
        /// Statisitics for inspectors.
        /// </summary>
        [Display(Name = "Statisitics for inspectors")]
        public IEnumerable<StatisiticsViewModel> Inspectors { get; set; }

        /// <summary>
        /// Statisitics for acceptors.
        /// </summary>
        [Display(Name = "Statisitics for acceptors")]
        public IEnumerable<StatisiticsViewModel> Acceptors { get; set; }
    }

    /// <summary>
    /// Statisitics for user.
    /// </summary>
    public class StatisiticsViewModel
    {
        /// <summary>
        /// User.
        /// </summary>
        [Display(Name = "User")]
        public UserViewModel User { get; set; }

        /// <summary>
        /// Statisitics data.
        /// </summary>
        [Display(Name = "Statisitics data")]
        public StatisiticsOverviewDetailsViewModel Statisitics { get; set; }
    }

    /// <summary>
    /// Statisitics details.
    /// </summary>
    public class StatisiticsOverviewDetailsViewModel
    {
        /// <summary>
        /// Created task count.
        /// </summary>
        [Display(Name = "Created task count")]
        public long CreatedTaskCount { get; set; }

        /// <summary>
        /// Finished task count.
        /// </summary>
        [Display(Name = "Finished task count")]
        public long FinishedTaskCount { get; set; }

        /// <summary>
        /// Not finished task count.
        /// </summary>
        [Display(Name = "Not finished task count")]
        public long NotFinishedTaskCount { get; set; }

        /// <summary>
        /// Finished document count.
        /// </summary>
        [Display(Name = "Finished document count")]
        public long FinishedDocumentCount { get; set; }

        /// <summary>
        /// Not finished document count.
        /// </summary>
        [Display(Name = "Not finished document count")]
        public long NotFinishedDocumentCount { get; set; }
    }
}
