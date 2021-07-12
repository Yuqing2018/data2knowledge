using MusicKG.Scheduler.DataAccess;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Service.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Service.MongoDB
{
    public class MongoDBLocker : ILocker
    {
        private readonly ISchedulerDbContext context;
        private readonly LockSettings lockOptions;
        private readonly ILogger<MongoDBLocker> logger;
        private readonly string id;

        private System.Timers.Timer timer;

        public MongoDBLocker(ISchedulerDbContext context,
            LockSettings lockOptions,
            ILogger<MongoDBLocker> logger)
        {
            this.context = context;
            this.lockOptions = lockOptions;
            this.logger = logger;
            this.id = Guid.NewGuid().ToString();
        }

        public async Task ReleaseAsync(string name, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Service '{id}' is trying to release lock '{name}'.");

            try
            {
                await context.Lock.DeleteOneAsync(l => l.Name.Equals(name) && l.Holder.Equals(id), cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Service '{id}' failed to release lock '{name}'.");
            }
            finally
            {
                try
                {
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Close();
                        timer.Dispose();

                        logger.LogInformation($"Service '{id}' stopped heartbeat timer for lock {name}.");
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Service '{id}' failed to stop timer for lock {name}.");
                }
            }

            logger.LogInformation($"Service '{id}' released lock '{name}'.");
        }

        public async Task<bool> TryLockAsync(string name, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Service '{id}' is trying to get lock from task '{name}'.");

            var lockOptions = this.lockOptions;

            var database = context.Database;

            var current = await GetMongoDbCurrentTime(database);

            if (current == null)
                return false;

            var filterBuilder = Builders<Lock>.Filter;

            var filter = filterBuilder.Eq(l => l.Name, name) &
                filterBuilder.Lt(l => l.LastSeenAt, current.Value.AddSeconds(-lockOptions.LivnessFailureThreshold * lockOptions.LivnessProbePeriodSeconds));

            var writes = new List<WriteModel<Lock>>
            {
                new DeleteOneModel<Lock>(filter),
                new InsertOneModel<Lock>(new Lock
                {
                    Name = name,
                    Holder = id,
                    IpAddress = GetLocalIP(),
                    CreatedAt = current.Value,
                    LastSeenAt = current.Value
                })
            };

            try
            {
                await context.Lock.BulkWriteAsync(writes);

                timer = new System.Timers.Timer(lockOptions.LivnessProbePeriodSeconds * 1000);
                timer.Elapsed += async (s, e) => await Heartbeat(name, cancellationToken);
                timer.Start();

                logger.LogInformation($"Service '{id}' started heartbeat timer for lock '{name}'.");
                logger.LogInformation($"Service '{id}' got lock '{name}'.");

                return true;
            }
            catch (MongoBulkWriteException e)
            {
                if (e.WriteErrors != null && e.WriteErrors.All(we => we.Category == ServerErrorCategory.DuplicateKey))
                    logger.LogInformation($"Service '{id}' failed to get lock '{name}'.");
                else
                    logger.LogError(e, $"Service '{id}' failed to get lock '{name}' with error");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Service '{id}' failed to get lock '{name}' with error.");
            }

            return false;
        }

        private async Task Heartbeat(string name, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Service '{id}' start heartbeat for lock '{name}'.");

            try
            {
                var update = Builders<Lock>.Update.CurrentDate(l => l.LastSeenAt);

                await context.Lock.UpdateOneAsync(l => l.Name.Equals(name) && l.Holder.Equals(id), update, cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Service '{id}' heartbeat failed for lock '{name}'.");
            }

            logger.LogInformation($"Service '{id}' finish heartbeat for lock '{name}'.");
        }

        private string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName();
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to get local ip");
            }

            return null;
        }

        private async Task<DateTime?> GetMongoDbCurrentTime(IMongoDatabase database)
        {
            return DateTime.UtcNow;
        }
    }
}
