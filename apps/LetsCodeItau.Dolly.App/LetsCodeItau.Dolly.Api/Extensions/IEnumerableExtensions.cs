using System.Security.Claims;

namespace LetsCodeItau.Dolly.Api.Extensions;

public static class IEnumerableExtensions
{
    public static Claim GetClaimByTypeOrDefault(this IEnumerable<Claim> claims, string type)
    {
        return claims.FirstOrDefault(x => x.Type == type)!;
    }
}
