using Identity.Server.Application.Enums;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Application.Models.Responses;
using Identity.Server.Domain.Enums;
using Identity.Server.Domain.Gateways;

namespace Identity.Server.Application.UseCases;

public class ChangeUserRoleAsyncUseCase : IChangeUserRoleAsyncUseCase
{
    private readonly IChangeUserRoleRepository userRoleRepository;

    public ChangeUserRoleAsyncUseCase(IChangeUserRoleRepository userRoleRepository)
    {
        this.userRoleRepository = userRoleRepository;
    }

    public async Task<StatusNotification<UpdateUserRoleResponse>> ExecuteAsync(ChangeUserRoleRequest request)
    {
        var entity = await userRoleRepository.GetUserByGlobalIdAsync(request.UserId.ToString());

        if (entity is null)
        {
            return StatusNotification<UpdateUserRoleResponse>.Create(null, ProcessingStatusEnum.CouldNotFindUser);
        }

        if (!Enum.TryParse<UserRolesEnum>(request.Role, true, out var newRole))
        {
            return StatusNotification<UpdateUserRoleResponse>.Create(null, ProcessingStatusEnum.WrongCredentials);
        }

        entity.Role = newRole.StringRepresentation();

        var updated = await userRoleRepository.UpdateUserRoleEntityAsync(entity);

        if (!updated)
        {
            return StatusNotification<UpdateUserRoleResponse>.Create(null, ProcessingStatusEnum.ServerHadProblemsProcessingRequest);
        }

        var response = new UpdateUserRoleResponse(Guid.Parse(entity.GlobalId), entity.Role);
        return StatusNotification<UpdateUserRoleResponse>.Create(response, ProcessingStatusEnum.SuccessfullyProcessed);
    }
}
