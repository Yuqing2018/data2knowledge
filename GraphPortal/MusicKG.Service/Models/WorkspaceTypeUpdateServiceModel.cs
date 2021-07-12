using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class WorkspaceTypeUpdateServiceModel
    {
        public string Name { get; set; }

        public bool IsNameAssigned { get; set; }

        public WorkspaceTypeStatusEnum Status { get; set; }

        public bool IsStatusAssigned { get; set; }
    }
}
