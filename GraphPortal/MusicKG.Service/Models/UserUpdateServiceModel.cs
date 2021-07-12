using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class UserUpdateServiceModel
    {
        public string Name { get; set; }

        public bool IsNameAssigned { get; set; }

        public string Password { get; set; }

        public bool IsPasswordAssigned { get; set; }

        public List<UserRoleEnum> Roles { get; set; }

        public bool IsRolesAssigned { get; set; }

        public UserStatusEnum? Status { get; set; }
    }
}
