using System.Security.Claims;
using LetsCodeItau.Dolly.Api.Presenters;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Ratings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCodeItau.Dolly.Api.Controllers;

[ApiController]
[Route("ratings")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RatingsController : ControllerBase
{
    private readonly IRateMovieAsyncUseCase rateMovieUseCase;
    private readonly ICreateRatingFromImdbIdAsyncUseCase createRatingFromImdbUseCase;

    public RatingsController(
        IRateMovieAsyncUseCase rateMovieUseCase,
        ICreateRatingFromImdbIdAsyncUseCase createRatingFromImdbUseCase)
    {
        this.rateMovieUseCase = rateMovieUseCase;
        this.createRatingFromImdbUseCase = createRatingFromImdbUseCase;
    }

    [HttpPost]
    [Authorize(Roles = "READER,BASIC,ADVANCED,MODERATOR")]
    public async Task<IActionResult> PostRatingAsync([FromBody] CreateRatingRequest request)
    {
        var globalUserId = User.FindFirstValue("GlobalId");

        var result = await rateMovieUseCase.ExecuteAsync(request, globalUserId);

        return ActionPresenters.RestPresenter(result);
    }

    [HttpPost("imbd")]
    [Authorize(Roles = "READER,BASIC,ADVANCED,MODERATOR")]
    public async Task<IActionResult> PostRatingFromImdbIdAsync([FromBody] CreateRatingFromImdbRequest request)
    {
        var globalUserId = User.FindFirstValue("GlobalId");

        var result = await createRatingFromImdbUseCase.ExecuteAsync(request, globalUserId);

        return ActionPresenters.RestPresenter(result);
    }
}
