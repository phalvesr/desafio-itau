using FluentValidation;
using Identity.Server.Application.Enums;
using Identity.Server.Application.Interfaces.Providers;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Application.Models.Responses;
using Identity.Server.Application.Utils;
using Identity.Server.Domain.Entities;
using Identity.Server.Domain.Gateways;
using BCryptDotnet = BCrypt.Net.BCrypt;

namespace Identity.Server.Application.UseCases;

public class CreateUserAsyncUseCase : ICreateUserAsyncUseCase
{
    private readonly IUserCreationRepository userCreationRepository;
    private readonly IValidator<CreateUserRequest> requestValidator;
    private readonly IJwtBearerTokenProvider jwtBearerTokenProvider;
    private readonly IDateTimeProvider dateTimeProvider;

    public CreateUserAsyncUseCase(
        IUserCreationRepository userCreationRepository,
        IValidator<CreateUserRequest> requestValidator,
        IJwtBearerTokenProvider jwtBearerTokenProvider,
        IDateTimeProvider dateTimeProvider)
    {
        this.userCreationRepository = userCreationRepository;
        this.requestValidator = requestValidator;
        this.jwtBearerTokenProvider = jwtBearerTokenProvider;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<StatusNotification<CreateUserResponse>> ExecuteAsync(CreateUserRequest request)
    {
        try
        {
            var alreadyRegisteredUser = await userCreationRepository.GetUserByUsernameOrDefaultAsync(request.Username);

            if (alreadyRegisteredUser is not null)
            {
                return StatusNotification<CreateUserResponse>.Create(null, ProcessingStatusEnum.UserAlreadyRegistered);
            }

            var globalId = Guid.NewGuid();
            var encriptedPassword = BCryptDotnet.HashPassword(request.Password);

            var newUser = UsersAuthEntity.Create(globalId, request.Username, encriptedPassword);
            var newUserRoles = UsersRolesEntity.Create(globalId);

            var created = await userCreationRepository.CreateAsync(newUser, newUserRoles);

            if (!created)
            {
                return StatusNotification<CreateUserResponse>.Create(null, ProcessingStatusEnum.ServerHadProblemsProcessingRequest);
            }

            var claims = ClaimsHelper.GetClaims(newUser.Username, newUserRoles.Role, globalId.ToString());

            var unixExpiresAt = dateTimeProvider.DateTimeOffsetNow
                                    .AddMinutes(jwtBearerTokenProvider.DefaultTokenDurationInMinutes);

            var token = jwtBearerTokenProvider.GenerateToken(claims, unixExpiresAt);

            var response = new CreateUserResponse(token, unixExpiresAt, globalId);

            return StatusNotification<CreateUserResponse>.Create(response, ProcessingStatusEnum.SuccessfullyProcessed);
        }
        catch (Exception)
        {
            return StatusNotification<CreateUserResponse>.Create(null, ProcessingStatusEnum.ServerHadProblemsProcessingRequest);
        }
    }
}
