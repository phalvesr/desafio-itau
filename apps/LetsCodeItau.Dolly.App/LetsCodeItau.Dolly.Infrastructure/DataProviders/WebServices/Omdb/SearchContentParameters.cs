using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.Omdb;

public class SearchContentParameters
{
    [AliasAs("page")]
    public int Page { get; set; }

    private string searchTerm = default!;
    [AliasAs("s")]
    public string SearchTerm
    {
        get
        {
            return searchTerm;
        }
        set
        {
            searchTerm = value.Replace(' ', '-');
        }
    }
}
