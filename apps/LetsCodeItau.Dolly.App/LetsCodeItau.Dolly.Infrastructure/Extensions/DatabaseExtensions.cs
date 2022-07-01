using System.Data;
using System.Data.SQLite;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsCodeItau.Dolly.Infrastructure.Extensions;

internal static class DatabaseExtensions
{
    internal static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringBase = configuration.GetConnectionString("SqliteBase");
        var connectionString = string.Format(connectionStringBase, configuration["DatabaseLocation"]);

        services.AddScoped<IDbConnection>(_ => new SQLiteConnection(connectionString));
        services.AddTransient<IMigrationGateway, MigrationGateway>();

        services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();

        services.AddDbContext<MoviesContext>(options => options.UseSqlite(connectionString.Replace(";Version=3", "")));

        return services;
    }
}
