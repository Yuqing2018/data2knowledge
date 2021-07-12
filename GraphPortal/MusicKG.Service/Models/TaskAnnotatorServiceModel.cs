using System;
using System.Collections.Generic;
using System.Text;
using MusicKG.DataAccess.Enums;

namespace MusicKG.Service.Models
{
    public class TaskAnnotatorServiceModel
    {
        public UserServiceModel Annotator { get; set; }

        public bool IsManager { get; set; }

        public IEnumerable<TaskDocumentServiceModel> TaskDocuments { get; set; }
    }
}
