using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Events;
using LetsCodeItau.Dolly.Application.Requests;
using LetsCodeItau.Dolly.Application.Responses.IdentityServer;
using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Domain.Enums;
using Serilog;

namespace LetsCodeItau.Dolly.Application.Events;

public class UpgradeUserRoleEventHandler : IUpgradeUserRoleEventHandler
{
    private readonly IAuthenticationGateway authenticationGateway;
    private readonly ILogger logger;

    public UpgradeUserRoleEventHandler(
        IAuthenticationGateway authenticationGateway,
        ILogger logger)
    {
        this.authenticationGateway = authenticationGateway;
        this.logger = logger;
    }

    public async Task HandleRoleUpgrade(User user)
    {
        Result<ChangeUserRoleResponseDto> result = null!;

        if (user.Points == 20)
        {
            result = await authenticationGateway.ChangeUserRoleAsync(new ChangeUserRoleRequest(
                    Guid.Parse(user.GlobalId),
                    UserRolesEnum.Basic.StringRepresentation()));
        }
        else if (user.Points == 100)
        {
            result = await authenticationGateway.ChangeUserRoleAsync(
                new ChangeUserRoleRequest(Guid.Parse(user.GlobalId),
                UserRolesEnum.Advanced.StringRepresentation()
            ));
        }
        else if (user.Points == 1000)
        {
            result = await authenticationGateway.ChangeUserRoleAsync(new ChangeUserRoleRequest(
                Guid.Parse(user.GlobalId),
                UserRolesEnum.Moderator.StringRepresentation()
            ));
        }

        if (result is null)
        {
            return;
        }

        if (!result.Succeeded)
        {
            logger.Error("An error happened while updating the user {@User} role. Check it!", user);
            return;
        }

        logger.Information("User {@User} had his role upgrade successfully!");
    }
}
