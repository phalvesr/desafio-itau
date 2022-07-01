using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly MoviesContext context;

    public UserRepository(MoviesContext context) : base(context)
    {
        this.context = context;
    }

    public User FindByGlobalId(string globalId)
    {
        return context.Users.SingleOrDefault(u => u.GlobalId == globalId)!;
    }

    public async Task UpdateLastLogin(string globalId, DateTime lastLoginAt)
    {
        var user = context.Users.SingleOrDefault(x => x.GlobalId == globalId);

        if (user is null)
        {
            return;
        }

        user.LastLogin = lastLoginAt;
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}
