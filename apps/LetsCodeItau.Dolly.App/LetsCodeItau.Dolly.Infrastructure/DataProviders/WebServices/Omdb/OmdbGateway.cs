using System.Net;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchContent;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationById;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationByTitle;
using Microsoft.Extensions.Configuration;
using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.Omdb;

public class OmdbGateway : IOmdbGateway
{
    private readonly IOmdbApi omdbApi;
    private readonly IConfiguration configuration;

    public OmdbGateway(
        IOmdbApi omdbApi,
        IConfiguration configuration)
    {
        this.omdbApi = omdbApi;
        this.configuration = configuration;
    }

    public Task<Result<SearchMovieByTitleResponseDto>> SearchContentByTitle(string title)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<SearchMovieInformationByIdResponseDto>> SearchMovieInformationById(string imdbId)
    {
        try
        {
            var parameters = new SearchMovieInformationByImdbIdParameters
            {
                ApiKey = configuration["OmdbApiKey"],
                ImdbId = imdbId
            };

            var response = await omdbApi.SearchMovieInformationByIdAsync(parameters);

            if (!response.IsSuccessStatusCode)
            {
                return Result<SearchMovieInformationByIdResponseDto>.Create(null!, false, response.StatusCode, response.Error?.Content!);
            }

            if (response.Content!.Response != "True")
            {
                return Result<SearchMovieInformationByIdResponseDto>.Create(
                    null!, false, HttpStatusCode.NotFound, response.Content.Response!);
            }

            return GetResult(response);
        }
        catch (HttpRequestException)
        {
            return GetResultForExternalDependencyIntegrationFailure<SearchMovieInformationByIdResponseDto>();
        }
        catch (Exception)
        {
            return GetResultForInternalServerError<SearchMovieInformationByIdResponseDto>();
        }
    }

    public async Task<Result<SearchMovieInformationByTitleResponseDto>> SearchMovieInformationByTitle(string title, string year)
    {
        try
        {
            var requestParameters = new SearchMovieInformationByTitleParameters
            {
                Title = title,
                Year = year,
                ApiKey = configuration["OmdbApiKey"]
            };
            var response = await omdbApi.SearchMovieInformationByTitleAsync(requestParameters);

            if (response.Content!.Response != "True")
            {
                return Result<SearchMovieInformationByTitleResponseDto>.Create(
                    null!, false, HttpStatusCode.NotFound, response.Content.Response!);
            }

            return GetResult(response);
        }
        catch (HttpRequestException)
        {
            return GetResultForExternalDependencyIntegrationFailure<SearchMovieInformationByTitleResponseDto>();
        }
        catch (Exception)
        {
            return GetResultForInternalServerError<SearchMovieInformationByTitleResponseDto>();
        }
    }

    private Result<T> GetResult<T>(IApiResponse<T> response) where T : class
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = response.Error?.Content;

            return Result<T>.Create(null!, false, response.StatusCode, error!);
        }

        var content = response.Content;

        return Result<T>.Create(content!, true, response.StatusCode, null!);
    }

    private Result<T> GetResultForExternalDependencyIntegrationFailure<T>() where T : class
    {
        return Result<T>.Create(null!, false, HttpStatusCode.BadGateway, "Failed while connectiong to external dependency.");
    }

    private Result<T> GetResultForInternalServerError<T>() where T : class
    {
        return Result<T>.Create(null!, false, HttpStatusCode.InternalServerError, string.Empty);
    }
}
