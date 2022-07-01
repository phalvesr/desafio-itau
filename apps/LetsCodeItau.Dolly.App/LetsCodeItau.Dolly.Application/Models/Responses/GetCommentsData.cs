namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record GetCommentsData
(
    int CommentId,
    string Content,
    string PostedBy,
    string Movie
);
