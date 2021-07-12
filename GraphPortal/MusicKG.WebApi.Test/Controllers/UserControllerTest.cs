using MongoDB.Bson;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.BindingModels;
using System.Collections.Generic;
using System.Linq;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Resources;
using MusicKG.Service.Helpers;

namespace MusicKG.WebApi.Test.Controllers
{
    public class UserControllerTest
    {
        private UserController systemUnderTest;
        private Mock<IUserService> serviceMock;

        public UserControllerTest()
        {
            this.serviceMock = new Mock<IUserService>();
        }

        #region Get

        [Fact]
        public async Task GetUsers()
        {
            const int resultCount = 4;

            UserServiceModel[] serviceModel = new UserServiceModel[]
            {
                new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToJson(),
                    Name = "UserName1",
                    Roles = new UserRoleEnum[] { UserRoleEnum.Administrator },
                    Status = UserStatusEnum.Enabled,
                    CreatedBy = "CreateUser1"
                },
                new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToJson(),
                    Name = "UserName2",
                    Roles = new UserRoleEnum[] { UserRoleEnum.Manager },
                    Status = UserStatusEnum.Enabled,
                    CreatedBy = "CreateUser2"
                },
                new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToJson(),
                    Name = "UserName3",
                    Roles = new UserRoleEnum[] { UserRoleEnum.Annotator },
                    Status = UserStatusEnum.Disabled,
                    CreatedBy = "CreateUser3"
                },
                new UserServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToJson(),
                    Name = "UserName4",
                    Roles = new UserRoleEnum[] { UserRoleEnum.Guest },
                    Status = UserStatusEnum.Enabled,
                    CreatedBy = "CreateUser4"
                },
            };

            this.serviceMock.Setup(x => x.GetUsersAsync(It.IsAny<UserRoleEnum?>(), It.IsAny<UserStatusEnum?>(),
                It.IsAny<int>(), It.IsAny<int?>())).Returns(Task.FromResult(
                new Tuple<long, IEnumerable<UserServiceModel>>(resultCount, serviceModel)));

            this.systemUnderTest = new UserController(this.serviceMock.Object);

            var result = await this.systemUnderTest.GetUsers(null, 0, null);

            Assert.NotNull(result);
            Assert.Equal(resultCount, result.Count);
            Assert.Equal(0, result.From);
            Assert.Equal(resultCount, result.TotalCount);
            Assert.NotNull(result.Items);

            var items = result.Items.ToArray();

            Assert.Equal(serviceModel.Length, items.Length);

            for (int i = 0; i < items.Length; i++)
            {
                Assert.Equal(serviceModel[i].Id, items[i].Id);
                Assert.Equal(serviceModel[i].Name, items[i].Name);
                Assert.Equal(serviceModel[i].Roles, items[i].Roles);
                Assert.Equal(serviceModel[i].Status, items[i].Status);
                Assert.Equal(serviceModel[i].CreatedBy, items[i].CreatedBy);
            }
        }

        [Fact]
        public async Task GetUser()
        {
            var userServiceModel = new UserServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "UserName",
                Roles = new UserRoleEnum[] { UserRoleEnum.Administrator },
                Status = UserStatusEnum.Enabled,
                CreatedBy = "CreateUser"
            };

            this.serviceMock.Setup(x => x.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult(userServiceModel));

            this.systemUnderTest = new UserController(this.serviceMock.Object);

            var result = await this.systemUnderTest.GetUser(userServiceModel.Id);

            Assert.Equal(userServiceModel.Id, result.Id);
            Assert.Equal(userServiceModel.Name, result.Name);
            Assert.Equal(userServiceModel.Roles, result.Roles);
            Assert.Equal(userServiceModel.Status, result.Status);
            Assert.Equal(userServiceModel.CreatedBy, result.CreatedBy);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create()
        {
            UserCreateBindingModel bindingModel = new UserCreateBindingModel
            {
                Name = "user",
                Password = "password",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Administrator }
            };

            UserServiceModel serviceModel = new UserServiceModel
            {
                Id = "mockid",
                Name = "user",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Administrator },
                CreatedBy = "CreateUser"
            };

            this.serviceMock.Setup(m => m.CreateUserAsync(It.IsAny<UserCreateServiceModel>())).Returns(Task.FromResult(serviceModel));

            this.systemUnderTest = new UserController(this.serviceMock.Object);

            var result = await this.systemUnderTest.CreateUser(bindingModel);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.NotEmpty(result.Id);
            Assert.Equal(serviceModel.Name, result.Name);
            Assert.True(serviceModel.Roles.All(x => result.Roles.Contains(x)));
            Assert.Equal(serviceModel.CreatedBy, result.CreatedBy);
        }

        #endregion

        #region Update

        #region Positive

        [Fact]
        public async Task Update()
        {
            var bindingModel = new UserUpdateBindingModel
            {
                Name = "name",
                Password = "password",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Administrator },
                Status = UserStatusEnum.Enabled
            };

            var userServiceModel = new UserServiceModel
            {
                Name = bindingModel.Name,
                Roles = bindingModel.Roles,
                Status = (UserStatusEnum)bindingModel.Status,
            };

            serviceMock.Setup(x => x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<UserUpdateServiceModel>())).Returns(Task.FromResult(userServiceModel));

            this.systemUnderTest = new UserController(this.serviceMock.Object);

            var result = await this.systemUnderTest.UpdateUser("userId", bindingModel);

            Assert.Equal(userServiceModel.Name, result.Name);
            Assert.Equal(userServiceModel.Roles, result.Roles);
            Assert.Equal(userServiceModel.Status, result.Status);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task UpdateWithEmptyName()
        {
            var bindingModel = new UserUpdateBindingModel
            {
                Name = "",
                Password = "password",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Administrator },
                Status = UserStatusEnum.Enabled
            };

            this.systemUnderTest = new UserController(this.serviceMock.Object);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.systemUnderTest.UpdateUser(
                ObjectId.GenerateNewId().ToString(), bindingModel));
            Assert.Equal(MusicKGMessages.UserNameEmptyMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateWithEmptyPassword()
        {
            var bindingModel = new UserUpdateBindingModel
            {
                Name = "Name",
                Password = "",
                Roles = new List<UserRoleEnum> { UserRoleEnum.Administrator },
                Status = UserStatusEnum.Enabled
            };

            this.systemUnderTest = new UserController(this.serviceMock.Object);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.systemUnderTest.UpdateUser(
                ObjectId.GenerateNewId().ToString(), bindingModel));
            Assert.Equal(MusicKGMessages.UserPasswordEmptyMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Login

        [Fact]
        public async Task Login()
        {
            DateTime expectedTokenExpiredAt = DateTime.Today;

            var loginBindingModel = new UserLoginBindingModel
            {
                Name = "name",
                Password = "password",
            };

            var loginServiceModel = new UserLoginServiceModel
            {
                Name = loginBindingModel.Name,
                Status = UserStatusEnum.Enabled,
                Roles = new UserRoleEnum[] { UserRoleEnum.Administrator },
                Token = "Token",
                TokenExpiredAt = expectedTokenExpiredAt,
            };

            this.serviceMock.Setup(x => x.LoginAsync(loginBindingModel.Name, loginBindingModel.Password)).Returns(Task.FromResult(loginServiceModel));

            this.systemUnderTest = new UserController(this.serviceMock.Object);
            var result = await this.systemUnderTest.Login(loginBindingModel);

            Assert.NotNull(result);
            Assert.Equal(loginServiceModel.Name, result.Name);
            Assert.Equal(loginServiceModel.Roles, result.Roles);
            Assert.Equal(loginServiceModel.Status, result.Status);
            Assert.Equal(loginServiceModel.Token, result.Token);
            Assert.Equal(loginServiceModel.TokenExpiredAt, result.TokenExpiredAt);
        }

        #endregion

        #region Logout

        [Fact]
        public async Task Logout()
        {
            this.systemUnderTest = new UserController(this.serviceMock.Object);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => this.systemUnderTest.Logout());
            Assert.Equal(MusicKGMessages.UserIdWrongMessage, exception.Message);
        }

        #endregion
    }
}
