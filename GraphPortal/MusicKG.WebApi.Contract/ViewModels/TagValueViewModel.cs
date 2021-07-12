using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Tag value view model.
    /// </summary>
    public class TagValueViewModel
    {
        /// <summary>
        /// Tag value Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tag value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Tag color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Tag description.
        /// </summary>
        public string Description { get; set; }
    }
}
