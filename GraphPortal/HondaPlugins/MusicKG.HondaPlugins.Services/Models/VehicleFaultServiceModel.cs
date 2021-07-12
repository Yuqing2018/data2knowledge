using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class VehicleFaultServiceModel : VehicleFaultListServiceModel
    {
        public List<VehicleFaultRelatedDataModel> RelatedInfo { get; set; }

        public Dictionary<string, string> Features { get; set; }
    }
}
