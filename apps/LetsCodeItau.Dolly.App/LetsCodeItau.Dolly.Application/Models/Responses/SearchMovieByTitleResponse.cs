namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record SearchMovieByTitleResponse
(
    string Title,
    string Year,
    string RuntimeMinutes,
    string Genre,
    string Awards,
    string Poster,
    string ImdbId,
    string Website,
    string Country,
    string Plot,
    string Director,
    string Writers,
    string Actors,
    string Languages
);
