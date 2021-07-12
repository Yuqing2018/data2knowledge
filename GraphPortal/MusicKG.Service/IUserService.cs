using MongoDB.Bson;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IUserService
    {
        Task<Tuple<long, IEnumerable<UserServiceModel>>> GetUsersAsync(UserRoleEnum? role = null, UserStatusEnum? status = null, int from = 0, int? size = null);
        
        Task<UserServiceModel> GetUserAsync(string id);

        Task<UserServiceModel> CreateUserAsync(UserCreateServiceModel serviceModel);

        Task<UserServiceModel> UpdateUserAsync(string id, UserUpdateServiceModel serviceModel);

        Task DeleteUserAsync(string id);

        UserServiceModel Authorize(string token, IEnumerable<UserRoleEnum> roles, bool ignoreRoleCheckIfUserSelf, string userIdFromRoute, string userIdFromToken);

        Task<UserLoginServiceModel> LoginAsync(string name, string password);

        Task LogoutAsync(string id, string token);
    }
}
