using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Labeling
{
    [BsonIgnoreExtraElements]
    public class IgnoredVehicle
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonDictionaryOptions(Representation = DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<string, List<string>> Data { get; set; }
    }
}
