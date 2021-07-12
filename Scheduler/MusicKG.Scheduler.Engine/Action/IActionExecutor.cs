using MusicKG.Scheduler.Engine.Models;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Engine.Action
{
    public interface IActionExecutor
    {
        public string ExecutorType { get; }

        Task<object> ExecuteAsync(string actionId, JobTaskContext context, JobActionOptions options, object data);

        Task RevertAsync(string actionId, JobTaskContext context);
    }
}
