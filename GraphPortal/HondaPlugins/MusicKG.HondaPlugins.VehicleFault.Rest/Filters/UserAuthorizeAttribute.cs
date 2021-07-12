using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using MusicKG.HondaPlugins.Services.Resources;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Filters
{
    public class UserAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private ILogger logger;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizations);
            var authorization = authorizations.FirstOrDefault();

            if (authorization == null)
            {
                ReturnUnauthorizedResult(context, MusicKGHondaPluginsMessage.AuthHeaderMissingMessage);
                return;
            }

            var parts = authorization.Split(" ");
            if (parts.Length != 2)
            {
                ReturnUnauthorizedResult(context, MusicKGHondaPluginsMessage.AuthHeaderWrongMessage);
                return;
            }

            var schema = parts[0];
            if (!schema.Equals("Bearer"))
            {
                ReturnUnauthorizedResult(context, MusicKGHondaPluginsMessage.SchemaWrongMessage);
                return;
            }

            var token = parts[1];
            if (string.IsNullOrWhiteSpace(token))
            {
                ReturnUnauthorizedResult(context, MusicKGHondaPluginsMessage.TokenWrongMessage);
                return;
            }

            JwtSecurityToken jwtToken = null;
            try
            {
                jwtToken = new JwtSecurityToken(token);
            }
            catch (Exception e)
            {
                logger.LogError(e, MusicKGHondaPluginsMessage.ParseTokenFailedMessage);

                ReturnUnauthorizedResult(context, MusicKGHondaPluginsMessage.TokenWrongMessage);
            }

            var userId = jwtToken.GetId();
            var userName = jwtToken.GetUniqueName();
            var userRoles = jwtToken.GetRoles();
            if (string.IsNullOrWhiteSpace(userId))
            {
                ReturnUnauthorizedResult(context, MusicKGHondaPluginsMessage.TokenWrongMessage);
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String),
                new Claim(ClaimTypes.Role, string.Join(",", userRoles), ClaimValueTypes.String)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context, string message)
        {
            context.Result = new UnauthorizedResult();
            context.HttpContext.Response.Headers.Add(HeaderNames.WWWAuthenticate, string.Format(MusicKGHondaPluginsMessage.BearerErrorMessage, message));
        }
    }
}
