using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.Workflow.Models.ViewModels;

namespace MusicKG.Workflow.Filters
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
                    Message = "System is unavailable"
                });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return;
            }

            context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizations);
            var authorization = authorizations.FirstOrDefault();

            if (authorization == null)
            {
                ReturnUnauthorizedResult(context, "Authorization header is missing.");
                return;
            }

            var parts = authorization.Split(" ");
            if (parts.Length != 2)
            {
                ReturnUnauthorizedResult(context, "Invalid authorization header.");
                return;
            }

            var schema = parts[0];
            if (!schema.Equals("Bearer"))
            {
                ReturnUnauthorizedResult(context, "Invalid scheme.");
                return;
            }

            var token = parts[1];
            if (string.IsNullOrWhiteSpace(token))
            {
                ReturnUnauthorizedResult(context, "Invalid token.");
                return;
            }

            JwtSecurityToken jwtToken = null;
            try
            {
                jwtToken = new JwtSecurityToken(token);
            }
            catch(Exception e)
            {
                logger.LogError(e, "Failed to parse token.");

                ReturnUnauthorizedResult(context, "Invalid token.");
            }

            string userIdFromToken = jwtToken?.Claims?.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if(string.IsNullOrWhiteSpace(userIdFromToken))
            {
                ReturnUnauthorizedResult(context, "Invalid token.");
                return;
            }

            context.RouteData.Values.TryGetValue("userId", out object userIdFromRoute);

            UserServiceModel user = null;
            try
            {
                user = userService.Authorize(token, Roles, IgnoreRoleCheckIfUserSelf, userIdFromRoute?.ToString(), userIdFromToken);
            }
            catch(Exception e)
            {
                logger.LogError(e, "Failed to authorize user.");

                ReturnUnauthorizedResult(context, "Unauthorized operation.");
                return;
            }

            if (user == null)
            {
                ReturnUnauthorizedResult(context, "Unauthorized operation.");
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id, ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context, string message)
        {
            context.Result = new UnauthorizedResult();
            context.HttpContext.Response.Headers.Add(HeaderNames.WWWAuthenticate, $"Bearer error=\"{message}\"");
        }
    }
}
