using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.HondaPlugins.DataAccess;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public class HondaUserOperator : IUserOperator
    {
        private readonly IHondaMongoDbContext context;
        private ILogger<HondaUserOperator> logger;

        public HondaUserOperator(IHondaMongoDbContext context, ILogger<HondaUserOperator> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task CreateAsync(string userName, string password)
        {
            var exists = await context.Users.AsQueryable().AnyAsync(u => u.Name == userName);

            if (exists)
            {
                logger.LogError("User already exist.");
                return;
            }

            var user = new UserDataModel
            {
                Name = userName,
                Roles = new List<UserRoleEnum> { UserRoleEnum.Annotator },
                Status = UserStatusEnum.Enabled,
                CreatedAt = DateTime.UtcNow,
                Tokens = new List<UserTokenDataModel>(),
                CreatedBy = null
            };

            user.Salt = Helpers.RandomString(256);

            user.Password = Helpers.Sha256Hash(user.Salt + password);

            await context.Users.InsertOneAsync(user);
        }

        public async Task DisableAsync(string userName)
        {
            var user = await GetUserAsync(userName);

            if (user == null)
                return;

            await context.Users.UpdateOneAsync(u => u.Name == userName,
                Builders<UserDataModel>.Update.Set(x => x.Status, UserStatusEnum.Disabled));
        }

        public async Task EnableAsync(string userName)
        {
            var user = await GetUserAsync(userName);

            if (user == null)
                return;

            await context.Users.UpdateOneAsync(u => u.Name == userName,
                Builders<UserDataModel>.Update.Set(x => x.Status, UserStatusEnum.Enabled));
        }

        public async Task<List<UserDataModel>> ListAsync(string userName)
        {
            var querable = context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(userName))
                querable.Where(u => u.Name == userName);

            return await querable.ToListAsync();
        }

        public async Task RenameAsync(string userName, string newName)
        {
            var user = await GetUserAsync(userName);

            if (user == null)
                return;

            await context.Users.UpdateOneAsync(u => u.Name == userName,
                Builders<UserDataModel>.Update.Set(x => x.Name, newName));
        }

        public async Task ResetPasswordAsync(string userName, string password)
        {
            var user = await GetUserAsync(userName);

            if (user == null)
                return;

            var salt = Helpers.RandomString(256);

            var hashedPassoword = Helpers.Sha256Hash(salt + password);

            await context.Users.UpdateOneAsync(u => u.Name == userName,
                Builders<UserDataModel>.Update.Set(x => x.Salt, salt).Set(x => x.Password, hashedPassoword));
        }

        private async Task<UserDataModel> GetUserAsync(string userName)
        {
            var user = await context.Users.AsQueryable().FirstOrDefaultAsync(u => u.Name == userName);

            if (user == null)
                logger.LogError("User does not exist.");

            return user;
        }
    }
}
