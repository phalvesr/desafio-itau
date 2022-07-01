using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface ISearchMovieByTitleAsyncUseCase
{
    Task<Notification<SearchMovieByTitleResponse>> ExecuteAsync(string title, string year);
}
