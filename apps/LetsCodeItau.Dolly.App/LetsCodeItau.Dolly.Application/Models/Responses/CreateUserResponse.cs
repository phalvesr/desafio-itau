namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record CreateUserResponse
(
    string AccessToken,
    DateTimeOffset UnixTimeExpiresAt,
    int StatusCode,
    string Errors,
    bool Succeeded
);
