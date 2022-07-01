using System.Linq.Expressions;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly DbContext context;
    private readonly DbSet<T> set;

    public RepositoryBase(MoviesContext context)
    {
        this.context = context;
        this.set = context.Set<T>();
    }

    public async Task<int> AddAsync(T entity)
    {
        set.Add(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(T entity)
    {
        return await set.FindAsync(entity) is not null;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await set.SingleOrDefaultAsync(predicate) is not null;
    }

    public async Task<T> FindByIdAsync(int id)
    {
        var entity = await set.FindAsync(id);

        return entity!;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await set.ToListAsync();
    }

    public async Task<T> SelectWhere(Expression<Func<T, bool>> predicate)
    {
        return await set.FirstOrDefaultAsync(predicate)!;
    }

    public async Task UpdateAsync(int id, T entity)
    {
        var result = await set.FindAsync(id);

        if (result is not null)
        {
            context.Entry(result).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();
        }
    }
}
