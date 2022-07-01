namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record ReactToMovieResponse
(
    DateTime ReactedAt,
    int MovieId,
    int CommentId
);
