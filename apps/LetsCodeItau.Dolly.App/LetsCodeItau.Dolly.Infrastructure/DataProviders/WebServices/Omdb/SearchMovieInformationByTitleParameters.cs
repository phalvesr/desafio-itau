using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.Omdb;

public class SearchMovieInformationByTitleParameters
{
    private string title = default!;
    [AliasAs("t")]
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value.Replace(' ', '-');
        }
    }
    [AliasAs("year")]
    public string Year { get; set; } = default!;

    [AliasAs("apikey")]
    public string ApiKey { get; set; } = default!;

    [AliasAs("type")]
    public string Type { get; set; } = "movie";
}
