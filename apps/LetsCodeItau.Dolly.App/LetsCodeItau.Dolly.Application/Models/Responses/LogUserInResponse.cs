namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record LogUserInResponse
(
    string AccessToken,
    DateTimeOffset UnixTimeExpiresAt,
    int StatusCode,
    string Errors,
    bool Succeeded
);
