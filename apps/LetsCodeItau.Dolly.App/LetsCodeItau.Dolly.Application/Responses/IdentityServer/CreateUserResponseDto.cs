namespace LetsCodeItau.Dolly.Application.Responses.IdentityServer;

public class CreateUserResponseDto
{
    public string AccessToken { get; set; } = default!;
    public DateTimeOffset UnixTimeExpiresAt { get; set; }
    public Guid GlobalId { get; set; }
}
