using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Helpers;
using System.Security.Claims;
using MusicKG.WebApi.Filters;
using Microsoft.Net.Http.Headers;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.Service.Constants;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.Attributes;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// User controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        /// <summary>
        /// User controller constructor.
        /// </summary>
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Get user list.
        /// </summary>
        /// <param name="role">User role.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>User object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<UserViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator })]
        public async Task<PaginationViewModel<UserViewModel>> GetUsers(
            [FromQuery] UserRoleEnum? role,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, users) = await userService.GetUsersAsync(role, from: from, size: size);

            return new PaginationViewModel<UserViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = users.Count(),
                Items = users.Select(u => UserServiceModelToViewModel<UserServiceModel, UserViewModel>(u))
            };
        }

        /// <summary>
        /// Get user by user ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>User object.</returns>
        [HttpGet]
        [Route("{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task<UserViewModel> GetUser(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string userId)
        {
            var user = await userService.GetUserAsync(userId);

            return UserServiceModelToViewModel<UserServiceModel, UserViewModel>(user);
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="bindingModel">User create binding object.</param>
        /// <returns>User object.</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator })]
        public async Task<UserViewModel> CreateUser(
            [FromBody] [RequiredLocalized] UserCreateBindingModel bindingModel)
        {
            var currentUser = HttpContextHelper.GetCurrentUser(this.HttpContext);

            var user = await userService.CreateUserAsync(new UserCreateServiceModel
            {
                Name = bindingModel.Name,
                Password = bindingModel.Password,
                Roles = bindingModel.Roles,
                CreatedBy = currentUser.Item2
            });

            return UserServiceModelToViewModel<UserServiceModel, UserViewModel>(user);
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="bindingModel">User update binding object.</param>
        /// <returns>User object.</returns>
        [HttpPut]
        [Route("{userId}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(IgnoreRoleCheckIfUserSelf = true, Roles = new UserRoleEnum[] { UserRoleEnum.Administrator })]
        public async Task<UserViewModel> UpdateUser(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string userId,
            [FromBody] [RequiredLocalized] UserUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrEmpty(bindingModel.Name))
                ErrorHelper.ThrowException(MusicKGMessages.UserNameEmptyMessage, HttpStatusCode.BadRequest);

            if (bindingModel.IsPasswordAssigned && string.IsNullOrEmpty(bindingModel.Password))
                ErrorHelper.ThrowException(MusicKGMessages.UserPasswordEmptyMessage, HttpStatusCode.BadRequest);

            var user = await userService.UpdateUserAsync(userId, new UserUpdateServiceModel
            {
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                Password = bindingModel.Password,
                IsPasswordAssigned = bindingModel.IsPasswordAssigned,
                Roles = bindingModel.Roles,
                IsRolesAssigned = bindingModel.IsRolesAssigned,
                Status = bindingModel.Status
            });

            return UserServiceModelToViewModel<UserServiceModel, UserViewModel>(user);
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator })]
        public async Task DeleteUser([FromRoute] [RequiredLocalized] string userId)
        {
            await userService.DeleteUserAsync(userId);
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="bindingModel">User login binding object.</param>
        [HttpPost]
        [Route("Login")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(UserLoginViewModel), (int)HttpStatusCode.OK)]
        public async Task<UserLoginViewModel> Login(
            [FromBody] [RequiredLocalized] UserLoginBindingModel bindingModel)
        {
            var user = await userService.LoginAsync(bindingModel.Name, bindingModel.Password);

            var loginViewModel = UserServiceModelToViewModel<UserLoginServiceModel, UserLoginViewModel>(user);
            loginViewModel.Token = user.Token;
            loginViewModel.TokenExpiredAt = user.TokenExpiredAt;

            return loginViewModel;
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        [HttpPost]
        [Route("Logout")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task Logout()
        {
            var userId = User?.Claims?.Where(c => c.Type.Equals(ClaimTypes.NameIdentifier)).FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                ErrorHelper.ThrowException(MusicKGMessages.UserIdWrongMessage, HttpStatusCode.BadRequest);

            var token = Request.Headers[HeaderNames.Authorization].FirstOrDefault().Split(" ")[1];

            await userService.LogoutAsync(userId, token);
        }

        private O UserServiceModelToViewModel<I, O>(I serviceModel)
            where I : UserServiceModel
            where O : UserViewModel, new()
        {
            return new O
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                Roles = serviceModel.Roles,
                Status = serviceModel.Status,
                CreatedAt = serviceModel.CreatedAt,
                CreatedBy = serviceModel.CreatedBy
            };
        }
    }
}
