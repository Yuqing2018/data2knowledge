using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class DocumentUploadServiceModel
    {
        public string Name { get; set; }

        public List<string> Tags { get; set; }

        public byte[] Content { get; set; }

        public long ItemCount { get; set; }

        public string ContentType { get; set; }

        public string UploadedBy { get; set; }

        public string ParentDocumentId { get; set; }
    }
}
