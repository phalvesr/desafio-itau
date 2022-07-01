using System.Data;
using System.Data.SQLite;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Infrastructure.Extensions;

internal static class DatabaseExtensions
{
    internal static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var sqliteConnectionString = $"Data Source={configuration["SqliteDatabasePath"]}\\security_token_service.db;Version=3;";

        services.AddScoped<IDbConnection>(_ => new SQLiteConnection(sqliteConnectionString));

        return services;
    }
}
