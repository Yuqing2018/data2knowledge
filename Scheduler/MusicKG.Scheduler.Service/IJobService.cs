using MusicKG.Scheduler.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Service
{
    public interface IJobService
    {
        Task<List<JobServiceModel>> ListAsync(List<string> jobNames);

        Task<JobServiceModel> GetAsync(string id);

        Task<JobServiceModel> GetByNameAsync(string name);

        Task CreateAsync(JobManagementServiceModel job);

        Task UpdateAsync(string id, JobManagementServiceModel job);

        Task UpdateRunTimeAsync(string id, DateTime runTime);
    }
}
