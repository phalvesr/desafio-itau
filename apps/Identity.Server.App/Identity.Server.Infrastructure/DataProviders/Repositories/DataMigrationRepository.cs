using System.Data;
using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;

namespace Identity.Server.Infrastructure.DataProviders.Repositories;

public class DataMigrationRepository : IDataMigrationRepository
{
    private readonly IDbConnection dbConnection;
    private readonly IDapperWrapper dapperWrapper;

    public DataMigrationRepository(
        IDbConnection dbConnection,
        IDapperWrapper dapperWrapper)
    {
        this.dbConnection = dbConnection;
        this.dapperWrapper = dapperWrapper;
    }

    public async Task MigrateDataAsync()
    {
        await AssureUsersAuthTableCreatedAsync();
        await AssureUsersRoleTableCreatedAsync();
        await AssureAppsAuthTableCreatedAsync();
        await AssureDollyApiAppCredentionsCreatedAsync();
    }

    private async Task AssureUsersAuthTableCreatedAsync()
    {
        var query = @"
            CREATE TABLE IF NOT EXISTS users_auth (
                user_auth_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                global_id TEXT NOT NULL,
                username TEXT NOT NULL,
                encrypted_password TEXT NOT NULL
            );";

        await dapperWrapper.ExecuteAsync(dbConnection, query);
    }

    private async Task AssureAppsAuthTableCreatedAsync()
    {
        var query = @"
            CREATE TABLE IF NOT EXISTS apps_auth (
                app_auth_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                client_key TEXT NOT NULL,
                client_secret TEXT NOT NULL
            );";

        await dapperWrapper.ExecuteAsync(dbConnection, query);
    }

    private async Task AssureUsersRoleTableCreatedAsync()
    {
        var query = @"
            CREATE TABLE IF NOT EXISTS users_roles (
                user_role_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                user_auth_id INTEGER NOT NULL,
                global_id TEXT NOT NULL,
                role TEXT NOT NULL DEFAULT 'READER',
                FOREIGN KEY(user_auth_id) REFERENCES users_auth(user_auth_id)
            );";

        await dapperWrapper.ExecuteAsync(dbConnection, query);
    }

    private async Task AssureDollyApiAppCredentionsCreatedAsync()
    {
        var query = @"
            INSERT INTO apps_auth (
                client_key, client_secret
            ) SELECT 
                'e86c091b-ee8d-41e9-a09e-f27d6340d063', 
                'b48af16d-de60-4400-b45b-f638156f38f8'
            WHERE NOT EXISTS (SELECT 1 FROM apps_auth WHERE client_key = 'e86c091b-ee8d-41e9-a09e-f27d6340d063');";

        await dapperWrapper.ExecuteAsync(dbConnection, query);
    }
}
