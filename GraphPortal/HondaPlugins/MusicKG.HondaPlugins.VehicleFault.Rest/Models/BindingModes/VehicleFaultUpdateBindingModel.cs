namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class VehicleFaultUpdateBindingModel
    {
        public string PartName { get; set; }

        public string Syndrome { get; set; }

        public bool IsAddToTraining { get; set; }
    }
}
