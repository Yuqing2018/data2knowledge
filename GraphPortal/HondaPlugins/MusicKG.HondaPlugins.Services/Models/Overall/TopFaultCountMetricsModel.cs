using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Models.Overall
{
    [BsonIgnoreExtraElements]
    public class TopFaultCountMetricsModel
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("_id")]
        public DataSource DataSource { get; set; }
        public IEnumerable<FaultCountMetrics> Results { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class FaultCountMetrics
    {
        [BsonRepresentation(BsonType.String)]
        public DataSource DataSource { get; set; }
        public string CarModel { get; set; }
        public string PartName { get; set; }
        public string Syndrome { get; set; }
        public string SyndromeName { get; set; }
        public double Count { get; set; }
    }
}
