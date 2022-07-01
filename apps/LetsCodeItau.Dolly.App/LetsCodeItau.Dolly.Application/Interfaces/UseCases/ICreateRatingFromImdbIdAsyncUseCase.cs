using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Requests.Ratings;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface ICreateRatingFromImdbIdAsyncUseCase
{
    Task<Notification<RateMovieResponse>> ExecuteAsync(CreateRatingFromImdbRequest request, string userGlobalId);
}
