using LetsCodeItau.Dolly.Domain.Enums;

namespace LetsCodeItau.Dolly.Domain.Entities;

public class CommentReaction
{
    public int CommentReactionId { get; set; }
    public int AuthorId { get; set; }
    public int ReactId { get; set; }
    public ReactionEnum Reaction { get; set; }
}
