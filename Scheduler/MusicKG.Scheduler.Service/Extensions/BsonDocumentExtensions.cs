using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MusicKG.Scheduler.Service.Extensions
{
    public static class BsonDocumentExtensions
    {
        public static JObject ToJObject(this BsonDocument bsonDocument)
        {
            JObject elems = new JObject();
            foreach (var item in bsonDocument)
            {
                elems[item.Name] = ToJsonValue(item.Value);
            }
            return elems;
        }

        private static JToken ToJsonValue(this BsonValue val)
        {
            if (val is BsonArray)
                return ToJArray(val as BsonArray);
            if (val is BsonDocument)
                return ToJObject(val as BsonDocument);
            if (val is BsonNull)
                return null;
            return JToken.FromObject(val);
        }

        public static JArray ToJArray(this BsonArray array)
        {
            IList<JToken> bvs = new List<JToken>();
            foreach (var item in array)
                bvs.Add(ToJsonValue(item));
            return new JArray(bvs);
        }
    }
}
