using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class HugeGraphSchemaPropertyKeyServiceModel
    {
        public HugeGraphSchemaPropertyKeyServiceModel(string name, string dataType)
        {
            Name = name;
            Datatype = dataType;
            IsMultiValue = dataType.EndsWith("List");
        }

        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property datatype.
        /// </summary>
        public string Datatype { get; set; }

        /// <summary>
        /// If the datatype is multiple.
        /// </summary>
        public bool IsMultiValue { get; set; }
    }
}
