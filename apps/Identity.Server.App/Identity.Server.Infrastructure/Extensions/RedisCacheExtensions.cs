using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.DataProviders.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Identity.Server.Infrastructure.Extensions;

internal static class RedisCacheExtensions
{
    internal static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"])
        );

        services.AddTransient(typeof(ICacheRepository<>), typeof(CacheRepository<>));

        return services;
    }
}
