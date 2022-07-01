using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Providers;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Domain.Entities;
using LetsCodeItau.Dolly.Domain.Enums;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class DeleteCommentAsyncUseCase : IDeleteCommentAsyncUseCase
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IRepositoryBase<Comment> commentBaseRepository;
    private readonly IDateTimeProvider dateTimeProvider;

    public DeleteCommentAsyncUseCase(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IRepositoryBase<Comment> commentBaseRepository,
        IDateTimeProvider dateTimeProvider)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.commentBaseRepository = commentBaseRepository;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<Notification<DeleteCommentResponse>> ExecuteAsync(
        int id, string globalUserId, string role)
    {

        var user = userRepository.FindByGlobalId(globalUserId);

        var commentToDelete = await commentBaseRepository.FindByIdAsync(id);

        if (!IsUserAllowedToDeleteComment(user, commentToDelete, role))
        {
            return Notification<DeleteCommentResponse>.Create(null!, ProcessingStatusEnum.UserNotAllowedTo, "Users can only delete his own comment must");
        }

        var deleted = await commentRepository.SoftDeleteAsync(id);

        if (deleted)
        {
            var response = new DeleteCommentResponse(dateTimeProvider.DateTimeUtcNow);

            return Notification<DeleteCommentResponse>.Create(response, ProcessingStatusEnum.SuccessfullyProcessed);
        }

        return Notification<DeleteCommentResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, "Server could not delete provided comment.");
    }

    private bool IsUserAllowedToDeleteComment(User user, Comment comment, string role)
    {
        return (role == UserRolesEnum.Moderator.StringRepresentation()
                || comment.UserId != user.UserId);
    }
}
