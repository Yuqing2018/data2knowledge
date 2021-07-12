using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Settings
{
    public class StorageTypeSettings
    {
        public const string GridFSKey = "StorageTypes:GridFS";
        public const string S3Key = "StorageTypes:S3";

        /// <summary>
        /// True to enable GridFS.
        /// </summary>
        public bool GridFS { get; set; }

        /// <summary>
        /// True to enable S3.
        /// </summary>
        public bool S3 { get; set; }

        public StorageTypeSettings(IConfiguration configuration)
        {
            GridFS = "true".Equals(configuration.GetSection(GridFSKey)?.Value?.ToLower());
            S3 = "true".Equals(configuration.GetSection(S3Key)?.Value?.ToLower());
        }
    }
}
