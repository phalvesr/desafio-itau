using System.Security.Claims;
using LetsCodeItau.Dolly.Api.Presenters;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Users;
using LetsCodeItau.Dolly.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCodeItau.Dolly.Api.Controllers;


[ApiController]
[Route("users")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : ControllerBase
{
    private readonly ICreateUserAsyncUseCase createUser;
    private readonly ILogUserInAsyncUseCase loginUseCase;
    private readonly IFindUserDataAsyncUseCase findUserDataUseCase;

    public UsersController(
        ICreateUserAsyncUseCase createUser,
        ILogUserInAsyncUseCase loginUseCase,
        IFindUserDataAsyncUseCase findUserDataUseCase)
    {
        this.createUser = createUser;
        this.loginUseCase = loginUseCase;
        this.findUserDataUseCase = findUserDataUseCase;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUserAsync(
        [FromBody] CreateRequest request)
    {
        var result = await createUser.ExecuteAsync(request);

        return ActionPresenters.RestPresenter(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LogUserInAsync([FromBody] LoginRequest request)
    {
        var result = await loginUseCase.ExecuteAsync(request);

        return ActionPresenters.RestPresenter(result);
    }

    [HttpGet]
    [Authorize(Roles = "READER,BASIC,ADVANCED,MODERATOR")]
    public async Task<IActionResult> GeProfileDataAsync()
    {
        var globalId = User.FindFirstValue("GlobalId");

        var result = await findUserDataUseCase.ExecuteAsync(globalId);

        return ActionPresenters.RestPresenter(result);
    }
}
