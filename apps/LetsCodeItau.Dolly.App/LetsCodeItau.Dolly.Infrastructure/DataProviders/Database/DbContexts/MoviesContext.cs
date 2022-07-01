using LetsCodeItau.Dolly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;

public class MoviesContext : DbContext
{
    public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
    { }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentReaction> CommentReactions { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<User> Users { get; set; }
}
