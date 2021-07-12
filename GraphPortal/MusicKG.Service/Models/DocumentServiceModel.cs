using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class DocumentServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WorkspaceId { get; set; }

        public string ContentType { get; set; }

        public DocumentStatusEnum Status { get; set; }

        public List<string> Tags { get; set; }

        public UserServiceModel UploadedBy { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
