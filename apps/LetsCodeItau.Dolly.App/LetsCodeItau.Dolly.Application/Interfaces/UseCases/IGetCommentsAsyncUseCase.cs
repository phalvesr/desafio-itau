using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface IGetCommentsAsyncUseCase
{
    Task<Notification<GetCommentsResponse>> ExecuteAsync(int lastIndex, int count);
}
