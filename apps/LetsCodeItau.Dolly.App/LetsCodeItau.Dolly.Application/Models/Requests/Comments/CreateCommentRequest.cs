namespace LetsCodeItau.Dolly.Application.Models.Requests.Comments;

public record CreateCommentRequest
(
    string Content,
    int MovieId
);
