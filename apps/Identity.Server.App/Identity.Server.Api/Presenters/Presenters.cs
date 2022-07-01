using System.Net;
using System.Text.Json;
using Identity.Server.Application.Enums;
using Identity.Server.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Server.Api.Presenters;

internal static class ActionPresenters
{
    private static string contentType = "application/json";

    internal static IActionResult RestRepresentation<T>(StatusNotification<T> result)
    {
        var status = result.Status;

        return status switch
        {
            ProcessingStatusEnum.CouldNotFindUser => CreteNotFoundContentResult(),
            ProcessingStatusEnum.ServerHadProblemsProcessingRequest => CreteInternalServerErrorContentResult(),
            ProcessingStatusEnum.SuccessfullyProcessed => CreteSuccessfullContentResult(result.Response),
            ProcessingStatusEnum.UserAlreadyRegistered => CreateConflictContentResult(),
            ProcessingStatusEnum.WrongCredentials => CreteBadRequestContentResult(),
            _ => CreteInternalServerErrorContentResult()
        };
    }

    private static ContentResult CreteSuccessfullContentResult<T>(T response)
    {
        return new ContentResult
        {
            StatusCode = ((int)HttpStatusCode.OK),
            ContentType = contentType,
            Content = JsonSerializer.Serialize(response)
        };
    }

    private static ContentResult CreteNotFoundContentResult()
    {
        return new ContentResult
        {
            StatusCode = ((int)HttpStatusCode.NotFound),
            ContentType = contentType,
            Content = JsonSerializer.Serialize("Username could not be found on server")
        };
    }

    private static ContentResult CreteInternalServerErrorContentResult()
    {
        return new ContentResult
        {
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.InternalServerError),
            Content = JsonSerializer.Serialize("Server had problems precessing your request. Please contact our support.")
        };
    }

    private static ContentResult CreteBadRequestContentResult()
    {
        return new ContentResult
        {
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.BadRequest),
            Content = JsonSerializer.Serialize("Wrong parameters provided.")
        };
    }

    private static ContentResult CreateConflictContentResult()
    {
        return new ContentResult
        {
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.Conflict),
            Content = JsonSerializer.Serialize("User credential already been uses by someone else.")
        };
    }
}
