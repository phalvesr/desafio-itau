namespace LetsCodeItau.Dolly.Application.Models.Requests.Users;

public sealed record CreateRequest
(
    string DisplayName,
    string Username,
    string Password
);

