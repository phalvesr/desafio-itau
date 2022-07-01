namespace Identity.Server.Application.Models.Requests;

public sealed record UserSignInRequest
(
    string Username,
    string Password
);
