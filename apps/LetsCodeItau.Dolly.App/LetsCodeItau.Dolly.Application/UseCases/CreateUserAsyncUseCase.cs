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

public class CreateUserAsyncUseCase : ICreateUserAsyncUseCase
{
    private readonly IAuthenticationGateway authenticationGateway;
    private readonly IRepositoryBase<User> userRepository;
    private readonly IMapper mapper;
    private readonly IDateTimeProvider dateTimeProvider;

    public CreateUserAsyncUseCase(
        IAuthenticationGateway authenticationGateway,
        IRepositoryBase<User> userRepository,
        IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        this.authenticationGateway = authenticationGateway;
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<Notification<CreateUserResponse>> ExecuteAsync(CreateRequest request)
    {
        var apiRequest = mapper.Map<CreateUserRequest>(request);
        var result = await authenticationGateway.CreateUserAsync(apiRequest);

        if (!result.Succeeded)
        {
            return GetUnsuccessfulStatus(result);
        }
        var globalId = result.Response.GlobalId;

        var newUser = new User
        {
            GlobalId = globalId.ToString(),
            Points = 0,
            DisplayName = request.DisplayName,
            RegisterDate = dateTimeProvider.DateTimeUtcNow,
            Username = request.Username
        };

        await userRepository.AddAsync(newUser);

        return GetSuccessfulStatus(result);
    }

    private Notification<CreateUserResponse> GetUnsuccessfulStatus(Result<CreateUserResponseDto> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.Conflict => Notification<CreateUserResponse>.Create(null!, ProcessingStatusEnum.UserAlreadyRegistered, "user already registered."),
            HttpStatusCode.BadRequest => Notification<CreateUserResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, "wrong client credetials provided."),
            _ => Notification<CreateUserResponse>.Create(null!, ProcessingStatusEnum.ServerHadProblemsProcessingRequest, "Server had problems processing request.")
        };
    }

    private Notification<CreateUserResponse> GetSuccessfulStatus(Result<CreateUserResponseDto> result)
    {
        var response = result.Response;
        var createUserResponse = new CreateUserResponse(response.AccessToken, response.UnixTimeExpiresAt, ((int)result.StatusCode), string.Empty, result.Succeeded);

        return Notification<CreateUserResponse>.Create(createUserResponse, ProcessingStatusEnum.SuccessfullyProcessed);
    }
}
