namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record TurnUserModeratorResponse
(
    string NewRole,
    DateTime UpgradedAt,
    Guid UpgradedUserGlobalId
);
