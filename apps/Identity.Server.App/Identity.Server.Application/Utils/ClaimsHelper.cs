using System.Security.Claims;

namespace Identity.Server.Application.Utils;

internal static class ClaimsHelper
{
    internal static IEnumerable<Claim> GetClaims(string username, string role, string globalId)
    {
        return new Claim[]
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim("GlobalId", globalId.ToString())
        };
    }
}
