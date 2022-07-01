using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Events;
using LetsCodeItau.Dolly.Application.Interfaces.Providers;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Requests.Comments;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class PostCommentAsyncUseCase : IPostCommentAsyncUseCase
{
    private readonly IRepositoryBase<Movie> movieRepository;
    private readonly IRepositoryBase<User> userRepository;
    private readonly IRepositoryBase<Comment> commentRepository;
    private readonly IAppEventHandler eventHandler;
    private readonly IDateTimeProvider dateTimeProvider;

    public PostCommentAsyncUseCase(
        IRepositoryBase<Movie> movieRepository,
        IRepositoryBase<User> userRepository,
        IRepositoryBase<Comment> commentRepository,
        IAppEventHandler eventHandler,
        IDateTimeProvider dateTimeProvider)
    {
        this.movieRepository = movieRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.eventHandler = eventHandler;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<Notification<CreateCommentResponse>> ExecuteAsync(CreateCommentRequest request, string userGlobalId)
    {
        var isMovieRegistered = await movieRepository.ExistsAsync(movie => movie.MovieId == request.MovieId);
        if (!isMovieRegistered)
        {
            return Notification<CreateCommentResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, "Provided movie is not valid.");
        }

        if (!await IsValidCommenter(userGlobalId))
        {
            return Notification<CreateCommentResponse>.Create(null!, ProcessingStatusEnum.WrongCredentials, "This user is not allowed to post comments.");
        }

        var commenter = await userRepository.SelectWhere(user => user.GlobalId == userGlobalId);

        var comment = new Comment
        {
            Content = request.Content,
            MovieId = request.MovieId,
            UserId = commenter.UserId,
        };

        await commentRepository.AddAsync(comment);

        var result = new CreateCommentResponse(dateTimeProvider.DateTimeUtcNow);
        return Notification<CreateCommentResponse>.Create(result, ProcessingStatusEnum.ResourceCreated);
    }

    private async Task<bool> IsValidCommenter(string userGlobalId)
    {
        return await userRepository.ExistsAsync(user => user.GlobalId == userGlobalId && !user.Deleted);
    }
}
