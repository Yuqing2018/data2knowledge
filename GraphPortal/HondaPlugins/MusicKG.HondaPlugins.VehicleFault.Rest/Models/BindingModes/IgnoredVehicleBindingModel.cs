using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class IgnoredVehicleBindingModel
    {
        [Required]
        public string CarModel { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> CarTypes { get; set; }
    }
}
