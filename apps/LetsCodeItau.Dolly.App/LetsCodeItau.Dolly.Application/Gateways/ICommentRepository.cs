using LetsCodeItau.Dolly.Application.Models;

namespace LetsCodeItau.Dolly.Application.Gateways;

public interface ICommentRepository
{
    Task<bool> SoftDeleteAsync(int id);
    Task<IEnumerable<CommentDto>> GetCommentsPaginatedAsync(int lastIndex, int count);
    Task<int> GetCommentCountAsync();
}
