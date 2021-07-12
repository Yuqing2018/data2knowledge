using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.Service.Settings;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// S3 file storage service.
    /// </summary>
    public class S3StorageService : IStorageService
    {
        private readonly IOptions<S3Settings> options;

        private readonly AmazonS3Client amazonS3Client;
        private readonly string bucket;

        private readonly ILogger<S3StorageService> logger;

        /// <summary>
        /// S3 file storage service constructor.
        /// </summary>
        /// <param name="options">S3 file storage service settings.</param>
        public S3StorageService(
            IOptions<S3Settings> options,
            ILogger<S3StorageService> logger)
        {
            this.options = options;
            this.logger = logger;

            bucket = this.options.Value.Bucket;

            AmazonS3Config config = new AmazonS3Config
            {
                ServiceURL = this.options.Value.ServiceUrl,
                ForcePathStyle = true,
                SignatureVersion = "2",
                SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA1
            };

            amazonS3Client = new AmazonS3Client(
                this.options.Value.AccessKeyId,
                this.options.Value.AccessKeySecret,
                config
            );
        }

        /// <summary>
        /// Create file on storage.
        /// </summary>
        /// <param name="serviceModel">Document store service model.</param>
        /// <returns></returns>
        public async Task Create(DocumentStoreServiceModel serviceModel)
        {
            bool isObjectExisting = true;
            try
            {
                GetObjectMetadataRequest getObjectMetadataRequest = new GetObjectMetadataRequest
                {
                    BucketName = bucket,
                    Key = serviceModel.ContentMd5
                };
                GetObjectMetadataResponse getObjectMetadataResponse = await amazonS3Client.GetObjectMetadataAsync(getObjectMetadataRequest);
            }
            catch (AmazonS3Exception e)
            {
                if(e.StatusCode == HttpStatusCode.NotFound)
                    isObjectExisting = false;
            }

            if (!isObjectExisting)
            {
                using (var stream = new MemoryStream(serviceModel.Content))
                {
                    PutObjectRequest putObjectRequest = new PutObjectRequest
                    {
                        BucketName = bucket,
                        Key = serviceModel.ContentMd5,
                        ContentType = serviceModel.ContentType,
                        InputStream = stream,
                        MD5Digest = HashHelper.GetMD5HashBase64(serviceModel.Content)
                    };

                    await amazonS3Client.PutObjectAsync(putObjectRequest);
                }
            }
        }

        /// <summary>
        /// Download document from storage.
        /// </summary>
        /// <param name="filename">The key of the document in storage.</param>
        /// <returns>File content.</returns>
        public async Task<byte[]> Read(string filename)
        {
            GetObjectResponse getObjectResponse = null;
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = filename
                };
                getObjectResponse = await amazonS3Client.GetObjectAsync(getObjectRequest);
            }
            catch (Exception e)
            {
                var message = MusicKGMessages.FileContentNotExistMessage;
                var statusCode = HttpStatusCode.NotFound;

                if (!(e is AmazonS3Exception) || ((AmazonS3Exception)e).StatusCode != HttpStatusCode.NotFound)
                {
                    message = MusicKGMessages.GetFileContentFailedMessage;
                    statusCode = HttpStatusCode.InternalServerError;
                } 

                logger?.LogError(e, message);

                ErrorHelper.ThrowException(message, statusCode);
            }

            if (getObjectResponse == null)
                ErrorHelper.ThrowException(MusicKGMessages.GetFileContentFailedMessage, HttpStatusCode.InternalServerError);

            using (var stream = getObjectResponse.ResponseStream)
            using (MemoryStream ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Close S3 client.
        /// </summary>
        public void Dispose()
        {
            amazonS3Client.Dispose();
        }
    }
}
