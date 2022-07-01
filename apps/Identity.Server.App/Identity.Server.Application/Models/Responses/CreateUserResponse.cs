namespace Identity.Server.Application.Models.Responses;

public sealed class CreateUserResponse
{

    public string AccessToken { get; set; }
    public DateTimeOffset UnixTimeExpiresAt { get; set; }
    public Guid GlobalId { get; set; }

    public CreateUserResponse(string accessToken, DateTimeOffset unixTimeExpiresAt, Guid globalId)
    {
        AccessToken = accessToken;
        UnixTimeExpiresAt = unixTimeExpiresAt;
        GlobalId = globalId;
    }
}
