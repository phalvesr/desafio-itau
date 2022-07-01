namespace LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationById;

public record SearchMovieInformationByIdResponseDto
(
    string Title,
    string Year,
    string Rated,
    DateTime Realeased,
    string Runtime,
    string Genre,
    string Director,
    string Writer,
    string Actors,
    string Plot,
    string Language,
    string Country,
    string Awards,
    string Poster,
    IEnumerable<RatingResponseDto> Ratings,
    string Metascore,
    string ImdbRating,
    string ImdbVotes,
    string ImdbId,
    string type,
    string Dvd,
    string BoxOffice,
    string Production,
    string Website,
    string Response
);
