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
using MusicKG.Service.Resources;

namespace MusicKG.Service.Test
{
    public class UserServiceGetTest : IClassFixture<UserFixture>
    {
        private readonly UserFixture userFixture;
        private readonly IMusicKGContext context;

        public UserServiceGetTest(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            context = userFixture.Context;
        }

        [Theory]
        [MemberData(nameof(MemberDataForGetUsers))]
        public async Task GetUsers(UserRoleEnum? role, UserStatusEnum? status, int from, int? size)
        {
            var rawData = userFixture.Data;
            var usersTotalExpected = rawData.Where(u => (!role.HasValue || u.Roles.Contains(role.Value)) && (!status.HasValue || u.Status == status));
            var usersExpectedTotal = usersTotalExpected;

            var usersExpected = usersExpectedTotal.OrderByDescending(x => x.CreatedAt).Skip(from).Take(size ?? int.MaxValue);

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1
            }), null);

            var (totalCount, users) = await service.GetUsersAsync(role, status, from, size);

            Assert.NotNull(users);
            Assert.Equal(totalCount, usersTotalExpected.Count());

            if (size.HasValue && size < usersExpected.Count())
                Assert.Equal(size.Value, users.Count());
            else
                Assert.Equal(usersExpected.Count(), users.Count());

            Assert.All(users, u =>
            {
                var userExpected = usersExpected.Where(ue => ue.Id.ToString().Equals(u.Id)).FirstOrDefault();
                Assert.NotNull(userExpected);
                AssertUserServiceModel(userExpected, u);
            });
        }

        [Fact]
        public async Task GetUser()
        {
            var rawData = userFixture.Data;
            var user = rawData.First();

            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1
            }), null);

            var userServiceModel = await service.GetUserAsync(user.Id.ToString());

            this.AssertUserServiceModel(user, userServiceModel);
        }

        [Fact]
        public async Task GetUserWithInvalidId()
        {
            var service = new UserService(context, Options.Create(new UserSettings
            {
                TokenExpiredInDays = 1
            }), null);

            var ex = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => service.GetUserAsync(ObjectId.GenerateNewId().ToString()));
            Assert.Equal(MusicKGMessages.UserNotExistMessage, ex.Message);
        }

        private void AssertUserServiceModel(UserDataModel expected, UserServiceModel actual)
        {
            Assert.NotNull(actual);
            Assert.Equal(expected.Id.ToString(), actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Roles, actual.Roles);
            Assert.Equal(expected.Status, actual.Status);
            Assert.Equal(expected.CreatedBy, actual.CreatedBy);
        }

        public static IEnumerable<object[]> MemberDataForGetUsers()
        {
            return Enum.GetValues(typeof(UserRoleEnum)).Cast<UserRoleEnum?>().ToList().Append(null).SelectMany(
                r => Enum.GetValues(typeof(UserStatusEnum)).Cast<UserStatusEnum?>().ToList().Append(null).SelectMany(
                    s => Enumerable.Range(0, 2).SelectMany(
                        from => Enumerable.Range(-1, 2).Select(
                            size => new object[]
                            {
                                r,
                                s,
                                from,
                                size < 0 ? null : (int?) size
                            }))));
        }
    }
}