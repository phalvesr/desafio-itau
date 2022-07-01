using AutoMapper;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Events;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Ratings;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationById;
using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Domain.Validators;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class CreateRatingFromImdbId : ICreateRatingFromImdbIdAsyncUseCase
{
    private readonly IOmdbGateway omdbGateway;
    private readonly IRepositoryBase<Movie> movieBaseRepository;
    private readonly IRepositoryBase<Rating> ratingBaseRepository;
    private readonly IUserRepository userRepository;
    private readonly IAppEventHandler eventHandler;

    public CreateRatingFromImdbId(
        IOmdbGateway omdbGateway,
        IRepositoryBase<Movie> movieBaseRepository,
        IRepositoryBase<Rating> ratingBaseRepository,
        IUserRepository userRepository,
        IAppEventHandler eventHandler)
    {
        this.omdbGateway = omdbGateway;
        this.movieBaseRepository = movieBaseRepository;
        this.ratingBaseRepository = ratingBaseRepository;
        this.userRepository = userRepository;
        this.eventHandler = eventHandler;
    }

    public async Task<Notification<RateMovieResponse>> ExecuteAsync(CreateRatingFromImdbRequest request, string userGlobalId)
    {
        var validationResult = RatingValidator.IsRatingValid(request.Rating);

        if (!validationResult.Item1)
        {
            return Notification<RateMovieResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, validationResult.Item2);
        }

        var resultDto = await omdbGateway.SearchMovieInformationById(request.ImdbId);

        if (!resultDto.Succeeded)
        {
            return Notification<RateMovieResponse>.Create(null!, ProcessingStatusEnum.ExternalDependecyFailed, "An error happened trying to fetch movie data.");
        }

        var isMovieRegistered = await movieBaseRepository.ExistsAsync(m => m.ImdbId == resultDto.Response.ImdbId);

        if (!isMovieRegistered)
        {
            var newMovie = new Movie
            {
                ImdbId = resultDto.Response.ImdbId,
                Plot = resultDto.Response.Plot,
                Title = resultDto.Response.Title,
                RuntimeMinutes = GetSanitizedRuntime(resultDto)
            };

            await movieBaseRepository.AddAsync(newMovie);
        }

        var registeredMovie = await movieBaseRepository.SelectWhere(m => m.ImdbId == request.ImdbId);
        var user = userRepository.FindByGlobalId(userGlobalId);

        var newRating = new Rating
        {
            MovieId = registeredMovie.MovieId,
            Rate = request.Rating,
            UserId = user.UserId
        };
        await ratingBaseRepository.AddAsync(newRating);

        await eventHandler.HandleMovieEvaluation(user);

        var response = new RateMovieResponse(request.Rating, registeredMovie.MovieId, registeredMovie.Title, registeredMovie.ImdbId);

        return Notification<RateMovieResponse>.Create(response, ProcessingStatusEnum.SuccessfullyProcessed);
    }

    private int GetSanitizedRuntime(Result<SearchMovieInformationByIdResponseDto> resultDto)
    {
        if (int.TryParse(resultDto.Response.Runtime.Replace(" min", string.Empty), out var result))
        {
            return result;
        }
        return 0;
    }
}
