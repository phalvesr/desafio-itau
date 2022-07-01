namespace Identity.Server.Application.Models.Requests;

public sealed record CreateUserRequest
(
    string Username,
    string Password
);
