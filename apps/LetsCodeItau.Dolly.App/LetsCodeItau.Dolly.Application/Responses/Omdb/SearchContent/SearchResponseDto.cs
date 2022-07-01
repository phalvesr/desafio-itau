namespace LetsCodeItau.Dolly.Application.Responses.Omdb.SearchContent;

public record SearchResponseDto
(
    string Title,
    string Year,
    string ImdbId,
    string type,
    string Poster
);
