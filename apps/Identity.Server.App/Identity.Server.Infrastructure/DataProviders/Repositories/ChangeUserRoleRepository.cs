using System.Data;
using Identity.Server.Domain.Entities;
using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;

namespace Identity.Server.Infrastructure.DataProviders.Repositories;

public class ChangeUserRoleRepository : IChangeUserRoleRepository
{
    private readonly IDbConnection dbConnection;
    private readonly IDapperWrapper dapperWrapper;

    public ChangeUserRoleRepository(
        IDbConnection dbConnection,
        IDapperWrapper dapperWrapper)
    {
        this.dbConnection = dbConnection;
        this.dapperWrapper = dapperWrapper;
    }

    public Task<UsersRolesEntity> GetUserByGlobalIdAsync(string globalId)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = @"
            SELECT * FROM users_roles WHERE global_id = @GlobalId;";

        var entity = dapperWrapper.QueryFirstOrDefaultAsync<UsersRolesEntity>(dbConnection, query, new
        {
            GlobalId = globalId
        });

        return entity;
    }

    public async Task<bool> UpdateUserRoleEntityAsync(UsersRolesEntity entity)
    {
        var query = @"UPDATE users_roles 
            SET role = @Role WHERE user_role_id = @UserRoleId;";

        var changedRows = await dapperWrapper.ExecuteAsync(dbConnection, query, entity);

        return changedRows > 0;
    }
}
