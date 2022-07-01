using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.Omdb;

public class SearchMovieInformationByImdbIdParameters
{
    [AliasAs("i")]
    public string ImdbId { get; set; } = default!;

    [AliasAs("apikey")]
    public string ApiKey { get; set; } = default!;

    [AliasAs("type")]
    public string Type { get; set; } = "movie";
}
