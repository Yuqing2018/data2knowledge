using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MusicKG.Scheduler.Rest.Models.BindingModels
{
    public class JobBindingModel : JobBindingModelBase
    {
        public List<ActionBindingModel> Actions { get;set; }
    }

    public class ActionBindingModel
    {
        public string ActionId { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }

        public JObject Options { get; set; }
    }
}
