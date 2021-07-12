using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class UserServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserRoleEnum> Roles { get; set; }

        public UserStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}
