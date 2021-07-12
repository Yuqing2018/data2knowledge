using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using MusicKG.Service;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using MusicKG.DataAccess;
using MusicKG.Service.Test.Helpers;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Test.Models;
using MusicKG.Service.Helpers;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Linq;

namespace MusicKG.Service.Test.Fixtures
{
    public class UserFixture : MongoFixture
    {
        public readonly IEnumerable<UserDataModel> Data;

        public UserFixture()
        {
            Data = PrepareUserData().Result;
        }

        private async Task<IEnumerable<UserDataModel>> PrepareUserData()
        {
            var users = Enum.GetValues(typeof(UserRoleEnum)).Cast<UserRoleEnum>().SelectMany((r, i) => Enum.GetValues(typeof(UserStatusEnum)).Cast<UserStatusEnum>().Select((s, j) =>
            {
                var salt = RandomStringHelper.RandomString(256);
                return new UserDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = $"UserName{i}-{j}",
                    Salt = salt,
                    Password = UserService.HashPassword(salt, "password"),
                    Tokens = new List<UserTokenDataModel>(),
                    Status = s,
                    Roles = new List<UserRoleEnum> { r },
                    CreatedBy = ObjectId.GenerateNewId().ToString()
                };
            })).ToList();

            await Context.Users.InsertManyAsync(users);

            return users;
        }
    }
}
