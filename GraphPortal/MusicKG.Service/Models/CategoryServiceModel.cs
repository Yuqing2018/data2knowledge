using System;

namespace MusicKG.Service.Models
{
    public class CategoryServiceModel
    {
        public string Id { get; set; }

        public string WorkspaceId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}
