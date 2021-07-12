using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MusicKG.Scheduler.Service.Extensions
{
    public static class JObjectExtensions
    {
        public static BsonDocument ToBsonDocument(this JObject @object)
        {
            IDictionary<string, object> elems = new Dictionary<string, object>();

            foreach (var item in @object)
                elems[item.Key] = ToBsonValue(item.Value);

            return new BsonDocument(elems);
        }

        public static BsonArray ToBsonArray(this JArray array)
        {
            IList<BsonValue> bvs = new List<BsonValue>();
            foreach (JToken item in array)
                bvs.Add(ToBsonValue(item));
            return new BsonArray(bvs);
        }

        private static BsonValue ToBsonValue(this JToken val)
        {
            if (val is JArray)
                return ToBsonArray(val as JArray);
            if (val is JObject)
                return ToBsonDocument(val as JObject);
            else
                return ToBasicValue(val);
        }

        private static BsonValue ToBasicValue(JToken @object)
        {
            switch (@object.Type)
            {
                case JTokenType.Integer:
                    return BsonValue.Create(@object.ToObject<int>());
                case JTokenType.Float:
                    return BsonValue.Create(@object.ToObject<float>());
                case JTokenType.String:
                    return BsonValue.Create(@object.ToObject<string>());
                case JTokenType.Boolean:
                    return BsonValue.Create(@object.ToObject<bool>());
                case JTokenType.Date:
                    return BsonValue.Create(@object.ToObject<DateTime>());
                case JTokenType.Guid:
                    return BsonValue.Create(@object.ToObject<Guid>());
                case JTokenType.Null:
                    return null;
                default:
                    throw new Exception("");
            }
        }
    }
}
