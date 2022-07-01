using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.Repositories;

public class CommentReactionRepository : RepositoryBase<CommentReaction>
{
    public CommentReactionRepository(MoviesContext context) : base(context)
    {
    }
}
