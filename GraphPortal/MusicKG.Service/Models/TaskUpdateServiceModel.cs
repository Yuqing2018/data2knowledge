using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class TaskUpdateServiceModel
    {
        public string Name { get; set; }

        public bool IsNameAssigned { get; set; }

        public DateTime? ExpectedDueAt { get; set; }
    }
}
