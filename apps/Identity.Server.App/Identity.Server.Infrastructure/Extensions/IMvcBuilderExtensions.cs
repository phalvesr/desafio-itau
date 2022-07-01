using FluentValidation.AspNetCore;
using Identity.Server.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Infrastructure.Extensions;

public static class IMvcBuilderExtensions
{
    public static IMvcBuilder AddValidations(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<IFluentValidationMarker>();
        });

        return mvcBuilder;
    }
}
