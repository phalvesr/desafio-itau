using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface IDeleteCommentAsyncUseCase
{
    Task<Notification<DeleteCommentResponse>> ExecuteAsync(int id, string globalUserId, string role);
}
