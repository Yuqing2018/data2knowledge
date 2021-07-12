using System;

namespace MusicKG.Scheduler.DataAccess.Models
{
    public class Lock
    {
        public string Name { get; set; }

        public string Holder { get; set; }

        public string IpAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastSeenAt { get; set; }
    }
}
