namespace LetsCodeItau.Dolly.Application.Requests;

public sealed record ChangeUserRoleRequest
(
    Guid UserId,
    string Role
);
