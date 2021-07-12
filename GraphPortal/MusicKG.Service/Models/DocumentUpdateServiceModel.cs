using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class DocumentUpdateServiceModel
    {
        public string Name { get; set; }

        public bool IsNameAssigned { get; set; }

        public List<string> Tags { get; set; }

        public bool IsTagsAssigned { get; set; }

        public DocumentStatusEnum Status { get; set; }

        public bool IsStatusAssigned { get; set; }
    }
}
