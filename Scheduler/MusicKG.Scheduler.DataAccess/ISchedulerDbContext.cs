using MusicKG.Scheduler.DataAccess.Models;
using MongoDB.Driver;

namespace MusicKG.Scheduler.DataAccess
{
    public interface ISchedulerDbContext
    {
        IMongoDatabase Database { get; }

        IMongoCollection<Lock> Lock { get; }

        IMongoCollection<JobDataModel> Jobs { get; }

        IMongoCollection<JobTaskDataModel> JobTasks { get; }
    }
}
