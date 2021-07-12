using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class TagServiceModel
    {
        public string WorkspaceId { get; set; }

        public TagTypeEnum Type { get; set; }

        public List<TagValueServiceModel> Values { get; set; }
    }
}
