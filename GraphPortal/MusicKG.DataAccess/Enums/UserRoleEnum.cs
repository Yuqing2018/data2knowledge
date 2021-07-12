using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// User role.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRoleEnum
    {
        /// <summary>
        /// Administrator.
        /// </summary>
        Administrator = 0,

        /// <summary>
        /// Manager.
        /// </summary>
        Manager = 1,

        /// <summary>
        /// Annotator.
        /// </summary>
        Annotator = 2,

        /// <summary>
        /// Guest.
        /// </summary>
        Guest = 3,

        /// <summary>
        /// ReadOnly.
        /// </summary>
        ReadOnly = 4,

        /// <summary>
        /// Inspector.
        /// </summary>
        Inspector = 5,

        /// <summary>
        /// Acceptor.
        /// </summary>
        Acceptor = 6
    }
}
