using System.Net;
using AutoMapper;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationByTitle;
using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class SearchMovieByTitleAsyncUseCase : ISearchMovieByTitleAsyncUseCase
{
    private readonly IOmdbGateway omdbGateway;
    private readonly IMapper mapper;
    private readonly IRepositoryBase<Movie> movieRepository;

    public SearchMovieByTitleAsyncUseCase(
        IOmdbGateway omdbGateway, IMapper mapper,
        IRepositoryBase<Movie> movieRepository)
    {
        this.omdbGateway = omdbGateway;
        this.mapper = mapper;
        this.movieRepository = movieRepository;
    }

    public async Task<Notification<SearchMovieByTitleResponse>> ExecuteAsync(string title, string year)
    {
        var omdbResult = await omdbGateway.SearchMovieInformationByTitle(title, year);

        if (!omdbResult.Succeeded)
        {
            return GetUnsuccessfulStatus(omdbResult);
        }

        var isMovieAlreadyRegistered = await movieRepository.ExistsAsync(x => x.ImdbId == omdbResult.Response.ImdbId);
        if (!isMovieAlreadyRegistered)
        {
            var newMovie = mapper.Map<Movie>(omdbResult.Response);
            await movieRepository.AddAsync(newMovie);
        }

        var result = GetResultFromResponse(omdbResult.Response);

        return GetSuccessfulStatus(result)!;
    }

    private Notification<SearchMovieByTitleResponse> GetUnsuccessfulStatus(Result<SearchMovieInformationByTitleResponseDto> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.NotFound => Notification<SearchMovieByTitleResponse>.Create(null!, ProcessingStatusEnum.CouldNotFindUser, "No results were found while looking for requested movie."),
            HttpStatusCode.BadRequest => Notification<SearchMovieByTitleResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, "Requested server could not process requested movie"),
            _ => Notification<SearchMovieByTitleResponse>.Create(null!, ProcessingStatusEnum.ServerHadProblemsProcessingRequest, "Server had problems processing request.")
        };
    }

    private SearchMovieByTitleResponse GetResultFromResponse(SearchMovieInformationByTitleResponseDto response)
    {
        return new SearchMovieByTitleResponse(
            response.Title, response.Year, response.Runtime.Replace("min", string.Empty).Trim(),
            response.Genre, response.Awards, response.Poster, response.ImdbId, response.Website,
            response.Country, response.Plot, response.Director, response.Writer, response.Actors, response.Language);
    }

    private Notification<SearchMovieByTitleResponse> GetSuccessfulStatus(SearchMovieByTitleResponse response)
    {
        return Notification<SearchMovieByTitleResponse>.Create(response!, ProcessingStatusEnum.SuccessfullyProcessed);
    }
}
