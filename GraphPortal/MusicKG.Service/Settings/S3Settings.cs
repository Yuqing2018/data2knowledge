using Microsoft.Extensions.Configuration;
using MusicKG.Service.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Settings
{
    public class S3Settings
    {
        public const string S3ServerUrlKey = "S3:ServiceUrl";
        public const string S3AccessKeyIdKey = "S3:AccessKeyId";
        public const string S3AccessKeySecretKey = "S3:AccessKeySecret";
        public const string S3BucketKey = "S3:Bucket";

        /// <summary>
        /// S3 service URL.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// S3 access key.
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// S3 access secret.
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        /// S3 bucket.
        /// </summary>
        public string Bucket { get; set; }

        public void ParseFrom(IConfiguration configuration)
        {
            this.ServiceUrl = configuration.GetSection(S3ServerUrlKey)?.Value;
            this.AccessKeyId = configuration.GetSection(S3AccessKeyIdKey)?.Value;
            this.AccessKeySecret = configuration.GetSection(S3AccessKeySecretKey)?.Value;
            this.Bucket = configuration.GetSection(S3BucketKey)?.Value;
        }
    }
}
