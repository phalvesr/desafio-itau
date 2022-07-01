using Identity.Server.Application.Models;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Application.Models.Responses;

namespace Identity.Server.Application.Interfaces.UseCases;

public interface IChangeUserRoleAsyncUseCase
{
    Task<StatusNotification<UpdateUserRoleResponse>> ExecuteAsync(ChangeUserRoleRequest request);
}
