using Identity.Server.Application.Enums;
using Identity.Server.Application.Interfaces.Providers;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Application.Models.Responses;
using Identity.Server.Domain.Enums;
using Identity.Server.Domain.Gateways;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Server.Application.UseCases;

public class SignInAppAsyncUseCase : ISignInAppAsyncUseCase
{
    private readonly IAppAuthenticationRepository appRepository;
    private readonly IConfiguration configuration;
    private readonly IJwtBearerTokenProvider jwtBearerTokenProvider;
    private readonly IDateTimeProvider dateTimeProvider;

    public SignInAppAsyncUseCase(
        IAppAuthenticationRepository appRepository,
        IConfiguration configuration,
        IJwtBearerTokenProvider jwtBearerTokenProvider,
        IDateTimeProvider dateTimeProvider)
    {
        this.appRepository = appRepository;
        this.configuration = configuration;
        this.jwtBearerTokenProvider = jwtBearerTokenProvider;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<StatusNotification<SignInAppResponse>> ExecuteAsync(AppSignInRequest request)
    {
        var appEntity = await appRepository.GetAppsAuthEntityByClientKeyAsync(request.ClientKey.ToString());

        if (appEntity is null)
        {
            return StatusNotification<SignInAppResponse>.Create(null, ProcessingStatusEnum.CouldNotFindUser);
        }

        var sha256 = SHA256.HashData(Encoding.UTF8.GetBytes(appEntity.ClientSecret));
        var computedSecretKey = BitConverter.ToString(sha256).Replace("-", "").ToUpper();

        if (computedSecretKey != request.SecretKeySha256.ToUpper())
        {
            return StatusNotification<SignInAppResponse>.Create(null, ProcessingStatusEnum.WrongCredentials);
        }

        var claims = GetApplicationClaims();

        var unixExpirationTime = dateTimeProvider.DateTimeOffsetNow
            .AddMinutes(jwtBearerTokenProvider.DefaultTokenDurationInMinutes);

        var token = jwtBearerTokenProvider.GenerateToken(claims, unixExpirationTime);


        var result = new SignInAppResponse(token, unixExpirationTime);

        return StatusNotification<SignInAppResponse>.Create(result, ProcessingStatusEnum.SuccessfullyProcessed);
    }

    private IEnumerable<Claim> GetApplicationClaims()
    {
        return new Claim[]
        {
            new Claim(ClaimTypes.Role, ApplicationRolesEnum.Application.StringRepresentation())
        };
    }
}
