using System.Net;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Providers;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Application.Requests;
using LetsCodeItau.Dolly.Application.Responses.IdentityServer;
using LetsCodeItau.Dolly.Domain.Enums;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class TurnUserModeratorAsyncUseCase : ITurnUserModeratorAsyncUseCase
{
    private readonly IAuthenticationGateway authencicationGateway;
    private readonly IDateTimeProvider dateTimeProvider;

    public TurnUserModeratorAsyncUseCase(
        IAuthenticationGateway authencicationGateway,
        IDateTimeProvider dateTimeProvider)
    {
        this.authencicationGateway = authencicationGateway;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<Notification<TurnUserModeratorResponse>> ExecuteAsync(Guid globalUserId)
    {
        var apiRequest = new ChangeUserRoleRequest(globalUserId, UserRolesEnum.Moderator.StringRepresentation());
        var result = await authencicationGateway.ChangeUserRoleAsync(apiRequest);

        if (!result.Succeeded)
        {
            return GetUnsuccessfulStatus(result);
        }

        var response = new TurnUserModeratorResponse(UserRolesEnum.Moderator.StringRepresentation(), dateTimeProvider.DateTimeUtcNow, globalUserId);

        return Notification<TurnUserModeratorResponse>.Create(response, ProcessingStatusEnum.SuccessfullyProcessed);
    }

    private Notification<TurnUserModeratorResponse> GetUnsuccessfulStatus(Result<ChangeUserRoleResponseDto> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.NotFound => Notification<TurnUserModeratorResponse>.Create(null!, ProcessingStatusEnum.CouldNotFindUser, "Provided user was not found."),
            HttpStatusCode.BadRequest => Notification<TurnUserModeratorResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, "Informations provided for processing were not valid."),
            HttpStatusCode.InternalServerError => Notification<TurnUserModeratorResponse>.Create(null!, ProcessingStatusEnum.ExternalDependecyFailed, "External server could not process your request."),
            _ => Notification<TurnUserModeratorResponse>.Create(null!, ProcessingStatusEnum.ServerHadProblemsProcessingRequest, "Our server had problem processing your request. Please contact our support.")
        };
    }
}
