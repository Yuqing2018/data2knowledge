using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class StatisticsServiceModel
    {
        public long CreatedTaskCount { get; set; }

        public long FinishedTaskCount { get; set; }

        public long NotFinishedTaskCount { get; set; }

        public long FinishedDocumentCount { get; set; }

        public long NotFinishedDocumentCount { get; set; }
    }
}
