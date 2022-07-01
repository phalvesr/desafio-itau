using System.Net;
using Identity.Server.Api.Presenters;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Server.Api.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly ISignInUserUseCaseAsync signUserIn;
    private readonly ISignInAppAsyncUseCase signAppIn;
    private readonly IApplyAttemptLimitUseCase attemptLimit;

    public AuthController(
        IConfiguration configuration,
        ISignInUserUseCaseAsync signUserIn,
        ISignInAppAsyncUseCase signAppIn,
        IApplyAttemptLimitUseCase attemptLimit)
    {
        this.configuration = configuration;
        this.signUserIn = signUserIn;
        this.signAppIn = signAppIn;
        this.attemptLimit = attemptLimit;
    }

    [HttpPost("users")]
    public async Task<IActionResult> AuthenticateUserAsync(
        [FromBody] UserSignInRequest request)
    {
        var limitReached = await attemptLimit.ExecuteAsync(request);
        if (limitReached)
        {
            return StatusCode(((int)HttpStatusCode.TooManyRequests), "Attempts limit reached");
        }

        var result = await signUserIn.ExecuteAsync(request);

        return ActionPresenters.RestRepresentation(result);
    }

    [HttpPost("apps")]
    public async Task<IActionResult> AuthenticateAppAsync(
        [FromBody] AppSignInRequest request)
    {
        var result = await signAppIn.ExecuteAsync(request);

        return ActionPresenters.RestRepresentation(result);
    }
}

