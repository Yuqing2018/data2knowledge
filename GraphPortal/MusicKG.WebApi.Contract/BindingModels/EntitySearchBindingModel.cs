using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Entity search binding model.
    /// </summary>
    public class EntitySearchBindingModel
    {
        /// <summary>
        /// Query string.
        /// </summary>
        [Display(Name = "Query string")]
        [Required]
        public string Query { get; set; }
    }
}
