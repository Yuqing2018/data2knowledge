using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Helpers
{
    public static class JwtSecurityTokenHelper
    {
        public static string GetId(this JwtSecurityToken principal) =>
            principal?.Claims?.FirstOrDefault(claim => ClaimTypes.NameIdentifier.Equals(claim.Type))?.Value;

        public static string GetUniqueName(this JwtSecurityToken principal) =>
            principal?.Claims?.FirstOrDefault(claim => "unique_name".Equals(claim.Type))?.Value;

        public static string[] GetRoles(this JwtSecurityToken principal) =>
            principal?.Claims?.Where(claim => ClaimTypes.Role.Equals(claim.Type)).Select(c => c.Value).ToArray() ?? new string[] { };
    }
}
