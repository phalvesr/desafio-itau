using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.Interfaces.Events;

public interface IUpgradeUserRoleEventHandler
{
    Task HandleRoleUpgrade(User user);
}
