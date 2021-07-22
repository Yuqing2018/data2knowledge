using Mongo2Go;
using System;
using System.IO;
using System.Threading;
using MusicKG.HondaPlugins.Services.Test.Helpers;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.Settings;

namespace MusicKG.HondaPlugins.Services.Test.Fixtures
{
    public class HondaMongoFixture : IDisposable
    {
        private static string dataDirectory = Path.Combine(Path.Combine(Path.GetTempPath(), "mongo2go"), Path.GetRandomFileName());

        private static int refCount = 0;
        private static MongoDbRunner runner = MongoDbRunner.Start(dataDirectory: dataDirectory, singleNodeReplSet: true);
        public static MongoDbRunner Runner
        {
            get
            {
                Interlocked.Increment(ref refCount);

                return runner;
            }
        }

        public static void ReleaseRunner()
        {
            Interlocked.Decrement(ref refCount);

            if (refCount == 0)
            {
                runner.Dispose();
            }  
        }

        public IHondaMongoDbContext Context;
        public IRawDbContext RawContext;
        private readonly string databaseName;

        static HondaMongoFixture()
        {
            dataDirectory = Path.Combine(Path.Combine(Path.GetTempPath(), "mongo2go"), Path.GetRandomFileName());

            runner = MongoDbRunner.Start(dataDirectory: dataDirectory, singleNodeReplSet: true);
        }

        public HondaMongoFixture()
        {
            var runner = Runner;

            databaseName = StringHelpers.RandomString(10);
            var options = new HondaMongoDbSettings
            {
                ConnectionString = runner.ConnectionString,
                Database = databaseName
            };
            Context = new HondaMongoDbContext(options);
            var rawDbOption = new MariaRawDataDbSettings()
            {
                ConnectionString = $"server=127.0.0.1;port=3306;user=root;password=123456;database=unit_test"
            };

            this.RawContext = new RawDbContext(rawDbOption);
        }

        public void Dispose()
        {
            Context.Client.DropDatabase(databaseName);

            ReleaseRunner();
        }
    }
}
