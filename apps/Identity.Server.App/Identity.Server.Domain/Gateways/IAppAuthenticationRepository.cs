using Identity.Server.Domain.Entities;

namespace Identity.Server.Domain.Gateways;

public interface IAppAuthenticationRepository
{
    Task<AppsAuthEntity> GetAppsAuthEntityByClientKeyAsync(string clientKey);
}
