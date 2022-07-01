using System.Data;
using System.Data.SqlClient;
using Dapper;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Models;
using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.Database.Repositories;

public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
{
    private readonly MoviesContext context;

    public CommentRepository(MoviesContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<int> GetCommentCountAsync()
    {
        var query = "SELECT COUNT(1) FROM Comments";
        var connection = context.Database.GetDbConnection();

        var result = await connection.QueryFirstOrDefaultAsync<int>(query);

        return result;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsPaginatedAsync(int lastIndex, int count)
    {
        var query = @"
        SELECT 
            u.DisplayName AS PostedBy,
            m.Title AS Movie, 
            c.Content AS Content, 
            c.CommentId AS CommentId 
        FROM Comments c
            INNER JOIN Movies m ON m.movieId = c.MovieId
            INNER JOIN Users u on c.UserId = u.UserId
        WHERE c.CommentId > @LastId ORDER BY c.CommentId LIMIT @Count;";

        var connection = context.Database.GetDbConnection();

        return await connection.QueryAsync<CommentDto>(query, new
        {
            LastId = lastIndex,
            Count = count
        });
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var comment = await context.Comments.FindAsync(id);

        if (comment is null)
        {
            return false;
        }

        comment.Deleted = true;
        context.Comments.Update(comment);
        var changedRows = await context.SaveChangesAsync();

        return changedRows > 0;
    }
}
