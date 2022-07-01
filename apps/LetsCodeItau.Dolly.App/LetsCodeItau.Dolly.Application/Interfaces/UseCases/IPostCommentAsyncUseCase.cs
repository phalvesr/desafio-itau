using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Requests.Comments;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface IPostCommentAsyncUseCase
{
    Task<Notification<CreateCommentResponse>> ExecuteAsync(CreateCommentRequest request, string userGlobalId);
}
