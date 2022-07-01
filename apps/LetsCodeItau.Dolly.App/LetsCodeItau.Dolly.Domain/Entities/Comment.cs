namespace LetsCodeItau.Dolly.Domain.Entities;

public class Comment
{
    public int CommentId { get; set; }
    public int MovieId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = default!;
    public int? ReplyId { get; set; }
    public bool Deleted { get; set; } = false;
}
