using Identity.Server.Application.Models;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Application.Models.Responses;

namespace Identity.Server.Application.Interfaces.UseCases;

public interface ISignInUserUseCaseAsync
{
    Task<StatusNotification<UserSignInResponse>> ExecuteAsync(UserSignInRequest request);
}
