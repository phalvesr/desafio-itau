using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.Repositories;

public class RatingRepository : RepositoryBase<Rating>
{
    public RatingRepository(MoviesContext context) : base(context) { }
}
