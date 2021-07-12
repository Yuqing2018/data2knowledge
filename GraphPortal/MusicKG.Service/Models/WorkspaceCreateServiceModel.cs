using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class WorkspaceCreateServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsAutoMerging { get; set; }

        public string Type { get; set; }

        public LanguageEnum Language { get; set; }

        public string CreatedBy { get; set; }

        public List<string> ReadOnlyUsers { get; set; }
    }
}
