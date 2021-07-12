using Microsoft.Extensions.Configuration;
using MusicKG.DataAccess.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Settings
{
    public class MongoDBSettings
    {
        public const string ConnectionStringKey = "MongoDB:ConnectionString";
        public const string DatabaseKey = "MongoDB:Database";
        public const string BucketKey = "MongoDB:Bucket";

        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public string Bucket { get; set; }

        public void ParseFrom(IConfiguration configuration)
        {
            ConnectionString = configuration.GetSection(ConnectionStringKey).Value;
            Database = configuration.GetSection(DatabaseKey).Value;
            Bucket = configuration.GetSection(BucketKey)?.Value;
        }
    }
}
