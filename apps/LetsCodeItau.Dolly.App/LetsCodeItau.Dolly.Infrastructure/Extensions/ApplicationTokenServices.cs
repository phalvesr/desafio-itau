using LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.IdentityServer;
using Microsoft.Extensions.DependencyInjection;

namespace LetsCodeItau.Dolly.Infrastructure.Extensions;

public static class ApplicationTokenServices
{
    public static IServiceCollection AddApplicationToken(this IServiceCollection services)
    {
        services.AddSingleton<TokenProvider>();
        services.AddSingleton<ITokenProvider>(sp => sp.GetRequiredService<TokenProvider>());
        services.AddHostedService(sp => sp.GetRequiredService<TokenProvider>());

        return services;
    }
}
