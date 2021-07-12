using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    /// <summary>
    /// Syndrome bingding model.
    /// </summary>
    public class SyndromeBindingModel
    {
        /// <summary>
        /// Syndrome name.
        /// </summary>
        [Display(Name = "Syndrome name")]
        public string Name { get; set; }
        /// <summary>
        ///Syndrome bad grade. 
        /// </summary>
        [Display(Name = "Syndrome bad grade")]
        public BadGrade? BadGrade { get; set; }
    }
}
