using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Events;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Ratings;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Domain.Validators;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class RateMovieAsyncUseCase : IRateMovieAsyncUseCase
{
    private readonly IRepositoryBase<Movie> movieBaseRepository;
    private readonly IRepositoryBase<User> userBaseRepository;
    private readonly IRepositoryBase<Rating> ratingBaseRepository;
    private readonly IAppEventHandler eventHandler;

    public RateMovieAsyncUseCase(
        IRepositoryBase<Movie> movieBaseRepository,
        IRepositoryBase<User> userBaseRepository,
        IRepositoryBase<Rating> ratingBaseRepository,
        IAppEventHandler eventHandler)
    {
        this.movieBaseRepository = movieBaseRepository;
        this.userBaseRepository = userBaseRepository;
        this.ratingBaseRepository = ratingBaseRepository;
        this.eventHandler = eventHandler;
    }

    public async Task<Notification<RateMovieResponse>> ExecuteAsync(CreateRatingRequest request, string globalUserId)
    {
        var validationResult = RatingValidator.IsRatingValid(request.Rating);

        if (!validationResult.Item1)
        {
            return Notification<RateMovieResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, validationResult.Item2);
        }

        var movie = await movieBaseRepository.FindByIdAsync(request.MovieId);

        if (movie is null)
        {
            return Notification<RateMovieResponse>.Create(null!, ProcessingStatusEnum.CouldNotFindResource, "Movie is not registered on our services");
        }

        var user = await userBaseRepository.SelectWhere(u => u.GlobalId == globalUserId);

        var rating = await ratingBaseRepository.SelectWhere(r => r.UserId == user.UserId && r.MovieId == movie.MovieId);

        if (rating is null)
        {
            await CreateNewRating(request, movie, user);

            await eventHandler.HandleMovieEvaluation(user);

            var ratingCreationResponse = new RateMovieResponse(request.Rating, movie.MovieId, movie.Title, movie.ImdbId);
            return Notification<RateMovieResponse>.Create(ratingCreationResponse, ProcessingStatusEnum.ResourceCreated);
        }

        rating.Rate = request.Rating;

        await ratingBaseRepository.UpdateAsync(rating.RatingId, rating);

        var ratingUpdateResponse = new RateMovieResponse(request.Rating, movie.MovieId, movie.Title, movie.ImdbId);

        return Notification<RateMovieResponse>.Create(ratingUpdateResponse, ProcessingStatusEnum.SuccessfullyProcessed);
    }

    private async Task CreateNewRating(CreateRatingRequest request, Movie? movie, User user)
    {
        var newRating = new Rating
        {
            MovieId = movie.MovieId,
            Rate = request.Rating,
            UserId = user.UserId,
        };

        await ratingBaseRepository.AddAsync(newRating);
    }
}
