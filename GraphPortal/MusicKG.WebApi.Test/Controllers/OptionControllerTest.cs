using Moq;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.DataAccess.Enums;
using Microsoft.Extensions.Localization;

namespace MusicKG.WebApi.Test.Controllers
{
    public class OptionControllerTest
    {
        private OptionController systemUnderTest;
        private Mock<IOptionService> optionServiceMock;
        private Mock<IStringLocalizer<OptionController>> localizerMock;

        public OptionControllerTest()
        {
            this.optionServiceMock = new Mock<IOptionService>();
            this.localizerMock = new Mock<IStringLocalizer<OptionController>>();
            this.localizerMock.Setup(l => l[It.IsAny<string>()]).Returns<LocalizedString>(n => new LocalizedString(n, n));
            this.systemUnderTest = new OptionController(this.optionServiceMock.Object, localizerMock.Object);
        }

        [Fact]
        public async Task GetAllOptions()
        {
            List<OptionServiceModel> rawData = this.SetupOptionService(true);
            IEnumerable<OptionViewModel> actualData = await this.systemUnderTest.GetOptions(null);
            this.VerifyResult(rawData.ToArray(), actualData.ToArray());
        }

        [Fact]
        public async Task GetFilteredOptions()
        {
            List<OptionServiceModel> rawData = this.SetupOptionService(false);
            IEnumerable<OptionViewModel> actualData = await this.systemUnderTest.GetOptions(OptionTypeEnum.UserRole);
            this.VerifyResult(rawData.ToArray(), actualData.ToArray());
        }

        private List<OptionServiceModel> SetupOptionService(bool setupForAll)
        {
            IEnumerable<OptionServiceModel> result = new List<OptionServiceModel>();
            if (setupForAll)
            {
                result.ToList().AddRange(this.GenerateEnumData(typeof(UserRoleEnum), OptionTypeEnum.UserRole));
                this.optionServiceMock.Setup(x => x.GetOptions(null)).Returns(Task.FromResult(result));
            }
            else
            {
                result.ToList().AddRange(this.GenerateEnumData(typeof(UserRoleEnum), OptionTypeEnum.UserRole));
                this.optionServiceMock.Setup(x => x.GetOptions(OptionTypeEnum.UserRole)).Returns(Task.FromResult(result));
            }
            return result.ToList();
        }

        private IEnumerable<OptionServiceModel> GenerateEnumData(Type enumType, OptionTypeEnum optionType)
        {
            List<OptionServiceModel> result = new List<OptionServiceModel>();
            foreach (string name in enumType.GetEnumNames())
            {
                result.Add(new OptionServiceModel
                {
                    Type = optionType,
                    DisplayName = name,
                    Value = name
                });
            }
            return result;
        }

        private void VerifyResult(OptionServiceModel[] rawData, OptionViewModel[] actualData)
        {
            Assert.NotNull(actualData);
            Assert.Equal(rawData.Length, actualData.Length);
            for (int i = 0; i < rawData.Count(); i++)
            {
                Assert.Equal(rawData[i].Type, actualData[i].Type);
                Assert.Equal(rawData[i].Value, actualData[i].Value);
                Assert.Equal(rawData[i].DisplayName, actualData[i].DisplayName);
            }
        }
    }
}
