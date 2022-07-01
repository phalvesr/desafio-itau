namespace LetsCodeItau.Dolly.Application.Models;

public class CommentDto
{
    public int CommentId { get; set; }
    public string Content { get; set; } = default!;
    public string PostedBy { get; set; } = default!;
    public string Movie { get; set; } = default!;
}
