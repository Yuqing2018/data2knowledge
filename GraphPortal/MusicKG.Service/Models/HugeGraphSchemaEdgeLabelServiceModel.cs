using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Models
{
    public class HugeGraphSchemaEdgeLabelServiceModel
    {
        public HugeGraphSchemaEdgeLabelServiceModel(string name, string sourceLabel, string targetLabel, List<string> properties, List<string> nullableKeys)
        {
            Name = name;
            SourceLabel = sourceLabel;
            TargetLabel = targetLabel;
            Properties = properties;
            NullableKeys = nullableKeys;
        }

        /// <summary>
        /// Edge name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Edge source vertex name.
        /// </summary>
        public string SourceLabel { get; set; }

        /// <summary>
        /// Edge target vertex name.
        /// </summary>
        public string TargetLabel { get; set; }

        /// <summary>
        /// Edge properties.
        /// </summary>
        public List<string> Properties { get; set; }

        /// <summary>
        /// Edge nullable keys from properties.
        /// </summary>
        public List<string> NullableKeys { get; set; }
    }
}
