using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MusicKG.Service.Models;
using MongoDB.Bson;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Settings;
using MusicKG.Service.Helpers;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Resources;
using MusicKG.Service.Constants;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// User service.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMusicKGContext context;
        private readonly IOptions<UserSettings> options;
        private readonly ILogger<UserService> logger;

        /// <summary>
        /// User service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        /// <param name="options">User settings.</param>
        public UserService(
            IMusicKGContext context,
            IOptions<UserSettings> options,
            ILogger<UserService> logger)
        {
            this.context = context;
            this.options = options;
            this.logger = logger;
        }

        /// <summary>
        /// Get user list with filters.
        /// </summary>
        /// <param name="role">User role filter.</param>
        /// <param name="status">User status filter.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>User service object.</returns>
        public async Task<Tuple<long, IEnumerable<UserServiceModel>>> GetUsersAsync(
            UserRoleEnum? role = null,
            UserStatusEnum? status = null,
            int from = 0,
            int? size = null)
        {
            var builder = Builders<UserDataModel>.Filter;
            var filter = builder.Empty;

            if (status != null)
            {
                filter &= builder.Eq(x => x.Status, status);
            }

            if (role != null)
            {
                filter &= builder.Where(x => x.Roles.Contains((UserRoleEnum)role));
            }

            return await this.GetUsersAsync(filter, from, size);
        }

        /// <summary>
        /// Get user by ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>User service object.</returns>
        public async Task<UserServiceModel> GetUserAsync(string id)
        {
            var user = await context.Users.Find(u => u.Id == new ObjectId(id)).FirstOrDefaultAsync();
            if (user == null)
                ErrorHelper.ThrowException(MusicKGMessages.UserNotExistMessage, HttpStatusCode.BadRequest);

            return UserDataModelToServiceModel(user);
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="serviceModel">User create service object.</param>
        /// <returns>User service object.</returns>
        public async Task<UserServiceModel> CreateUserAsync(UserCreateServiceModel serviceModel)
        {
            var user = new UserDataModel
            {
                Name = serviceModel.Name,
                Roles = serviceModel.Roles,
                Status = UserStatusEnum.Enabled,
                CreatedAt = DateTime.UtcNow,
                Tokens = new List<UserTokenDataModel>(),
                CreatedBy = serviceModel.CreatedBy
            };
            user.Salt = RandomStringHelper.RandomString(256);
            user.Password = HashPassword(user.Salt, serviceModel.Password);

            try
            {
                await context.Users.InsertOneAsync(user);
            }
            catch(MongoWriteException e)
            {
                var message = MusicKGMessages.UserCreateFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = MusicKGMessages.UserNameExistMessage;

                logger.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return UserDataModelToServiceModel(user);
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="serviceModel">User update service object.</param>
        /// <returns>User service object.</returns>
        public async Task<UserServiceModel> UpdateUserAsync(string id, UserUpdateServiceModel serviceModel)
        {
            var user = await context.Users.Find(u => u.Id == new ObjectId(id)).FirstOrDefaultAsync();
            if (user == null)
                ErrorHelper.ThrowException(MusicKGMessages.UserNotExistMessage, HttpStatusCode.BadRequest);

            var update = Builders<UserDataModel>.Update.Set(u => u.Id, user.Id);

            if (serviceModel.IsNameAssigned)
                update = update.Set(u => u.Name, serviceModel.Name);

            if (serviceModel.IsRolesAssigned)
                if (serviceModel.Roles.Count == 0)
                    update = update.Unset(u => u.Roles);
                else
                    update = update.Set(u => u.Roles, serviceModel.Roles);

            if (serviceModel.IsPasswordAssigned)
            {
                var salt = RandomStringHelper.RandomString(256);
                update = update
                    .Set(u => u.Salt, salt)
                    .Set(u => u.Password, HashPassword(salt, serviceModel.Password))
                    .Set(u => u.Tokens, new List<UserTokenDataModel>());
            }

            if (serviceModel.Status != null)
                update = update.Set(u => u.Status, serviceModel.Status);

            user = await context.Users.FindOneAndUpdateAsync<UserDataModel>(u => u.Id == user.Id, update, new FindOneAndUpdateOptions<UserDataModel>
            {
                ReturnDocument = ReturnDocument.After
            });

            return UserDataModelToServiceModel(user);
        }

        /// <summary>
        /// Delete user by user ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        public async Task DeleteUserAsync(string id)
        {
            var result = await context.Users.DeleteOneAsync(u => u.Id == new ObjectId(id));

            if (result.DeletedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.UserNotExistMessage, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="password">User password.</param>
        /// <returns>User login service object.</returns>
        public async Task<UserLoginServiceModel> LoginAsync(string name, string password)
        {
            var user = await context.Users.Find(u => u.Name == name).FirstOrDefaultAsync();
            if (user == null)
                ErrorHelper.ThrowException(MusicKGMessages.UserNamePasswordWrongMessage, HttpStatusCode.Unauthorized);

            if (user.Status == UserStatusEnum.Disabled)
                ErrorHelper.ThrowException(MusicKGMessages.UserDisabledMessage, HttpStatusCode.Unauthorized);

            var hashedPassword = HashPassword(user.Salt, password);

            if (!hashedPassword.Equals(user.Password))
                ErrorHelper.ThrowException(MusicKGMessages.UserNamePasswordWrongMessage, HttpStatusCode.Unauthorized);

            if (user.Tokens != null)
            {
                if (user.Tokens.Where(t => t.ExpiredAt > DateTime.UtcNow).Count() >= options.Value.MaxExistingValidLoginsPerUser)
                    ErrorHelper.ThrowException(MusicKGMessages.UserLoginCountOverflowMessage, HttpStatusCode.Unauthorized);

                if (user.Tokens.Count > 0)
                {
                    var removeExpiredToken = Builders<UserDataModel>.Update.PullFilter(u => u.Tokens,
                    Builders<UserTokenDataModel>.Filter.Lte(t => t.ExpiredAt, DateTime.UtcNow));

                    await context.Users.FindOneAndUpdateAsync<UserDataModel>(u => u.Id == user.Id, removeExpiredToken);
                }
            }

            var expiredAt = DateTime.UtcNow.AddDays(options.Value.TokenExpiredInDays);
            var token = GenerateJwtToken(user, expiredAt);

            var update = Builders<UserDataModel>.Update
                .Push(u => u.Tokens, new UserTokenDataModel
                {
                    Token = token,
                    ExpiredAt = expiredAt,
                    CreatedAt = DateTime.UtcNow
                });

            user = await context.Users.FindOneAndUpdateAsync<UserDataModel>(u => u.Id == user.Id, update, new FindOneAndUpdateOptions<UserDataModel>
            {
                ReturnDocument = ReturnDocument.After
            });

            return new UserLoginServiceModel
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Roles = user.Roles,
                Status = user.Status,
                CreatedAt = user.CreatedAt,
                Token = token,
                TokenExpiredAt = expiredAt
            };
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="token">Token.</param>
        public async Task LogoutAsync(string id, string token)
        {
            var user = await context.Users.Find(u => u.Id == new ObjectId(id)).FirstOrDefaultAsync();
            if (user == null)
                ErrorHelper.ThrowException(MusicKGMessages.UserNotExistMessage, HttpStatusCode.BadRequest);

            var update = Builders<UserDataModel>.Update.PullFilter(u => u.Tokens,
                Builders<UserTokenDataModel>.Filter.Eq(t => t.Token, token));

            await context.Users.FindOneAndUpdateAsync(u => u.Id == user.Id, update);
        }

        /// <summary>
        /// Authorize user login.
        /// </summary>
        /// <param name="token">Access token.</param>
        /// <param name="roles">Required user role list.</param>
        /// <param name="ignoreRoleCheckIfUserSelf">If ignore user role check when user operate self owned resources.</param>
        /// <param name="userIdFromRoute">User ID from route.</param>
        /// <param name="userIdFromToken">User ID from token.</param>
        /// <returns>User service object.</returns>
        public UserServiceModel Authorize(
            string token,
            IEnumerable<UserRoleEnum> roles,
            bool ignoreRoleCheckIfUserSelf,
            string userIdFromRoute,
            string userIdFromToken)
        {
            var tokenBuilder = Builders<UserTokenDataModel>.Filter;
            var tokenFilter = tokenBuilder.Eq(t => t.Token, token)
                & tokenBuilder.Exists(t => t.ExpiredAt)
                & tokenBuilder.Gt(t => t.ExpiredAt, DateTime.UtcNow);

            var builder = Builders<UserDataModel>.Filter;
            var filter = builder.Eq(u => u.Id, new ObjectId(userIdFromToken))
                & builder.Eq(u => u.Status, UserStatusEnum.Enabled)
                & builder.Exists(u => u.Tokens)
                & builder.ElemMatch(u => u.Tokens, tokenFilter);

            if (roles != null && roles.Count() > 0)
            {
                var roleFilter = !builder.Empty;
                roleFilter |= builder.AnyIn(u => u.Roles, roles);

                if (ignoreRoleCheckIfUserSelf && !string.IsNullOrEmpty(userIdFromRoute))
                    roleFilter |= builder.Eq(u => u.Id, new ObjectId(userIdFromRoute));

                filter &= roleFilter;
            }

            var user = context.Users.Find(filter).FirstOrDefault();
            if (user == null)
                ErrorHelper.ThrowException(MusicKGMessages.UserUnauthorizedMessage, HttpStatusCode.Unauthorized);

            return UserDataModelToServiceModel(user);
        }

        /// <summary>
        /// Hash password.
        /// </summary>
        /// <param name="salt">Salt.</param>
        /// <param name="password">Password.</param>
        /// <returns>Hashed password.</returns>
        public static string HashPassword(string salt, string password)
        {
            return Sha256Hash(salt + password);
        }

        /// <summary>
        /// Sha256 hash.
        /// </summary>
        /// <param name="text">Text to hash.</param>
        /// <returns>Hash result.</returns>
        public static string Sha256Hash(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty);
            }
        }

        private UserServiceModel UserDataModelToServiceModel(UserDataModel dataModel)
        {
            return new UserServiceModel
            {
                Id = dataModel.Id.ToString(),
                Name = dataModel.Name,
                Status = dataModel.Status,
                Roles = dataModel.Roles,
                CreatedAt = dataModel.CreatedAt,
                CreatedBy = dataModel.CreatedBy
            };
        }

        /// <summary>
        /// Generate JWT token.
        /// </summary>
        /// <param name="user">User data object.</param>
        /// <param name="expires">Expiration date.</param>
        /// <returns></returns>
        public string GenerateJwtToken(UserDataModel user, DateTime expires)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.System, options.Value.Audience)
            };
            claims.AddRange(user.Roles?.Select(role => new Claim(ClaimTypes.Role, role.ToString())) ?? new List<Claim>());

            var creds = new SigningCredentials(SecurityKeyHelper.RsaPrivateKey, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                options.Value.Issuer,
                options.Value.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Tuple<long, IEnumerable<UserServiceModel>>> GetUsersAsync(FilterDefinition<UserDataModel> filter, int from, int? size)
        {
            var usersFind = context.Users.Find(filter);

            var totalCount = await usersFind.CountDocumentsAsync();

            usersFind.Sort(Builders<UserDataModel>.Sort.Descending(x => x.CreatedAt));

            if (from > 0)
                usersFind = usersFind.Skip(from);

            if (size.HasValue)
                usersFind = usersFind.Limit(size.Value);

            var users = await usersFind.ToListAsync();

            var userServiceModels = users.Take(size ?? int.MaxValue).Select(u => UserDataModelToServiceModel(u));

            return new Tuple<long, IEnumerable<UserServiceModel>>(totalCount, userServiceModels);
        }
    }
}
