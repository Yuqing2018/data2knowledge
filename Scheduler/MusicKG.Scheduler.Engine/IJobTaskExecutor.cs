using MusicKG.Scheduler.Service.Models;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Engine
{
    public interface IJobTaskExecutor
    {
        Task<JobTaskExecuteResultServiceModel> ExecuteAsync(JobServiceModel job, JobTaskServiceModel task);
    }
}
