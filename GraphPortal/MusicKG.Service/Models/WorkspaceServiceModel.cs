using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class WorkspaceServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsAutoMerging { get; set; }

        public WorkspaceTypeServiceModel Type { get; set; }

        public LanguageEnum Language { get; set; }

        public UserServiceModel CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<UserServiceModel> ReadOnlyUsers { get; set; }
    }
}
