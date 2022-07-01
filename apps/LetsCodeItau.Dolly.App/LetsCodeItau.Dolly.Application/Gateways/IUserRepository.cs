using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.Gateways;

public interface IUserRepository
{
    Task UpdateLastLogin(string globalId, DateTime lastLoginAt);
    User FindByGlobalId(string globalId);
}
