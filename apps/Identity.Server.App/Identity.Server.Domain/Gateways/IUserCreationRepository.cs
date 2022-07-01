using Identity.Server.Domain.Entities;

namespace Identity.Server.Domain.Gateways;

public interface IUserCreationRepository
{
    Task<bool> CreateAsync(UsersAuthEntity userEntity, UsersRolesEntity usersRolesEntity);
    Task<UsersAuthEntity> GetUserByUsernameOrDefaultAsync(string username);
}
