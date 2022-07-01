using Identity.Server.Application.Models.Requests;

namespace Identity.Server.Application.Interfaces.UseCases;

public interface IApplyAttemptLimitUseCase
{
    Task<bool> ExecuteAsync(UserSignInRequest request);
}
