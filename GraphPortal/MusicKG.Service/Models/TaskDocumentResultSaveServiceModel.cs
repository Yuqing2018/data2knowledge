using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class TaskDocumentResultSaveServiceModel
    {
        /// <summary>
        /// Task document result document ID.
        /// </summary>
        public string ResultDocumentId { get; set; }

        /// <summary>
        /// Task document result type.
        /// </summary>
        public TaskDocumentResultTypeEnum ResultType { get; set; }

        /// <summary>
        /// Task document status.
        /// </summary>
        public TaskDocumentStatusEnum Status { get; set; }
    }
}
