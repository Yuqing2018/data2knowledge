using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MusicKG.Service.Models;
using MusicKG.Service.Implementations;
using Microsoft.Extensions.Options;
using MusicKG.Service.Settings;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using Moq;
using System.Threading;
using MusicKG.Service.Helpers;
using MusicKG.Service.Test.Fixtures;
using MusicKG.DataAccess;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    public class GridFSStorageServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;
        private readonly IGridFSBucket bucket;

        public GridFSStorageServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            context = mongoFixture.Context;
            bucket = context.Bucket;
        }

        [Fact]
        public async Task CreateFile()
        {
            byte[] content = Encoding.UTF8.GetBytes(RandomStringHelper.RandomString(20));

            var serviceModel = new DocumentStoreServiceModel
            {
                Content = content,
                ContentType = "text/plain",
                ContentMd5 = HashHelper.GetMD5Hash(content)
            };

            var service = new GridFSStorageService(context, null);

            await service.Create(serviceModel);

            var file = await bucket.Find(Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, serviceModel.ContentMd5)).FirstOrDefaultAsync();
            Assert.NotNull(file);
            Assert.Equal(serviceModel.ContentMd5, file.Filename);
            Assert.Equal(serviceModel.ContentType, file.Metadata["contentType"]);

            var contentReturned = await bucket.DownloadAsBytesByNameAsync(serviceModel.ContentMd5, new GridFSDownloadByNameOptions
            {
                Revision = -1
            });

            Assert.Equal(content, contentReturned);
        }

        [Fact]
        public async Task CreateSameFileTwice()
        {
            byte[] content = Encoding.UTF8.GetBytes(RandomStringHelper.RandomString(20));

            var serviceModel = new DocumentStoreServiceModel
            {
                Content = content,
                ContentType = "text/plain",
                ContentMd5 = HashHelper.GetMD5Hash(content)
            };

            var service = new GridFSStorageService(context, null);

            await service.Create(serviceModel);
            await service.Create(serviceModel);

            var file = await bucket.Find(Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, serviceModel.ContentMd5)).FirstOrDefaultAsync();
            Assert.NotNull(file);
            Assert.Equal(serviceModel.ContentMd5, file.Filename);
            Assert.Equal(serviceModel.ContentType, file.Metadata["contentType"]);

            var contentReturned = await bucket.DownloadAsBytesByNameAsync(serviceModel.ContentMd5, new GridFSDownloadByNameOptions
            {
                Revision = -1
            });

            Assert.Equal(content, contentReturned);
        }

        [Fact]
        public async Task ReadFile()
        {
            byte[] content = Encoding.UTF8.GetBytes(RandomStringHelper.RandomString(20));
            string contentMD5 = HashHelper.GetMD5Hash(content);

            await bucket.UploadFromBytesAsync(contentMD5, content);

            var service = new GridFSStorageService(context, null);

            var contentReturned = await service.Read(contentMD5);

            Assert.Equal(content, contentReturned);
        }

        [Fact]
        public async Task ReadNotExistingFile()
        {
            var service = new GridFSStorageService(context, null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.Read("abcdef"));
            Assert.Equal(MusicKGMessages.FileContentNotExistMessage, exception.Message);
        }
    }
}
