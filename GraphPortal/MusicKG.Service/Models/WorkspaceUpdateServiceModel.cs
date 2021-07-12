using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class WorkspaceUpdateServiceModel
    {
        public string Name { get; set; }

        public bool IsNameAssigned { get; set; }

        public string Description { get; set; }

        public bool IsDescriptionAssigned { get; set; }

        public bool? IsAutoMerging { get; set; }

        public List<string> ReadOnlyUserIds { get; set; }
    }
}
