namespace LetsCodeItau.Dolly.Application.Requests;

public sealed record UserSignInRequest
(
    string Username,
    string Password
);
