using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Service.Models;
using System;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Engine.Action
{
    public interface IJobAction
    {
        string ActionType { get; }

        Task<JobActionResult> ExecuteAsync(JobActionDataModel jobAction, object actionData, int actionRetryTimes);

        Task RevertAsync(JobActionDataModel jobAction);

        JobTaskContext CreateContext(DateTime from, DateTime to, JobTaskServiceModel taskDefine);

        void SetContext(JobTaskContext context);
    }
}
