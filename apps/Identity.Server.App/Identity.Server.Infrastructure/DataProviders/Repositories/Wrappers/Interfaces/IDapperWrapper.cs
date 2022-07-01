using System.Data;

namespace Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;

public interface IDapperWrapper
{
    Task<int> ExecuteAsync(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    Task<int> InsertAsync<T>(IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null, ISqlAdapter sqlAdapter = null) where T : class;
}
