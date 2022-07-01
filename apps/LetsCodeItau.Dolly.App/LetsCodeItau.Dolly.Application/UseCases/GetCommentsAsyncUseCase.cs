using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Responses;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class GetCommentsAsyncUseCase : IGetCommentsAsyncUseCase
{
    const int MAX_COMMENTS_PER_REQUEST = 20;
    private readonly ICommentRepository commentRepository;

    public GetCommentsAsyncUseCase(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task<Notification<GetCommentsResponse>> ExecuteAsync(int lastIndex, int count)
    {
        if (count > MAX_COMMENTS_PER_REQUEST)
        {
            count = MAX_COMMENTS_PER_REQUEST;
        }

        var comments = await commentRepository.GetCommentsPaginatedAsync(lastIndex, count);
        var commentCount = await commentRepository.GetCommentCountAsync();

        var totalPages = (int)Math.Ceiling((decimal)commentCount / count);

        var commentsData = comments.Select(c => new GetCommentsData(c.CommentId, c.Content, c.PostedBy, c.Movie));

        var result = new GetCommentsResponse(
            commentsData, totalPages,
            comments.Last().CommentId, count);

        return Notification<GetCommentsResponse>.Create(result, ProcessingStatusEnum.SuccessfullyProcessed);
    }
}
