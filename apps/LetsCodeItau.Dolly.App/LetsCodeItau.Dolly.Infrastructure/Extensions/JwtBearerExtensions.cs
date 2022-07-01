using LetsCodeItau.Dolly.Application.Interfaces.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LetsCodeItau.Dolly.Infrastructure.Extensions;

public static class JwtBearerExtensions
{
    public static IServiceCollection AddJwtBearerTokenAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var scope = services.BuildServiceProvider().CreateScope();
            var rsaProvider = scope.ServiceProvider.GetRequiredService<IRsaProvider>();
            var rsa = rsaProvider.Rsa;

            rsa.ImportRSAPublicKey(
                Convert.FromBase64String(configuration["Jwt:PublicKey"]),
                out var _);

            options.IncludeErrorDetails = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidAudience = configuration["Jwt:Audience"],
                ValidIssuer = configuration["Jwt:Issuer"],
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
