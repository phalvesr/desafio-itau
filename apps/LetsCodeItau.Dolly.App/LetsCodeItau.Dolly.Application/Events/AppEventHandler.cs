using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Events;
using LetsCodeItau.Dolly.Domain.Entities;
using Serilog;

namespace LetsCodeItau.Dolly.Application.Events;

public class AppEventHandler : IAppEventHandler
{
    private readonly IUpgradeUserRoleEventHandler upgradeRoleHandler;
    private readonly IRepositoryBase<User> userBaseRepository;
    private readonly ILogger logger;

    public AppEventHandler(
        IUpgradeUserRoleEventHandler upgradeRoleHandler,
        IRepositoryBase<User> userBaseRepository,
        ILogger logger)
    {
        this.upgradeRoleHandler = upgradeRoleHandler;
        this.userBaseRepository = userBaseRepository;
        this.logger = logger;
    }

    public async Task HandleCommentReply(User user)
    {
        const int POINTS_PER_COMMENT_REPLY = 1;

        user.Points += POINTS_PER_COMMENT_REPLY;

        await userBaseRepository.UpdateAsync(user.UserId, user);

        await upgradeRoleHandler.HandleRoleUpgrade(user);
    }

    public async Task HandleMovieEvaluation(User user)
    {
        const int POINTS_PER_MOVIE_EVALUATION = 1;

        user.Points += POINTS_PER_MOVIE_EVALUATION;

        await userBaseRepository.UpdateAsync(user.UserId, user);

        await upgradeRoleHandler.HandleRoleUpgrade(user);
    }
}
