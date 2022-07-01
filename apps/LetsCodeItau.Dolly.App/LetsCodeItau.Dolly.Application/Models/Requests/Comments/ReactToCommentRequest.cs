using LetsCodeItau.Dolly.Domain.Enums;
namespace LetsCodeItau.Dolly.Application.Models.Requests.Comments;

public class ReactToCommentRequest
{
    public int CommentId { get; set; }
    public ReactionEnum Reaction { get; set; }
}
