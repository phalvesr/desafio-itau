using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;

namespace Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers;

public class DapperWrapper : IDapperWrapper
{
    public async Task<int> ExecuteAsync(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
    }

    public async Task<int> InsertAsync<T>(IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null, ISqlAdapter sqlAdapter = null) where T : class
    {
        return await connection.InsertAsync<T>(entityToInsert, transaction, commandTimeout, sqlAdapter);
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await cnn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
    }
}
