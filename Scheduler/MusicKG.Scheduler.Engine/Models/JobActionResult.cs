namespace MusicKG.Scheduler.Engine.Models
{
    public class JobActionResult
    {
        public string NextActionId { get; private set; }

        public object NextActionData { get; private set; }

        public bool IsFinished { get; private set; }

        public string Message { get; private set; }

        public static JobActionResult NextAction(string nextActionId, object nextActionData, string message)
        {
            return new JobActionResult
            {
                IsFinished = false,
                NextActionId = nextActionId,
                NextActionData = nextActionData,
                Message = message,
            };
        }

        public static JobActionResult Finish(string message)
        {
            return new JobActionResult
            {
                IsFinished = true,
                Message = message
            };
        }
    }
}
