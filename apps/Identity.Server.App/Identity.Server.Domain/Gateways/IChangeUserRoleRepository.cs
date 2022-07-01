using Identity.Server.Domain.Entities;

namespace Identity.Server.Domain.Gateways;

public interface IChangeUserRoleRepository
{
    Task<UsersRolesEntity> GetUserByGlobalIdAsync(string globalId);
    Task<bool> UpdateUserRoleEntityAsync(UsersRolesEntity entity);
}
