using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchContent;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationById;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationByTitle;
using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.Omdb;

public interface IOmdbApi
{
    [Get("/apikey={apiKey}&type=movie")]
    Task<IApiResponse<SearchMovieByTitleResponseDto>> SearchContentAsync(
        string apiKey,
        SearchContentParameters parameters);

    [Get("/")]
    Task<IApiResponse<SearchMovieInformationByIdResponseDto>> SearchMovieInformationByIdAsync(
        SearchMovieInformationByImdbIdParameters parameters);

    [Get("/")]
    Task<IApiResponse<SearchMovieInformationByTitleResponseDto>> SearchMovieInformationByTitleAsync(
        SearchMovieInformationByTitleParameters parameters);
}
