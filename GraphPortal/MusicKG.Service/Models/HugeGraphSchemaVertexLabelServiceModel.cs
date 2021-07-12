using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class HugeGraphSchemaVertexLabelServiceModel
    {
        public HugeGraphSchemaVertexLabelServiceModel(string name, List<string> properties, List<string> primaryKeys, List<string> nullableKeys)
        {
            Name = name;
            Properties = properties;
            PrimaryKeys = primaryKeys;
            NullableKeys = nullableKeys;
        }

        /// <summary>
        /// Vertex name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Vertex properties.
        /// </summary>
        public List<string> Properties { get; set; }

        /// <summary>
        /// Vertex primary keys comes from properties.
        /// </summary>
        public List<string> PrimaryKeys { get; set; }

        /// <summary>
        /// Vertex nullable Keys comes from properties and can not contain primary keys.
        /// </summary>
        public List<string> NullableKeys { get; set; }
    }

}
