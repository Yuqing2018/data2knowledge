using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels
{
    public class SyndromeListViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public BadGrade? BadGrade { get; set; }
    }
}
