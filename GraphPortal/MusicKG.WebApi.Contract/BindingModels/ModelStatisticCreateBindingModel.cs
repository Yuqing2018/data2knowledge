using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Model statistic binding model.
    /// </summary>
    public class ModelStatisticCreateBindingModel
    {
        /// <summary>
        /// Model name.
        /// </summary>
        [Display(Name = "Model name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Training file count.
        /// </summary>
        [Display(Name = "Training file count")]
        [Required]
        public int TrainingFileCount { get; set; }

        /// <summary>
        /// Training duration in seconds.
        /// </summary>
        [Display(Name = "Training duration in seconds")]
        [Required]
        public int TrainingDuration { get; set; }

        /// <summary>
        /// Created at.
        /// </summary>
        [Display(Name = "Created at")]
        public DateTime? CreatedAt { get; set; }
    }
}
