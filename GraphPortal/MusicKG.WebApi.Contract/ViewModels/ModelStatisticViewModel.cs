using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Model statistic view model.
    /// </summary>
    public class ModelStatisticViewModel
    {
        /// <summary>
        /// Model name.
        /// </summary>
        [Display(Name = "Dictionary name")]
        public string Name { get; set; }

        /// <summary>
        /// Training file count.
        /// </summary>
        [Display(Name = "Training file count")]
        public int TrainingFileCount { get; set; }

        /// <summary>
        /// Training duration in seconds.
        /// </summary>
        [Display(Name = "Training duration in seconds")]
        public int TrainingDuration { get; set; }

        /// <summary>
        /// Created at.
        /// </summary>
        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }
    }
}
