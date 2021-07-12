using System;

namespace MusicKG.HondaPlugins.Services.Models.ReannotationTasks
{
    public class DataReannotationTaskManageServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string ManagedBy { get; set; }
    }
}
