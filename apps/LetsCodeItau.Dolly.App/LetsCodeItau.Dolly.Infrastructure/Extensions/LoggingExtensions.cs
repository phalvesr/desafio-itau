using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LetsCodeItau.Dolly.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static IServiceCollection AddSerilog(this IServiceCollection services)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        services.AddSingleton<ILogger>(_ => logger);

        return services;
    }
}
