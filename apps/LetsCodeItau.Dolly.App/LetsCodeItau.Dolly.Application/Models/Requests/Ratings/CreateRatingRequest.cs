namespace LetsCodeItau.Dolly.Application.Models.Requests.Ratings;

public record CreateRatingRequest
(
    int MovieId,
    int Rating
);
