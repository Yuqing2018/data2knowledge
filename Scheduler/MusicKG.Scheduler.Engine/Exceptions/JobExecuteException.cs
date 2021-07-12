using System;

namespace MusicKG.Scheduler.Engine.Exceptions
{
    public class JobExecuteException : Exception
    {
        public string ActionId { get; private set; }

        public JobExecuteException(string message, string actionId, Exception innerException = null) : base(message, innerException)
        {
            ActionId = actionId;
        }
    }
}
