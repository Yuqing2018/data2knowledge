using System;
using System.Collections.Generic;
using System.Text;
using MusicKG.DataAccess.Enums;

namespace MusicKG.Service.Models
{
    public class TaskDocumentServiceModel
    {
        public DocumentServiceModel Document { get; set; }

        public TaskDocumentStatusEnum Status { get; set; }

        public string AnnotatorId { get; set; }

        public DateTime LatestResultSavedAt { get; set; }
    }
}
