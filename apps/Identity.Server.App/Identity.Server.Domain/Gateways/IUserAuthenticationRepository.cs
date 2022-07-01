using Identity.Server.Domain.Entities;

namespace Identity.Server.Domain.Gateways;

public interface IUserAuthenticationRepository
{
    Task<UsersAuthEntity> GetUserByUsernameOrDefaultAsync(string username);
    Task<UsersRolesEntity> GetUserRoleByUserAuthIdAsync(int userAuthId);
}
