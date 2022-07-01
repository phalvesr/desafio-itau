using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LetsCodeItau.Dolly.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ContentResult
        {
            Content = context.Exception.Message,
            StatusCode = ((int)HttpStatusCode.InternalServerError),
            ContentType = "application/json"
        };
    }
}
