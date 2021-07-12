using MusicKG.Scheduler.DataAccess;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Service.Models;
using MusicKG.Scheduler.Service.Resources;
using MusicKG.Scheduler.Service.Helpers;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;

namespace MusicKG.Scheduler.Service.MongoDB
{
    public class MongoDBJobService : IJobService
    {
        private readonly ISchedulerDbContext context;
        private readonly ILogger<MongoDBJobService> logger;

        public MongoDBJobService(
            ISchedulerDbContext context,
            ILogger<MongoDBJobService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<List<JobServiceModel>> ListAsync(List<string> jobNames)
        {
            var querable = context.Jobs.AsQueryable();

            if (jobNames != null && jobNames.Count > 0)
                querable = querable.Where(j => jobNames.Contains(j.Name));

            var result = await querable.ToListAsync();

            return result?.Select(j => new JobServiceModel
            {
                Id = j.Id,
                Name = j.Name,
                Description = j.Description,
                Schedule = j.Schedule,
                JobType = j.JobType,
                Actions = j.Actions?.Select(a => new JobActionServiceModel
                {
                    ActionId = a.ActionId,
                    Description = a.Description,
                    IsDefault = a.IsDefault,
                    Options = a.Options
                })?.ToList(),
                LastRunAt = j.LastRunAt
            })?.ToList();
        }

        public async Task<JobServiceModel> GetAsync(string id)
        {
            var result = await context.Jobs.AsQueryable().FirstOrDefaultAsync(j => j.Id == id);

            if (result == null)
                ErrorHelper.ThrowException(SchedulerMessages.JobNotExistMessage, HttpStatusCode.NotFound);

            return new JobServiceModel
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Schedule = result.Schedule,
                JobType = result.JobType,
                Actions = result.Actions?.Select(a => new JobActionServiceModel
                {
                    ActionId = a.ActionId,
                    Description = a.Description,
                    IsDefault = a.IsDefault,
                    Options = a.Options
                })?.ToList(),
                LastRunAt = result.LastRunAt
            };
        }

        public async Task<JobServiceModel> GetByNameAsync(string name)
        {
            var result = await context.Jobs.AsQueryable().FirstOrDefaultAsync(j => j.Name == name);

            if (result == null)
                ErrorHelper.ThrowException(SchedulerMessages.JobNotExistMessage, HttpStatusCode.NotFound);

            return new JobServiceModel
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Schedule = result.Schedule,
                JobType = result.JobType,
                Actions = result.Actions?.Select(a => new JobActionServiceModel
                {
                    ActionId = a.ActionId,
                    Description = a.Description,
                    IsDefault = a.IsDefault,
                    Options = a.Options
                })?.ToList(),
                LastRunAt = result.LastRunAt
            };
        }

        public async Task CreateAsync(JobManagementServiceModel job)
        {
            var dataModel = new JobDataModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = job.Name,
                Description = job.Description,
                JobType = job.JobType,
                LastRunAt = null,
                Schedule = job.Schedule,
                Actions = job.Actions?.Select(a => new JobActionDataModel
                {
                    ActionId = a.ActionId,
                    Description = a.Description,
                    IsDefault = a.IsDefault,
                    Options = a.Options
                })?.ToList()
            };

            await context.Jobs.InsertOneAsync(dataModel);
        }

        public async Task UpdateAsync(string id, JobManagementServiceModel job)
        {
            var currentJob = await context.Jobs.AsQueryable().FirstOrDefaultAsync(j => j.Id == id);

            if (currentJob == null)
                ErrorHelper.ThrowException(SchedulerMessages.JobNotExistMessage, HttpStatusCode.NotFound);

            currentJob.Name = job.Name;
            currentJob.Description = job.Description;
            currentJob.Schedule = job.Schedule;
            currentJob.JobType = job.JobType;
            currentJob.Actions = job.Actions?.Select(a => new JobActionDataModel
            {
                ActionId = a.ActionId,
                Description = a.Description,
                IsDefault = a.IsDefault,
                Options = a.Options
            })?.ToList();

            await context.Jobs.ReplaceOneAsync(j => j.Id == id, currentJob);
        }

        public async Task UpdateRunTimeAsync(string id, DateTime runTime)
        {
            var builder = Builders<JobDataModel>.Update;

            var update = builder.Set(j => j.LastRunAt, runTime);

            await context.Jobs.UpdateOneAsync(j => j.Id == id, update);
        }
    }
}
