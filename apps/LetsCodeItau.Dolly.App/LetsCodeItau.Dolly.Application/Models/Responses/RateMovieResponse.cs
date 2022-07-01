namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record RateMovieResponse
(
    int Rate,
    int MovieId,
    string MovieTitle,
    string ImdbId
);
