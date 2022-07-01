using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.Interfaces.Events;

public interface IAppEventHandler
{
    Task HandleMovieEvaluation(User user);
    Task HandleCommentReply(User user);
}
