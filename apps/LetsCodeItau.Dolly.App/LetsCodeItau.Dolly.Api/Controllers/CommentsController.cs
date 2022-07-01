using System.Security.Claims;
using LetsCodeItau.Dolly.Api.Presenters;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Comments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCodeItau.Dolly.Api.Controllers;

[ApiController]
[Route("comments")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentsController : ControllerBase
{
    private readonly IPostCommentAsyncUseCase postCommentUseCase;
    private readonly IDeleteCommentAsyncUseCase deleteCommentUseCase;
    private readonly IReactToCommentAsyncUseCase reactToCommentUseCase;
    private readonly IGetCommentsAsyncUseCase getCommentsUseCase;

    public CommentsController(
        IPostCommentAsyncUseCase postCommentUseCase,
        IDeleteCommentAsyncUseCase deleteCommentUseCase,
        IReactToCommentAsyncUseCase reactToCommentUseCase,
        IGetCommentsAsyncUseCase getCommentsUseCase)
    {
        this.postCommentUseCase = postCommentUseCase;
        this.deleteCommentUseCase = deleteCommentUseCase;
        this.reactToCommentUseCase = reactToCommentUseCase;
        this.getCommentsUseCase = getCommentsUseCase;
    }

    [HttpPost]
    [Authorize(Roles = "BASIC,ADVANCED,MODERATOR")]
    public async Task<IActionResult> PostAsync([FromBody] CreateCommentRequest request)
    {
        var globalId = User.FindFirstValue("GlobalId");

        var result = await postCommentUseCase.ExecuteAsync(request, globalId);

        return ActionPresenters.RestPresenter(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "BASIC,ADVANCED,MODERATOR")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var userGlobalId = User.FindFirstValue("GlobalId");
        var role = User.FindFirstValue(ClaimTypes.Role);

        var result = await deleteCommentUseCase.ExecuteAsync(id, userGlobalId, role);

        return ActionPresenters.RestPresenter(result);
    }

    [HttpPost("react")]
    [Authorize(Roles = "ADVANCED,MODERATOR")]
    public async Task<IActionResult> ReactAsync([FromBody] ReactToCommentRequest request)
    {
        var globalId = User.FindFirstValue("GlobalId");

        var result = await reactToCommentUseCase.ExecuteAsync(request, globalId);

        return ActionPresenters.RestPresenter(result);
    }

    [HttpGet]
    [Authorize(Roles = "READER,BASIC,ADVANCED,MODERATOR")]
    public async Task<IActionResult> GetCommentsAsync(
        [FromQuery] int lastIndex = 0,
        [FromQuery] int count = 10)
    {
        var result = await getCommentsUseCase.ExecuteAsync(lastIndex, count);

        return ActionPresenters.RestPresenter(result);
    }
}
