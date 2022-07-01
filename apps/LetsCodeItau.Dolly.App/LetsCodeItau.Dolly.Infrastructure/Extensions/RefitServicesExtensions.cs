using LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.IdentityServer;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.Omdb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.Extensions;

internal static class RefitServicesExtensions
{
    internal static IServiceCollection AddRefit(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IdentityServerTokenHandler>();

        services.AddRefitClient<IIdentityServerApi>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(configuration["IdentityServer:BaseUrl"]);
        }).AddHttpMessageHandler<IdentityServerTokenHandler>();

        services.AddRefitClient<IOmdbApi>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("http://www.omdbapi.com");
        });

        return services;
    }
}
