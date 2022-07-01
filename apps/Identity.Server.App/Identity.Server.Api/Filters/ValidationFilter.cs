using System.Net;
using System.Text.Json;
using Identity.Server.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Server.Api.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.GetErrors();

            context.Result = new ContentResult
            {
                Content = JsonSerializer.Serialize(new
                {
                    Errors = errors
                }),
                StatusCode = ((int)HttpStatusCode.BadRequest),
                ContentType = "application/json"
            };
        }
    }
}
