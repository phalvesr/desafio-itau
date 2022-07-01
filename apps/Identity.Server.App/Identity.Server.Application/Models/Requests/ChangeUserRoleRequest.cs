namespace Identity.Server.Application.Models.Requests;

public sealed record ChangeUserRoleRequest
(
    Guid UserId,
    string Role
);

