using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface ITurnUserModeratorAsyncUseCase
{
    Task<Notification<TurnUserModeratorResponse>> ExecuteAsync(Guid globalUserId);
}
