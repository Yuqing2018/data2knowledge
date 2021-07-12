using System;

namespace MusicKG.Scheduler.Service.Models
{
    public class JobTaskExecuteResultServiceModel
    {
        public DateTime RunAt { get; set; }

        public bool IsSucceed { get; set; }

        public string DeadAction { get; set; }

        public string Message { get; set; }
    }
}
