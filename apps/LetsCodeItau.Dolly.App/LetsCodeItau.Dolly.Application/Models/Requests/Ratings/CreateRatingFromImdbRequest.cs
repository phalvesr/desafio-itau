namespace LetsCodeItau.Dolly.Application.Models.Requests.Ratings;

public record CreateRatingFromImdbRequest
(
    string ImdbId,
    int Rating
);
