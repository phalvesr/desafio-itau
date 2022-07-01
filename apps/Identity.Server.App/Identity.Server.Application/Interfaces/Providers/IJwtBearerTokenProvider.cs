using System.Security.Claims;

namespace Identity.Server.Application.Interfaces.Providers;

public interface IJwtBearerTokenProvider
{
    int DefaultTokenDurationInMinutes { get; }
    string GenerateToken(IEnumerable<Claim> claims, DateTimeOffset expiresAt);
}
