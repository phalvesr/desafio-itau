using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Requests.Ratings;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface IRateMovieAsyncUseCase
{
    Task<Notification<RateMovieResponse>> ExecuteAsync(CreateRatingRequest request, string globalUserId);
}
