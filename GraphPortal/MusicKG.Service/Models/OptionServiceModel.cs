using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Models
{
    public class OptionServiceModel
    {
        public OptionTypeEnum Type { get; set; }

        public string Value { get; set; }

        public string DisplayName { get; set; }
    }
}
