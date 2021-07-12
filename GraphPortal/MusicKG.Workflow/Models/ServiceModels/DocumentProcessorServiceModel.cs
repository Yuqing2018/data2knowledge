using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Models.ServiceModels
{
    public class DocumentProcessorServiceModel
    {
        public string DocumentId { get; set; }

        public DocumentProcessStatusEnum Status { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Message { get; set; }
    }
}
