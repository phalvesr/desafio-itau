using Identity.Server.Api.Presenters;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Server.Api.Controllers;

[Route("roles")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RolesController : ControllerBase
{
    private readonly IChangeUserRoleAsyncUseCase changeUserRole;

    public RolesController(IChangeUserRoleAsyncUseCase changeUserRole)
    {
        this.changeUserRole = changeUserRole;
    }

    [HttpPatch]
    [Authorize(Roles = "APPLICATION,MODERATOR")]
    public async Task<IActionResult> ChangeUserRoleAsync(
        [FromBody] ChangeUserRoleRequest request)
    {
        var result = await changeUserRole.ExecuteAsync(request);

        return ActionPresenters.RestRepresentation(result);
    }
}
