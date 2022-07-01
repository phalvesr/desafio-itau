using LetsCodeItau.Dolly.Api.Presenters;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCodeItau.Dolly.Api.Controllers;

[ApiController]
[Route("movies")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MoviesController : ControllerBase
{
    private readonly ISearchMovieByTitleAsyncUseCase searchByTitleUseCase;

    public MoviesController(ISearchMovieByTitleAsyncUseCase searchByTitleUseCase)
    {
        this.searchByTitleUseCase = searchByTitleUseCase;
    }

    [HttpGet("search/{title}")]
    [Authorize(Roles = "READER,BASIC,ADVANCED,MODERATOR")]
    public async Task<IActionResult> SeachMovieDataByTitle(
        [FromQuery] string? year,
        string title)
    {
        var result = await searchByTitleUseCase.ExecuteAsync(title, year);

        return ActionPresenters.RestPresenter(result);
    }
}
