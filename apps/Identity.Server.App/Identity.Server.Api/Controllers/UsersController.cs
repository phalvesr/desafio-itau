using Identity.Server.Api.Presenters;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Server.Api.Controllers;

[Route("users")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : ControllerBase
{
    private readonly ICreateUserAsyncUseCase createUser;

    public UsersController(ICreateUserAsyncUseCase createUser)
    {
        this.createUser = createUser;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var result = await createUser.ExecuteAsync(request);

        return ActionPresenters.RestRepresentation(result);
    }
}
