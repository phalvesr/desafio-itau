namespace LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationByTitle;

public class SearchMovieInformationByTitleResponseDto
{
    public string Title { get; set; } = default!;
    public string Year { get; set; } = default!;
    public string Rated { get; set; } = default!;
    public DateTime Realeased { get; set; }
    public string Runtime { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public string Director { get; set; } = default!;
    public string Writer { get; set; } = default!;
    public string Actors { get; set; } = default!;
    public string Plot { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string Awards { get; set; } = default!;
    public string Poster { get; set; } = default!;
    public IEnumerable<RatingDto> Ratings { get; set; }
    public string Metascore { get; set; } = default!;
    public string ImdbRating { get; set; } = default!;
    public string ImdbVotes { get; set; } = default!;
    public string ImdbId { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Dvd { get; set; } = default!;
    public string BoxOffice { get; set; } = default!;
    public string Production { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Response { get; set; } = default;
}
