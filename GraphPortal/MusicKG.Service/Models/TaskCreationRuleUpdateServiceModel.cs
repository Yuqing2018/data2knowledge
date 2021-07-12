using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class TaskCreationRuleUpdateServiceModel
    {
        public string CreateUser { get; set; }

        public List<AutoTaskRuleServiceModel> Rules { get; set; }
    }
}
