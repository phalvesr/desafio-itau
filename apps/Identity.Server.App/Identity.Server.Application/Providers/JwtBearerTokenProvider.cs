using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.Server.Application.Interfaces.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Server.Application.Providers;

public class JwtBearerTokenProvider : IJwtBearerTokenProvider
{
    private readonly JwtSecurityTokenHandler tokenHandler;
    private readonly IRsaProvider rsaProvider;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IConfiguration configuration;

    public JwtBearerTokenProvider(
        IRsaProvider rsaProvider,
        IDateTimeProvider dateTimeProvider,
        IConfiguration configuration)
    {
        tokenHandler = new JwtSecurityTokenHandler();
        this.rsaProvider = rsaProvider;
        this.dateTimeProvider = dateTimeProvider;
        this.configuration = configuration;
    }

    public int DefaultTokenDurationInMinutes => 5;

    public string GenerateToken(IEnumerable<Claim> claims, DateTimeOffset expiresAt)
    {
        var rsa = rsaProvider.Rsa;

        rsa.ImportRSAPrivateKey(
            source: Convert.FromBase64String(configuration["Jwt:PrivateKey"]),
            bytesRead: out var _
        );

        var signinCredentials = new SigningCredentials(
            key: new RsaSecurityKey(rsa),
            algorithm: SecurityAlgorithms.RsaSha256
        );

        var jwt = new JwtSecurityToken(
            audience: configuration["Jwt:Audience"],
            issuer: configuration["Jwt:Issuer"],
            claims: claims,
            notBefore: dateTimeProvider.DateTimeNow,
            expires: expiresAt.DateTime,
            signingCredentials: signinCredentials
        );

        var token = tokenHandler.WriteToken(jwt);

        return token;
    }
}
