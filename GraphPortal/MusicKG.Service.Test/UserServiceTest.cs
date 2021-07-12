using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using MusicKG.Service.Test.Fixtures;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MusicKG.Service.Settings;
using Moq;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    public class UserServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private IMusicKGContext context;

        public UserServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
        }

        #region Create

        #region Positive

        [Fact]
        public async Task Create()
        {
            var createServiceModel = new UserCreateServiceModel
            {
                Name = "name",
                Password = "password"
            };

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var userServiceModel = await service.CreateUserAsync(createServiceModel);

            Assert.NotNull(userServiceModel);
            Assert.NotEmpty(userServiceModel.Id.ToString());
            Assert.Equal(createServiceModel.Name, userServiceModel.Name);
            Assert.NotEmpty(userServiceModel.CreatedAt.ToString());

            var userDataModel = await context.Users.Find(u => u.Id == new ObjectId(userServiceModel.Id)).FirstOrDefaultAsync();

            Assert.NotNull(userDataModel);
            Assert.NotEmpty(userDataModel.Id.ToString());
            Assert.Equal(createServiceModel.Name, userDataModel.Name);
            Assert.NotEmpty(userDataModel.Salt);
            Assert.Equal(UserService.HashPassword(userDataModel.Salt, createServiceModel.Password), userDataModel.Password);
            Assert.NotNull(userDataModel.Tokens);
            Assert.Empty(userDataModel.Tokens);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task CreateWithExistedName()
        {
            const string existedUserName = "Name";
            var existedUser = await PrepareData(existedUserName, UserRoleEnum.Administrator, UserStatusEnum.Enabled, "CreateUser");

            var loggerMock = new Mock<ILogger<UserService>>();

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), loggerMock.Object);

            var createServiceModel = new UserCreateServiceModel
            {
                Name = existedUserName,
                Password = "password"
            };

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.CreateUserAsync(createServiceModel));
            Assert.Equal(MusicKGMessages.UserNameExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Update

        #region Positive

        [Fact]
        public async Task Update()
        {
            const string existedUserName = "Name";
            var userExisting = await PrepareData(existedUserName, UserRoleEnum.Administrator, UserStatusEnum.Enabled, "CreateUser1");

            var updateServiceModel = new UserUpdateServiceModel
            {
                Name = "newname",
                IsNameAssigned = true,
                Password = "newpassword",
                IsPasswordAssigned = true,
                Roles = new List<UserRoleEnum> { UserRoleEnum.Manager },
                IsRolesAssigned = true,
                Status = UserStatusEnum.Disabled
            };

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var userServiceModel = await service.UpdateUserAsync(userExisting.Id.ToString(), updateServiceModel);
            Assert.NotNull(userServiceModel);
            Assert.Equal(userServiceModel.Name, updateServiceModel.Name);

            var userDataModel = await context.Users.Find(u => u.Id == userExisting.Id).FirstOrDefaultAsync();

            Assert.NotNull(userDataModel);
            Assert.Equal(updateServiceModel.Name, userDataModel.Name);
            Assert.NotNull(userDataModel.Tokens);
            Assert.Empty(userDataModel.Tokens);
            Assert.Equal(updateServiceModel.Roles, userDataModel.Roles);
            Assert.Equal(updateServiceModel.Status, userDataModel.Status);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task UpdateUserWithNonExistedId()
        {
            var updateServiceModel = new UserUpdateServiceModel
            {
                Name = "Name",
                IsNameAssigned = true,
                Password = "newpassword",
                IsPasswordAssigned = true
            };

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.UpdateUserAsync(ObjectId.GenerateNewId().ToString(), updateServiceModel));
            Assert.Equal(MusicKGMessages.UserNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Delete

        #region Positive

        [Fact]
        public async Task Delete()
        {
            const string existedUserName = "Name";
            var userExisting = await PrepareData(existedUserName, UserRoleEnum.Administrator, UserStatusEnum.Enabled, "CreateUser");

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            await service.DeleteUserAsync(userExisting.Id.ToString());

            var count = await this.context.Users.CountDocumentsAsync(x => x.Id == userExisting.Id);

            Assert.Equal(0, count);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task DeleteWithNonExistedId()
        {
            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.DeleteUserAsync(ObjectId.GenerateNewId().ToString()));
            Assert.Equal(MusicKGMessages.UserNotExistMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Login

        #region Positive

        [Fact]
        public async Task Login()
        {
            var name = "name";
            var password = "password";
            var salt = RandomStringHelper.RandomString(256);
            var userExisting = new UserDataModel
            {
                Name = name,
                Salt = salt,
                Password = UserService.HashPassword(salt, password),
                Tokens = new List<UserTokenDataModel>()
            };
            await context.Users.InsertOneAsync(userExisting);

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var userLoginServiceModel1 = await service.LoginAsync(name, password);
            Assert.NotNull(userLoginServiceModel1);
            Assert.Equal(name, userLoginServiceModel1.Name);
            Assert.NotEmpty(userLoginServiceModel1.Token);
            Assert.Equal(1, userLoginServiceModel1.TokenExpiredAt.CompareTo(DateTime.UtcNow));

            var userDataModel1 = await context.Users.Find(u => u.Id == userExisting.Id).FirstOrDefaultAsync();
            Assert.NotNull(userDataModel1);
            Assert.Equal(name, userDataModel1.Name);
            Assert.Single(userDataModel1.Tokens);
            Assert.NotNull(userDataModel1.Tokens[0]);
            Assert.NotNull(userDataModel1.Tokens[0].Token);
            Assert.Equal(1, userDataModel1.Tokens[0].ExpiredAt.CompareTo(DateTime.UtcNow));
            Assert.Equal(userLoginServiceModel1.Token, userDataModel1.Tokens[0].Token);

            var userLoginServiceModel2 = await service.LoginAsync(name, password);
            Assert.NotNull(userLoginServiceModel2);
            Assert.Equal(name, userLoginServiceModel2.Name);
            Assert.NotEmpty(userLoginServiceModel2.Token);
            Assert.Equal(1, userLoginServiceModel2.TokenExpiredAt.CompareTo(DateTime.UtcNow));

            var userDataModel2 = await context.Users.Find(u => u.Id == userExisting.Id).FirstOrDefaultAsync();
            Assert.NotNull(userDataModel2);
            Assert.Equal(name, userDataModel2.Name);
            Assert.NotNull(userDataModel2.Tokens);
            Assert.Equal(2, userDataModel2.Tokens.Count);
            var tokenDataModel = userDataModel2.Tokens.Where(t => t.Token.Equals(userLoginServiceModel2.Token)).ToList();
            Assert.Single(tokenDataModel);
            Assert.NotNull(tokenDataModel[0].Token);
            Assert.Equal(1, tokenDataModel[0].ExpiredAt.CompareTo(DateTime.UtcNow));
        }

        #endregion

        #region Negative

        [Fact]
        public async Task LoginTooManyTimes()
        {
            var name = "name";
            var password = "password";
            var salt = RandomStringHelper.RandomString(256);
            var userExisting = new UserDataModel
            {
                Name = name,
                Salt = salt,
                Password = UserService.HashPassword(salt, password),
                Tokens = new List<UserTokenDataModel>()
            };
            await context.Users.InsertOneAsync(userExisting);

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                MaxExistingValidLoginsPerUser = 0,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var ex = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.LoginAsync(name, password));
            Assert.Equal(MusicKGMessages.UserLoginCountOverflowMessage, ex.Message);
            Assert.Equal(HttpStatusCode.Unauthorized, ex.Data["statusCode"]);
        }

        [Fact]
        public async Task LoginWithWrongUsernameOrPassword()
        {
            var name = "name";
            var password = "password";
            var salt = RandomStringHelper.RandomString(256);
            var userExisting = new UserDataModel
            {
                Name = name,
                Salt = salt,
                Password = UserService.HashPassword(salt, password),
                Tokens = new List<UserTokenDataModel>()
            };
            await context.Users.InsertOneAsync(userExisting);

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var ex1 = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.LoginAsync("wrongname", password));
            Assert.Equal(MusicKGMessages.UserNamePasswordWrongMessage, ex1.Message);
            Assert.Equal(HttpStatusCode.Unauthorized, ex1.Data["statusCode"]);

            var ex2 = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.LoginAsync(name, "wrongpassword"));
            Assert.Equal(MusicKGMessages.UserNamePasswordWrongMessage, ex2.Message);
            Assert.Equal(HttpStatusCode.Unauthorized, ex2.Data["statusCode"]);
        }

        [Fact]
        public async Task LoginWithDisabledUser()
        {
            var name = "name";
            var password = "password";
            var salt = RandomStringHelper.RandomString(256);
            var userExisting = new UserDataModel
            {
                Name = name,
                Salt = salt,
                Status = UserStatusEnum.Disabled,
                Password = UserService.HashPassword(salt, password),
                Tokens = new List<UserTokenDataModel>()
            };
            await context.Users.InsertOneAsync(userExisting);

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var ex1 = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.LoginAsync(name, password));
            Assert.Equal(MusicKGMessages.UserDisabledMessage, ex1.Message);
            Assert.Equal(HttpStatusCode.Unauthorized, ex1.Data["statusCode"]);
        }

        #endregion

        #endregion

        #region Authorize

        #region Positive

        [Fact]
        public async Task AuthorizeWithToken()
        {
            var name = "name";
            var password = "password";
            var salt = RandomStringHelper.RandomString(256);
            var expiredAt = DateTime.UtcNow.AddDays(1);

            var userExisting = new UserDataModel
            {
                Name = name,
                Salt = salt,
                Password = UserService.HashPassword(salt, password)
            };

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var token = service.GenerateJwtToken(userExisting, expiredAt);

            userExisting.Tokens = new List<UserTokenDataModel>
            {
                new UserTokenDataModel
                {
                    Token = token,
                    ExpiredAt = expiredAt,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.InsertOneAsync(userExisting);

            var userServiceModel = service.Authorize(token, null, false, null, userExisting.Id.ToString());
            Assert.NotNull(userServiceModel);
            Assert.Equal(name, userServiceModel.Name);
        }

        [Fact]
        public async Task AuthorizeWithRoles()
        {
            var name = "name";
            var password = "password";
            var salt = RandomStringHelper.RandomString(256);
            var expiredAt = DateTime.UtcNow.AddDays(1);

            var userExisting = new UserDataModel
            {
                Name = name,
                Salt = salt,
                Roles = new List<UserRoleEnum> { UserRoleEnum.Administrator },
                Password = UserService.HashPassword(salt, password)
            };

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var token = service.GenerateJwtToken(userExisting, expiredAt);

            userExisting.Tokens = new List<UserTokenDataModel>
            {
                new UserTokenDataModel
                {
                    Token = token,
                    ExpiredAt = expiredAt,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.InsertOneAsync(userExisting);

            var userServiceModel1 = service.Authorize(token, new List<UserRoleEnum> { UserRoleEnum.Administrator }, false, null, userExisting.Id.ToString());
            Assert.NotNull(userServiceModel1);
            Assert.Equal(name, userServiceModel1.Name);

            var userServiceModel2 = service.Authorize(token, new List<UserRoleEnum> { UserRoleEnum.Administrator }, true, userExisting.Id.ToString(), userExisting.Id.ToString());
            Assert.NotNull(userServiceModel2);
            Assert.Equal(name, userServiceModel2.Name);
        }

        #endregion

        #region Negative

        [Fact]
        public async Task AuthorizeWithInvalidRoles()
        {
            var name = "name";
            var password = "password";
            var salt = RandomStringHelper.RandomString(256);
            var expiredAt = DateTime.UtcNow.AddDays(1);

            var userExisting = new UserDataModel
            {
                Name = name,
                Salt = salt,
                Password = UserService.HashPassword(salt, password)
            };

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1,
                Audience = UserSettings.DefaultAudience,
                Issuer = UserSettings.DefaultIssuer
            }), null);

            var token = service.GenerateJwtToken(userExisting, expiredAt);

            userExisting.Tokens = new List<UserTokenDataModel>
            {
                new UserTokenDataModel
                {
                    Token = token,
                    ExpiredAt = expiredAt,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.InsertOneAsync(userExisting);

            var ex = Assert.Throws<ErrorHelper.ErrorMessageException>(() => service.Authorize(token, new List<UserRoleEnum> { UserRoleEnum.Administrator }, false, null, userExisting.Id.ToString()));
            Assert.Equal(MusicKGMessages.UserUnauthorizedMessage, ex.Message);
            Assert.Equal(HttpStatusCode.Unauthorized, ex.Data["statusCode"]);
        }

        #endregion

        #endregion

        private async Task<UserDataModel> PrepareData(string userName, UserRoleEnum role, UserStatusEnum status, string createdBy)
        {
            var userExisting = this.CreateDataModel(userName, role, status, createdBy);
            await context.Users.InsertOneAsync(userExisting);
            return userExisting;
        }

        private UserDataModel CreateDataModel(string userName, UserRoleEnum role, UserStatusEnum status, string createdBy)
        {
            var salt = RandomStringHelper.RandomString(256);
            var userExisting = new UserDataModel
            {
                Name = userName,
                Salt = salt,
                Password = UserService.HashPassword(salt, "password"),
                Tokens = new List<UserTokenDataModel>(),
                Status = status,
                Roles = new List<UserRoleEnum>(new UserRoleEnum[] { role }),
                CreatedBy = createdBy
            };
            return userExisting;
        }
    }
}