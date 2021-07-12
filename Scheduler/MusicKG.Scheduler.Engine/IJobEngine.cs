using MusicKG.Scheduler.Service.Models;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Engine
{
    public interface IJobEngine
    {
        Task ExecuteAsync(JobServiceModel job);
    }
}
