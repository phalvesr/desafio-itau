using LetsCodeItau.Dolly.Api.Presenters;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCodeItau.Dolly.Api.Controllers;

[ApiController]
[Route("roles")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RolesController : ControllerBase
{
    private readonly ITurnUserModeratorAsyncUseCase turnUserModeratorUseCase;

    public RolesController(
        ITurnUserModeratorAsyncUseCase turnUserModeratorUseCase)
    {
        this.turnUserModeratorUseCase = turnUserModeratorUseCase;
    }

    [HttpPatch("{globalId:guid}")]
    [Authorize(Roles = "MODERATOR")]
    public async Task<IActionResult> UpgradeUserRoleAsync(Guid globalId)
    {
        var result = await turnUserModeratorUseCase.ExecuteAsync(globalId);

        return ActionPresenters.RestPresenter(result);
    }
}
