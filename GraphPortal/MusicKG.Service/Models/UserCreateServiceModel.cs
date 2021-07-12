using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class UserCreateServiceModel
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public List<UserRoleEnum> Roles { get; set; }

        public string CreatedBy { get; set; }
    }
}
