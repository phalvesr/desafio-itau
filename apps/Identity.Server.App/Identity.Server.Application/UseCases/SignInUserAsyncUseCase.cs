using System.Security.Claims;
using Identity.Server.Application.Enums;
using Identity.Server.Application.Interfaces.Providers;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Application.Models.Responses;
using Identity.Server.Application.Providers;
using Identity.Server.Application.Utils;
using Identity.Server.Domain.Entities;
using Identity.Server.Domain.Gateways;
using BCryptDotnet = BCrypt.Net.BCrypt;

namespace Identity.Server.Application.UseCases;

public sealed class SignInUserAsyncUseCase : ISignInUserUseCaseAsync
{
    private readonly IUserAuthenticationRepository userAuthRepository;
    private readonly ICacheRepository<CachedSignInAttemptModel> cacheRepository;
    private readonly IJwtBearerTokenProvider jwtBearerTokenProvider;
    private readonly IDateTimeProvider dateTimeProvider;

    public SignInUserAsyncUseCase(
        IUserAuthenticationRepository userAuthRepository,
        ICacheRepository<CachedSignInAttemptModel> cacheRepository,
        IJwtBearerTokenProvider jwtBearerTokenProvider,
        IDateTimeProvider dateTimeProvider)
    {
        this.userAuthRepository = userAuthRepository;
        this.cacheRepository = cacheRepository;
        this.jwtBearerTokenProvider = jwtBearerTokenProvider;
        this.dateTimeProvider = dateTimeProvider;
    }
    public async Task<StatusNotification<UserSignInResponse>> ExecuteAsync(UserSignInRequest request)
    {
        try
        {
            var user = await userAuthRepository.GetUserByUsernameOrDefaultAsync(request.Username);

            if (user is null)
            {
                return StatusNotification<UserSignInResponse>.Create(null, ProcessingStatusEnum.CouldNotFindUser);
            }

            if (!IsUserCredentialsRight(request, user))
            {
                var attempts = await UpdateCachedSignInAttempts(request.Username);

                return StatusNotification<UserSignInResponse>.Create(null, ProcessingStatusEnum.WrongCredentials);
            }

            var roleEntity = await userAuthRepository.GetUserRoleByUserAuthIdAsync(user.UserAuthId);

            var claims = ClaimsHelper.GetClaims(request.Username, roleEntity.Role, roleEntity.GlobalId);

            var unixExpiresAt = dateTimeProvider.DateTimeOffsetNow
                                    .AddMinutes(jwtBearerTokenProvider.DefaultTokenDurationInMinutes);

            var accessToken = jwtBearerTokenProvider.GenerateToken(
                claims, unixExpiresAt);

            var response = new UserSignInResponse(accessToken, unixExpiresAt, roleEntity.GlobalId);

            return StatusNotification<UserSignInResponse>.Create(response, ProcessingStatusEnum.SuccessfullyProcessed);
        }
        catch (Exception)
        {
            return StatusNotification<UserSignInResponse>.Create(null, ProcessingStatusEnum.ServerHadProblemsProcessingRequest);
        }
    }

    private bool IsUserCredentialsRight(UserSignInRequest request, UsersAuthEntity user)
    {
        return request.Username == user.Username && BCryptDotnet.Verify(request.Password, user.EncryptedPassword);
    }

    private async Task<int> UpdateCachedSignInAttempts(string username)
    {
        var expiry = TimeSpan.FromMinutes(60);
        var cachedContent = await cacheRepository.GetEntryOrDefaultAsync(username);

        if (cachedContent is null)
        {
            var newEntry = CachedSignInAttemptModel.Create(dateTimeProvider.DateTimeOffsetNow);

            await cacheRepository.SetEntryAsync(username, newEntry, expiry);

            return newEntry.TotalAttempts;
        }

        cachedContent.AttemptedAt.Add(dateTimeProvider.DateTimeOffsetNow);
        cachedContent.TotalAttempts++;

        await cacheRepository.SetEntryAsync(username, cachedContent, expiry);

        return cachedContent.TotalAttempts;
    }

    private IEnumerable<Claim> GetClaims(UserSignInRequest request, UsersRolesEntity roleEntity)
    {
        return new Claim[]
        {
            new Claim(ClaimTypes.Role, roleEntity.Role),
            new Claim(ClaimTypes.NameIdentifier, request.Username),
            new Claim("GlobalId", roleEntity.GlobalId.ToString())
        };
    }
}
