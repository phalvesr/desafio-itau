using System.Net;
using AutoMapper;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Providers;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Users;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Application.Requests;
using LetsCodeItau.Dolly.Application.Responses.IdentityServer;
using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class LogUserInAsyncUseCase : ILogUserInAsyncUseCase
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IRepositoryBase<User> userBaseRepository;
    private readonly IAuthenticationGateway authenticationGateway;

    public LogUserInAsyncUseCase(
        IMapper mapper, IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider, IRepositoryBase<User> userBaseRepository,
        IAuthenticationGateway authenticationGateway)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.userBaseRepository = userBaseRepository;
        this.authenticationGateway = authenticationGateway;
    }

    public async Task<Notification<LogUserInResponse>> ExecuteAsync(LoginRequest request)
    {
        var apiRequest = mapper.Map<UserSignInRequest>(request);
        var result = await authenticationGateway.AuthenticateUserAsync(apiRequest);

        if (!result.Succeeded)
        {
            return GetUnsuccessfulStatus(result);
        }

        var response = result.Response;
        await userRepository.UpdateLastLogin(response.GlobalId.ToString(), dateTimeProvider.DateTimeUtcNow);

        return GetSuccessfulStatus(result);
    }

    private Notification<LogUserInResponse> GetSuccessfulStatus(Result<AuthenticateUserResponseDto> result)
    {
        var response = result.Response;
        var createUserResponse = new LogUserInResponse(response.AccessToken, response.UnixTimeExpiresAt, ((int)result.StatusCode), string.Empty, result.Succeeded);

        return Notification<LogUserInResponse>.Create(createUserResponse, ProcessingStatusEnum.SuccessfullyProcessed);
    }

    private Notification<LogUserInResponse> GetUnsuccessfulStatus(Result<AuthenticateUserResponseDto> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.NotFound => Notification<LogUserInResponse>.Create(null!, ProcessingStatusEnum.CouldNotFindUser, "User not registered."),
            HttpStatusCode.BadRequest => Notification<LogUserInResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, "Wrong client credetials provided."),
            _ => Notification<LogUserInResponse>.Create(null!, ProcessingStatusEnum.ServerHadProblemsProcessingRequest, "Server had problems processing request.")
        };
    }
}
