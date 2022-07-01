using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Models.Requests.Users;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.Interfaces.UseCases;

public interface ICreateUserAsyncUseCase
{
    Task<Notification<CreateUserResponse>> ExecuteAsync(CreateRequest request);
}
