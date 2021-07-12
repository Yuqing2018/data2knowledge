using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class TaskDocumentResultServiceModel
    {
        /// <summary>
        /// Document ID.
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// Document status.
        /// </summary>
        public TaskDocumentStatusEnum TaskDocumentStatus { get; set; }

        /// <summary>
        /// Task document result type.
        /// </summary>
        public TaskDocumentResultTypeEnum ResultType { get; set; }

        /// <summary>
        /// Task document result document ID.
        /// </summary>
        public string ResultDocumentId { get; set; }

        /// <summary>
        /// Created by.
        /// </summary>
        public UserServiceModel CreatedBy { get; set; }

        /// <summary>
        /// Created at.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
