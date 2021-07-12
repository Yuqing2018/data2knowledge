using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.Services.Models.WarningTasks
{
    public class WarningIndexServiceModel
    {
        public WarningIndexServiceModel() { }
        public WarningIndexServiceModel(WarningIndexDataModel data)
        {
            Id = data.Id.ToString();
            WarningType = data.WarningType;
            IndexName = data.IndexName;
            Value = data.Value;
            Unit = data.Unit;
        }

        public string Id { get; set; }
        public WarningType WarningType { get; set; }
        public WarningIndexNames IndexName { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}
