using System.Data;
using Identity.Server.Domain.Entities;
using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;

namespace Identity.Server.Infrastructure.DataProviders.Repositories;

public class UserCreationRepository : IUserCreationRepository
{
    private readonly IDapperWrapper dapperWrapper;
    private readonly IDbConnection dbConnection;

    public UserCreationRepository(IDapperWrapper dapperWrapper, IDbConnection dbConnection)
    {
        this.dapperWrapper = dapperWrapper;
        this.dbConnection = dbConnection;
    }

    public async Task<bool> CreateAsync(UsersAuthEntity userEntity, UsersRolesEntity usersRolesEntity)
    {
        dbConnection.Open();
        using var transaction = dbConnection.BeginTransaction();

        try
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var userAuthQuery = @"
                INSERT INTO users_auth (
                    global_id, username, encrypted_password
                ) VALUES (@GlobalId, @Username, @EncryptedPassword);
                SELECT last_insert_rowid();";
            var userRole = @"
                INSERT INTO users_roles (
                    user_auth_id, global_id, role
                ) VALUES (@UserAuthId, @GlobalId, @Role);";

            var authId = await dapperWrapper.QueryFirstOrDefaultAsync<int>(dbConnection, userAuthQuery, userEntity, transaction);

            usersRolesEntity.UserAuthId = authId;

            var roleChangedRows = await dapperWrapper.ExecuteAsync(dbConnection, userRole, usersRolesEntity, transaction);

            if (authId != 0 && roleChangedRows != 0)
            {
                transaction.Commit();
                return true;
            }

            transaction.Rollback();
            return false;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            dbConnection.Close();
            transaction?.Dispose();
        }
    }

    public async Task<UsersAuthEntity> GetUserByUsernameOrDefaultAsync(string username)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = @"
            SELECT * FROM users_auth WHERE username = @Username LIMIT 1;";

        var entity = await dapperWrapper.QueryFirstOrDefaultAsync<UsersAuthEntity>(dbConnection, query, new { Username = username });

        return entity;
    }
}
