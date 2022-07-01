using System.Net;
using System.Text.Json;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LetsCodeItau.Dolly.Api.Presenters;

internal static class ActionPresenters
{
    private static readonly string contentType = "application/json";

    internal static IActionResult RestPresenter<T>(Notification<T> notification)
    {
        return notification.Status switch
        {
            ProcessingStatusEnum.SuccessfullyProcessed => GetResponseOk(notification),
            ProcessingStatusEnum.ResourceCreated => GetResponseCreated(notification),
            ProcessingStatusEnum.CouldNotFindUser => GetResponseNotFound(notification),
            ProcessingStatusEnum.UserAlreadyRegistered => GetResponseConflict(notification),
            ProcessingStatusEnum.WrongCredentials => GetResponseBadRequest(notification),
            ProcessingStatusEnum.ServerHadProblemsProcessingRequest => GetResponseForServerError(notification),
            ProcessingStatusEnum.ExternalDependecyFailed => GetResponseNotOk(notification),
            ProcessingStatusEnum.UserNotAllowedTo => GetResponseInvalidPermission(notification),
            _ => GetResponseForServerError(notification)
        };
    }

    private static IActionResult GetResponseInvalidPermission<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            Content = JsonSerializer.Serialize(new { Data = notification.Errors }),
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.Forbidden)
        };
    }

    private static IActionResult GetResponseCreated<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.Created),
            Content = JsonSerializer.Serialize(notification.Data)
        };
    }

    private static IActionResult GetResponseNotFound<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            Content = JsonSerializer.Serialize(new
            {
                Errors = notification.Errors
            }),
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.NotFound)
        };
    }

    private static IActionResult GetResponseConflict<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            Content = JsonSerializer.Serialize(new
            {
                Errors = notification.Errors
            }),
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.Conflict)
        };
    }

    private static IActionResult GetResponseBadRequest<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            Content = JsonSerializer.Serialize(new
            {
                Errors = notification.Errors
            }),
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.BadRequest)
        };
    }

    private static IActionResult GetResponseExternalFailure<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            Content = JsonSerializer.Serialize(new
            {
                Error = notification.Data
            }),
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.BadGateway)
        };
    }

    private static IActionResult GetResponseNotOk<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            Content = JsonSerializer.Serialize(new
            {
                Error = notification.Errors
            }),
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.BadGateway)
        };
    }

    private static IActionResult GetResponseOk<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            Content = JsonSerializer.Serialize(new
            {
                Data = notification.Data
            }),
            ContentType = contentType,
            StatusCode = ((int)HttpStatusCode.OK)
        };
    }

    private static IActionResult GetResponseForServerError<T>(Notification<T> notification)
    {
        return new ContentResult
        {
            StatusCode = ((int)HttpStatusCode.InternalServerError),
            Content = JsonSerializer.Serialize(new
            {
                Errors = notification.Errors
            }),
            ContentType = contentType
        };
    }
}
