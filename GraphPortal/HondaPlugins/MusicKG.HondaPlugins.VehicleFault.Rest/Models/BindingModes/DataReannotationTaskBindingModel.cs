using System;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class DataReannotationTaskBindingModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }
    }
}
