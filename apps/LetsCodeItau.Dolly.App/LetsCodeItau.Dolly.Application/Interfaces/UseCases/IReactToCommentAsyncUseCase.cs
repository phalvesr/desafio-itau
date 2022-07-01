using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Requests.Comments;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface IReactToCommentAsyncUseCase
{
    Task<Notification<ReactToMovieResponse>> ExecuteAsync(ReactToCommentRequest request, string globalId);
}
