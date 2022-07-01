namespace LetsCodeItau.Dolly.Application.Responses.Omdb.SearchContent;

public record SearchMovieByTitleResponseDto
(
    IEnumerable<SearchResponseDto> Search,
    int TotalResults,
    bool Response
);
