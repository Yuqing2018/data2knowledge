using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.DataAccess.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Net.Sockets;

namespace MusicKG.Scheduler.DataAccess
{
    public class SchedulerDbContext : ISchedulerDbContext
    {
        private readonly SchedulerDBSettings options;

        public SchedulerDbContext(SchedulerDBSettings options, ILogger<SchedulerDbContext> logger)
        {
            this.options = options;

            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);

            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);

            settings.ClusterConfigurator = builder =>
            {
                // Enable TCP keep alive to relief MongoConnectionException.
                void SocketConfigurator(Socket s) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                builder.ConfigureTcp(tcp => tcp.With(socketConfigurator: (Action<Socket>)SocketConfigurator));

                // Log MongoDB statements.
                builder.Subscribe<CommandStartedEvent>(e =>
                {
                    logger?.LogDebug($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };

            Setup(new MongoClient(settings), options.Database, logger);
        }

        public IMongoClient Client { get; private set; }

        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<Lock> Lock { get; private set; }

        public IMongoCollection<JobDataModel> Jobs { get; private set; }

        public IMongoCollection<JobTaskDataModel> JobTasks { get; private set; }

        private void Setup(
            IMongoClient client,
            string database,
            ILogger<SchedulerDbContext> logger = null)
        {
            Client = client;
            Database = Client.GetDatabase(database);

            GetCollections();

            EnsureIndexes();
        }

        private void GetCollections()
        {
            Lock = Database.GetCollection<Lock>(options.LockerCollectionName);
            Jobs = Database.GetCollection<JobDataModel>(options.JobDefineCollectionName);
            JobTasks = Database.GetCollection<JobTaskDataModel>(options.JobTaskCollectionName);
        }

        private void EnsureIndexes()
        {
            Lock.Indexes.CreateOne(new CreateIndexModel<Lock>(Builders<Lock>.IndexKeys.Ascending(_ => _.Name),
                new CreateIndexOptions { Unique = true, Background = true }));

            Jobs.Indexes.CreateOne(new CreateIndexModel<JobDataModel>(Builders<JobDataModel>.IndexKeys.Ascending(_ => _.Name),
                new CreateIndexOptions { Unique = true, Background = true }));

            JobTasks.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<JobTaskDataModel>(Builders<JobTaskDataModel>.IndexKeys.Ascending(_ => _.JobId).Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }),
                new CreateIndexModel<JobTaskDataModel>(Builders<JobTaskDataModel>.IndexKeys.Ascending(_ => _.JobId),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<JobTaskDataModel>(Builders<JobTaskDataModel>.IndexKeys.Descending(_ => _.ManagedAt),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<JobTaskDataModel>(Builders<JobTaskDataModel>.IndexKeys.Descending(_ => _.Status.LastRunAt),                    new CreateIndexOptions { Background = true }),
            });
        }
    }
}
