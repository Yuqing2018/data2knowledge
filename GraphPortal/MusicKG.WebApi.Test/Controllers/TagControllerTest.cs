using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Moq;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.WebApi.Test.Controllers
{
    public class TagControllerTest
    {
        public TagControllerTest()
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UploadTagValuesTest(bool @override)
        {
            var file = new Mock<IFormFile>();
            var values = Enumerable.Range(1, new Random().Next(100, 500)).Select(i => RandomStringHelper.RandomString(10)).ToList();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, values))))
            {
                stream.Position = 0;
                file.Setup(f => f.OpenReadStream()).Returns(stream);
                file.Setup(f => f.FileName).Returns("TestFile");
                file.Setup(f => f.Length).Returns(stream.Length);
                file.Setup(x => x.ContentType).Returns("text/plain");

                var workspaceId = ObjectId.GenerateNewId().ToString();
                var type = TagTypeEnum.None;

                var serviceMock = new Mock<ITagService>();

                serviceMock.Setup(x => x.AddTagValuesAsync(It.IsAny<TagServiceModel>())).Returns(Task.CompletedTask);
                serviceMock.Setup(x => x.ReplaceTagAsync(It.IsAny<TagServiceModel>())).Returns(Task.CompletedTask);

                var controller = new TagController(serviceMock.Object);

                await controller.UploadTagValuesAsync(workspaceId, type, @override, file.Object);

                if (@override)
                {
                    serviceMock.Verify(x => x.ReplaceTagAsync(It.IsAny<TagServiceModel>()), Times.Once);
                }
                else
                {
                    serviceMock.Verify(x => x.AddTagValuesAsync(It.IsAny<TagServiceModel>()), Times.Once);
                }
            }
        }
    }
}
