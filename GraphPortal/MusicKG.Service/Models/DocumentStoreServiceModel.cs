using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class DocumentStoreServiceModel
    {
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public string ContentMd5 { get; set; }
    }
}
