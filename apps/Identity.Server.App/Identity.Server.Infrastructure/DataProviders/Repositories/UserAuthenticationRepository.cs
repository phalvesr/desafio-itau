using System.Data;
using Dapper;
using Identity.Server.Domain.Entities;
using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;

namespace Identity.Server.Infrastructure.DataProviders.Repositories;

public class UserAuthenticationRepository : IUserAuthenticationRepository
{
    private readonly IDbConnection dbConnection;
    private readonly IDapperWrapper dapperWrapper;

    public UserAuthenticationRepository(IDbConnection dbConnection,
        IDapperWrapper dapperWrapper)
    {
        this.dbConnection = dbConnection;
        this.dapperWrapper = dapperWrapper;
    }

    public async Task<UsersAuthEntity> GetUserByUsernameOrDefaultAsync(string username)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = @"
            SELECT * FROM users_auth WHERE username = @Username LIMIT 1;";

        var entity = await dapperWrapper.QueryFirstOrDefaultAsync<UsersAuthEntity>(dbConnection, query, new
        {
            Username = username
        });

        return entity;
    }

    public async Task<UsersRolesEntity> GetUserRoleByUserAuthIdAsync(int userAuthId)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = @"
            SELECT * FROM users_roles WHERE user_auth_id = @UserAuthId LIMIT 1";

        var entity = await dapperWrapper.QueryFirstOrDefaultAsync<UsersRolesEntity>(dbConnection, query, new
        {
            UserAuthId = userAuthId
        });

        return entity;
    }
}
