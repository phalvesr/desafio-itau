using System.Linq.Expressions;

namespace LetsCodeItau.Dolly.Application.Gateways;

public interface IRepositoryBase<T> where T : class
{
    Task<int> AddAsync(T entity);
    Task<T> FindByIdAsync(int id);
    Task<T> SelectWhere(Expression<Func<T, bool>> predicate);
    Task UpdateAsync(int id, T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> ExistsAsync(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
