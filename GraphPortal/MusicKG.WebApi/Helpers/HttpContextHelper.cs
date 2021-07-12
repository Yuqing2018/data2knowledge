using Microsoft.AspNetCore.Http;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Helpers
{
    public class HttpContextHelper
    {
        /// <summary>
        /// Get current login user from http context.
        /// </summary>
        /// <param name="httpContext">The current http context.</param>
        /// <returns>Current user Id and user name.</returns>
        public static Tuple<string, string, List<UserRoleEnum>> GetCurrentUser(HttpContext httpContext)
        {
            return new Tuple<string, string, List<UserRoleEnum>>(
                httpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value,
                httpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value,
                GetUserRoles(httpContext?.User?.Claims?.Where(x=>x.Type == ClaimTypes.Role).FirstOrDefault()?.Value));
        }

        private static List<UserRoleEnum> GetUserRoles(string rolesString)
        {
            if (string.IsNullOrEmpty(rolesString))
            {
                return new List<UserRoleEnum>();
            }
            else
            {
                return rolesString.Split(',').Select(x => (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), x)).ToList();
            }
        }
    }
}
