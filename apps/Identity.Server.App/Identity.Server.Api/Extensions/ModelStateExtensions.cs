using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.Server.Api.Extensions;

public static class ModelStateExtensions
{
    public static IEnumerable<string> GetErrors(this ModelStateDictionary modelState)
    {
        return modelState
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage);
    }
}
