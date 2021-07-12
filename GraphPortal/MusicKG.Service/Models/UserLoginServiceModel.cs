using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class UserLoginServiceModel : UserServiceModel
    {
        public string Token { get; set; }

        public DateTime TokenExpiredAt { get; set; }
    }
}
