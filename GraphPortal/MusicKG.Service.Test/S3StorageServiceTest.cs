//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using MusicKG.Service.Models;
//using MusicKG.Service.Implementations;
//using Microsoft.Extensions.Options;
//using MusicKG.Service.Settings;
//using Amazon.S3;
//using Amazon.S3.Model;
//using System.IO;
//using Moq;
//using System.Threading;
//using MusicKG.Service.Helpers;
//using MusicKG.Service.Resources;

//namespace MusicKG.Service.Test
//{
//    public class S3StorageServiceTest
//    {
//        private readonly IStorageService serviceUnderTest;
//        private readonly S3Settings storageSettings;

//        private readonly AmazonS3Config config;

//        public S3StorageServiceTest()
//        {
//            this.storageSettings = new S3Settings
//            {
//                ServiceUrl = "http://d2kdemo.eastasia.cloudapp.azure.com:8080",
//                AccessKeyId = "IH7IE2XW9Q998P2UH59E",
//                AccessKeySecret = "WZChO6QkDUtkyo5Uj8LMHaOatQfBeWiBVH5dmwB1",
//                Bucket = "businessai"
//            };
//            this.serviceUnderTest = new S3StorageService(Options.Create(this.storageSettings), null);

//            this.config = new AmazonS3Config
//            {
//                ServiceURL = this.storageSettings.ServiceUrl,
//                ForcePathStyle = true,
//                SignatureVersion = "2",
//                SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA1
//            };
//        }

//        [Fact]
//        public async Task CreateFile()
//        {
//            byte[] fileContent = Encoding.UTF8.GetBytes(RandomStringHelper.RandomString(20));

//            byte[] actualContent;

//            var serviceModel = new DocumentStoreServiceModel
//            {
//                Content = fileContent,
//                ContentType = "text/plain",
//                ContentMd5 = HashHelper.GetMD5Hash(fileContent)
//            };

//            await this.serviceUnderTest.Create(serviceModel);

//            using (var amazonS3Client = new AmazonS3Client(
//                this.storageSettings.AccessKeyId,
//                this.storageSettings.AccessKeySecret,
//                this.config))
//            {
//                try
//                {
//                    var getObjectResponse = await amazonS3Client.GetObjectAsync(new GetObjectRequest
//                    {
//                        BucketName = this.storageSettings.Bucket,
//                        Key = serviceModel.ContentMd5
//                    });

//                    using (var stream = getObjectResponse.ResponseStream)
//                    using (MemoryStream ms = new MemoryStream())
//                    {
//                        await stream.CopyToAsync(ms);
//                        actualContent = ms.ToArray();
//                    }
//                }
//                catch
//                {
//                    actualContent = new byte[0];
//                }
//                finally
//                {
//                    await this.RemoveTestData(serviceModel.ContentMd5);
//                }
//                Assert.Equal(fileContent, actualContent);
//            }
//        }

//        [Fact]
//        public async Task ReadFile()
//        {
//            byte[] fileContent = Encoding.UTF8.GetBytes(RandomStringHelper.RandomString(20));
//            string contentMD5 = HashHelper.GetMD5Hash(fileContent);

//            byte[] actualFileContent;

//            await this.PrepareFileAsync(contentMD5, fileContent);

//            actualFileContent = await this.serviceUnderTest.Read(contentMD5);

//            await this.RemoveTestData(contentMD5);

//            Assert.Equal(fileContent, actualFileContent);
//        }

//        [Fact]
//        public async Task ReadNotExistingFile()
//        {
//            byte[] fileContent = Encoding.UTF8.GetBytes(RandomStringHelper.RandomString(20));
//            string contentMD5 = HashHelper.GetMD5Hash(fileContent);

//            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.serviceUnderTest.Read(contentMD5));
//            Assert.Equal(MusicKGMessages.FileContentNotExistMessage, exception.Message);
//        }

//        private async Task RemoveTestData(string contentMD5)
//        {
//            using (var amazonS3Client = new AmazonS3Client(
//                this.storageSettings.AccessKeyId,
//                this.storageSettings.AccessKeySecret,
//                this.config))
//            {
//                await amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
//                {
//                    BucketName = this.storageSettings.Bucket,
//                    Key = contentMD5
//                });
//            }
//        }

//        private async Task PrepareFileAsync(string contentMD5, byte[] content)
//        {
//            using (var amazonS3Client = new AmazonS3Client(
//                this.storageSettings.AccessKeyId,
//                this.storageSettings.AccessKeySecret,
//                this.config))
//            {
//                using (var stream = new MemoryStream(content))
//                {
//                    PutObjectRequest putObjectRequest = new PutObjectRequest
//                    {
//                        BucketName = this.storageSettings.Bucket,
//                        Key = contentMD5,
//                        ContentType = "text/plain",
//                        InputStream = stream
//                    };

//                    await amazonS3Client.PutObjectAsync(putObjectRequest);
//                }
//            }
//        }
//    }
//}
