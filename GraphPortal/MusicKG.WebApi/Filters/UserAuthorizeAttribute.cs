using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Contract.ViewModels;

namespace MusicKG.WebApi.Filters
{
    public class UserAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private ILogger logger;

        public UserRoleEnum[] Roles { get; set; }

        public bool IgnoreRoleCheckIfUserSelf { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            logger = context.HttpContext.RequestServices.GetService<ILogger<UserAuthorizeAttribute>>();

            var userService = context.HttpContext.RequestServices.GetService<IUserService>();
            if (userService == null)
            {
                context.Result = new JsonResult(new ErrorViewModel
                {
                    Message = MusicKGMessages.SystemDownMessage
                });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return;
            }

            context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizations);
            var authorization = authorizations.FirstOrDefault();

            if (authorization == null)
            {
                ReturnUnauthorizedResult(context, MusicKGMessages.AuthHeaderMissingMessage);
                return;
            }

            var parts = authorization.Split(" ");
            if (parts.Length != 2)
            {
                ReturnUnauthorizedResult(context, MusicKGMessages.AuthHeaderWrongMessage);
                return;
            }

            var schema = parts[0];
            if (!schema.Equals("Bearer"))
            {
                ReturnUnauthorizedResult(context, MusicKGMessages.SchemaWrongMessage);
                return;
            }

            var token = parts[1];
            if (string.IsNullOrWhiteSpace(token))
            {
                ReturnUnauthorizedResult(context, MusicKGMessages.TokenWrongMessage);
                return;
            }

            JwtSecurityToken jwtToken = null;
            try
            {
                jwtToken = new JwtSecurityToken(token);
            }
            catch (Exception e)
            {
                logger.LogError(e, MusicKGMessages.ParseTokenFailedMessage);

                ReturnUnauthorizedResult(context, MusicKGMessages.TokenWrongMessage);
            }

            string userIdFromToken = jwtToken?.Claims?.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(userIdFromToken))
            {
                ReturnUnauthorizedResult(context, MusicKGMessages.TokenWrongMessage);
                return;
            }

            context.RouteData.Values.TryGetValue("userId", out object userIdFromRoute);

            UserServiceModel user = null;
            try
            {
                user = userService.Authorize(token, Roles, IgnoreRoleCheckIfUserSelf, userIdFromRoute?.ToString(), userIdFromToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, MusicKGMessages.AuthUserFailedMessage);

                ReturnUnauthorizedResult(context, MusicKGMessages.UnauthorizedMessage);
                return;
            }

            if (user == null)
            {
                ReturnUnauthorizedResult(context, MusicKGMessages.UnauthorizedMessage);
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id, ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String),
                new Claim(ClaimTypes.Role, string.Join(",", user.Roles), ClaimValueTypes.String)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context, string message)
        {
            context.Result = new UnauthorizedResult();
            context.HttpContext.Response.Headers.Add(HeaderNames.WWWAuthenticate, string.Format(MusicKGMessages.BearerErrorMessage, message));
        }
    }
}
