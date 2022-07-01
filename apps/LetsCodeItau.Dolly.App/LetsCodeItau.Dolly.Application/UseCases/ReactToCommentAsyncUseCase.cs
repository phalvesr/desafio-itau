using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Events;
using LetsCodeItau.Dolly.Application.Interfaces.Providers;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Comments;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Domain.Enums;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class ReactToCommentAsyncUseCase : IReactToCommentAsyncUseCase
{
    private readonly IUserRepository userRepository;
    private readonly IRepositoryBase<Comment> commentBaseRepository;
    private readonly IRepositoryBase<CommentReaction> commentReactionRepository;
    private readonly IAppEventHandler eventHandler;
    private readonly IDateTimeProvider dateTimeProvider;

    public ReactToCommentAsyncUseCase(
        IUserRepository userRepository,
        IRepositoryBase<Comment> commentBaseRepository,
        IRepositoryBase<CommentReaction> commentReactionRepository,
        IAppEventHandler eventHandler, IDateTimeProvider dateTimeProvider)
    {
        this.userRepository = userRepository;
        this.commentBaseRepository = commentBaseRepository;
        this.commentReactionRepository = commentReactionRepository;
        this.eventHandler = eventHandler;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<Notification<ReactToMovieResponse>> ExecuteAsync(ReactToCommentRequest request, string globalId)
    {
        var user = userRepository.FindByGlobalId(globalId);
        var comment = await commentBaseRepository.FindByIdAsync(request.CommentId);

        if (comment is null)
        {
            return Notification<ReactToMovieResponse>.Create(null!, ProcessingStatusEnum.CouldNotFindResource, "Provided comment is not a valid one.");
        }

        var previousReaction = await GetPreviousReaction(user, comment);

        if (previousReaction is null)
        {
            await AddNewReaction(user, comment, request);

            var updateResponse = new ReactToMovieResponse(dateTimeProvider.DateTimeUtcNow, comment.MovieId, comment.CommentId);

            return Notification<ReactToMovieResponse>.Create(updateResponse, ProcessingStatusEnum.SuccessfullyProcessed);
        }

        previousReaction.Reaction = request.Reaction;
        await commentReactionRepository.UpdateAsync(previousReaction.CommentReactionId, previousReaction);

        var createResponse = new ReactToMovieResponse(dateTimeProvider.DateTimeUtcNow, comment.MovieId, comment.CommentId);

        return Notification<ReactToMovieResponse>.Create(createResponse, ProcessingStatusEnum.SuccessfullyProcessed);
    }

    private async Task AddNewReaction(User user, Comment comment, ReactToCommentRequest request)
    {
        var newCommentReaction = new CommentReaction
        {
            AuthorId = comment.UserId,
            Reaction = request.Reaction,
            ReactId = comment.CommentId
        };

        await commentReactionRepository.AddAsync(newCommentReaction);

        await eventHandler.HandleMovieEvaluation(user);
    }

    private async Task<CommentReaction?> GetPreviousReaction(User user, Comment comment)
    {
        return await commentReactionRepository
            .SelectWhere(r => r.AuthorId == user.UserId && r.ReactId == comment.CommentId);
    }
}
