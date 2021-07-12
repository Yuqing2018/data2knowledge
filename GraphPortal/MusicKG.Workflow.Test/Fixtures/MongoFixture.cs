using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using MusicKG.Service;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using System.IO;
using System.Threading;

namespace MusicKG.Workflow.Test.Fixtures
{
    public class MongoFixture : IDisposable
    {
        private static string dataDirectory;

        private static int refCount = 0;
        private static MongoDbRunner runner;
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

        public IMusicKGContext Context;

        private readonly string databaseName;

        static MongoFixture()
        {
            dataDirectory = Path.Combine(Path.Combine(Path.GetTempPath(), "mongo2go"), Path.GetRandomFileName());

            runner = MongoDbRunner.Start(dataDirectory: dataDirectory, singleNodeReplSet: true);
        }

        public MongoFixture()
        {
            var runner = Runner;

            databaseName = RandomStringHelper.RandomString(10);

            Context = new MusicKGContext(Options.Create(new MongoDBSettings
            {
                ConnectionString = runner.ConnectionString,
                Database = databaseName
            }), null);
        }

        public void Dispose()
        {
            Context.Client.DropDatabase(databaseName);

            ReleaseRunner();
        }
    }
}
