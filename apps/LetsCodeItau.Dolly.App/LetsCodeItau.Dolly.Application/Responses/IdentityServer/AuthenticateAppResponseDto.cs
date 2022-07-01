namespace LetsCodeItau.Dolly.Application.Responses.IdentityServer;

public class AuthenticateAppResponseDto
{
    public string AccessToken { get; set; } = default!;
    public DateTime UnixTimeExpiresAt { get; set; }
}
