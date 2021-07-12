using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class TagValueUpdateServiceModel
    {
        public string Value { get; set; }

        public bool IsValueAssigned { get; set; }

        public string Description { get; set; }

        public bool IsDescriptionAssigned { get; set; }

        public string Color { get; set; }

        public bool IsColorAssigned { get; set; }
    }
}
