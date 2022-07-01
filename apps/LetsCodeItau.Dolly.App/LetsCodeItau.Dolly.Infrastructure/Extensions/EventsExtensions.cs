using LetsCodeItau.Dolly.Application.Events;
using LetsCodeItau.Dolly.Application.Interfaces.Events;
using Microsoft.Extensions.DependencyInjection;

namespace LetsCodeItau.Dolly.Infrastructure.Extensions;

public static class EventsExtensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddScoped<IAppEventHandler, AppEventHandler>();
        services.AddScoped<IUpgradeUserRoleEventHandler, UpgradeUserRoleEventHandler>();

        return services;
    }
}
