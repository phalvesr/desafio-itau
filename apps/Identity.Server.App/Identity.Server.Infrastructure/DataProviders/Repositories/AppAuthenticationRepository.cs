using System.Data;
using Identity.Server.Domain.Entities;
using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;

namespace Identity.Server.Infrastructure.DataProviders.Repositories;

public class AppAuthenticationRepository : IAppAuthenticationRepository
{
    private readonly IDbConnection dbConnection;
    private readonly IDapperWrapper dapperWrapper;

    public AppAuthenticationRepository(
        IDbConnection dbConnection,
        IDapperWrapper dapperWrapper)
    {
        this.dbConnection = dbConnection;
        this.dapperWrapper = dapperWrapper;
    }
    public async Task<AppsAuthEntity> GetAppsAuthEntityByClientKeyAsync(string clientKey)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        try
        {
            var query = @"SELECT * FROM apps_auth WHERE client_key = @ClientKey";

            var entity = await dapperWrapper.QueryFirstOrDefaultAsync<AppsAuthEntity>(dbConnection, query, new
            {
                ClientKey = clientKey
            });

            return entity;
        }
        catch
        {
            throw;
        }
    }
}
