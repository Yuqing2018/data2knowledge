using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Implementations
{
    public class GridFSStorageService : IStorageService
    {
        private readonly IMusicKGContext context;
        private readonly IGridFSBucket bucket;
        private readonly ILogger<GridFSStorageService> logger;

        public GridFSStorageService(
            IMusicKGContext context,
            ILogger<GridFSStorageService> logger)
        {
            this.context = context;
            bucket = context.Bucket;
            this.logger = logger;
        }

        public async Task Create(DocumentStoreServiceModel serviceModel)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, serviceModel.ContentMd5);

            var file = await bucket.Find(filter).FirstOrDefaultAsync();
            if (file != null)
                return;

            await bucket.UploadFromBytesAsync(serviceModel.ContentMd5, serviceModel.Content, new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "contentType", serviceModel.ContentType }
                }
            });
        }

        public async Task<byte[]> Read(string filename)
        {
            byte[] content = null;

            try
            {
                content = await bucket.DownloadAsBytesByNameAsync(filename, new GridFSDownloadByNameOptions
                {
                    Revision = -1
                });
            }
            catch (Exception e)
            {
                var message = MusicKGMessages.FileContentNotExistMessage;
                var statusCode = HttpStatusCode.NotFound;

                if (!(e is GridFSFileNotFoundException))
                {
                    message = MusicKGMessages.GetFileContentFailedMessage;
                    statusCode = HttpStatusCode.InternalServerError;
                }

                logger?.LogError(e, message);

                ErrorHelper.ThrowException(message, statusCode);
            }

            if (content == null)
                ErrorHelper.ThrowException(MusicKGMessages.GetFileContentFailedMessage, HttpStatusCode.InternalServerError);

            return content;
        }
    }
}
