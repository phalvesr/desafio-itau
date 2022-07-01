using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchContent;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationById;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationByTitle;

namespace LetsCodeItau.Dolly.Application.Gateways;

public interface IOmdbGateway
{
    Task<Result<SearchMovieInformationByTitleResponseDto>> SearchMovieInformationByTitle(string title, string year);
    Task<Result<SearchMovieInformationByIdResponseDto>> SearchMovieInformationById(string imdbId);
    Task<Result<SearchMovieByTitleResponseDto>> SearchContentByTitle(string title);
}
