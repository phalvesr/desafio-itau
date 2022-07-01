using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.Repositories;

public class MovieRepository : RepositoryBase<Movie>
{
    public MovieRepository(MoviesContext context) : base(context) { }
}
