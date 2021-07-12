using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.DataManager.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Labeling
{
    public class AnnotationDocumentItem : AnnotationItemModel
    {
        public string RawId { get; set; }

        public string CarModel { get; set; }

        public string CarType { get; set; }

        public string ModelYear { get; set; }

        public string FrameNo { get; set; }

        public int MileAge { get; set; }

        public DateTime ProductionDate { get; set; }

        public DateTime InitialRegistDate { get; set; }

        public DateTime FaultDate { get; set; }

        public string DealerCD { get; set; }

        public string DealerName { get; set; }

        public string Region { get; set; }

        public string Province { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float CostRepair { get; set; } = 0f;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PartNo { get; set; }

        public List<VehicleFaultRelatedDataModel> RelatedInfo { get; set; }

        public Dictionary<string, ItemFeature> Features { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MarkResult OriginalResult { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MarkResult ModelResult { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MarkResult Result { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime SyncTimestamp { get; set; }

        [JsonIgnore]
        public string DataSourceName { get; set; }
    }
}