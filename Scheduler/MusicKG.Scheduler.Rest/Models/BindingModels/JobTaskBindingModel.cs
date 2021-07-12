using Newtonsoft.Json.Linq;

namespace MusicKG.Scheduler.Rest.Models.BindingModels
{
    public class JobTaskBindingModel : JobTaskBindingModelBase
    {
        public JObject TaskDefine { get; set; }
    }
}
